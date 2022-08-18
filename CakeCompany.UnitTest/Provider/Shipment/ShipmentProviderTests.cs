using System;
using System.Collections.Generic;
using CakeCompany.Models;
using CakeCompany.Provider.Order;
using CakeCompany.Provider.Shipment;
using CakeCompany.Service.Order;
using CakeCompany.Service.Transport;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CakeCompany.UnitTest.Provider.Shipment;

[TestFixture]
public class ShipmentProviderTests
{
    private Mock<IOrderProvider>? _orderProviderMock;
    private Mock<IOrderService>? _orderServiceMock;
    private Mock<ITransportService>? _transportServiceMock;
    private Mock<ILogger>? _loggerMock;
    private ShipmentProvider? _subjectUnderTest;
    
    [SetUp]
    public void SetUpMock()
    {
        _orderProviderMock = new Mock<IOrderProvider>();
        _orderServiceMock = new Mock<IOrderService>();
        _transportServiceMock = new Mock<ITransportService>();
        _loggerMock = new Mock<ILogger>();
        _subjectUnderTest = new ShipmentProvider(
            _orderProviderMock.Object,
            _orderServiceMock.Object,
            _transportServiceMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public void GetShipment_ShouldAddTraceLog_WhenInvoked()
    {
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Trace,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Equals("Entering method: GetShipment.ShipmentProvider")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldLogException_WhenOrderProviderThrowsAnException()
    {
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Throws<Exception>();
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occured while getting shipment")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);

        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldNotProcessOrder_WhenOrderProviderThrowsAnException()
    {
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Throws<Exception>();
        
        _subjectUnderTest!.GetShipment();
        
        _orderServiceMock!.VerifyNoOtherCalls();
        _transportServiceMock!.VerifyNoOtherCalls();
    }
    
    [Test]
    public void GetShipment_ShouldAddInformationLog_WhenOrderProviderReturnsNoOrder()
    {
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(Array.Empty<Models.Order>());
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("No new orders found")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldAddInformationLog_WhenOrderProviderReturnsOrders()
    {
        var orders = new Models.Order[1];
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Processing products")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldLogException_WhenOrderServiceThrowsAnException()
    {
        var orders = new Models.Order[1];
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Throws<Exception>();
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occured while getting shipment")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);

        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldNotInitiateTransport_WhenOrderServiceThrowsAnException()
    {
        var orders = new Models.Order[1];
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Throws<Exception>();
        
        _subjectUnderTest!.GetShipment();

        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
        _transportServiceMock!.VerifyNoOtherCalls();
    }
    
    [Test]
    public void GetShipment_ShouldAddInformationLog_WhenOrderServiceDoesNotProcessOrders()
    {
        var orders = new Models.Order[1];

        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Returns(new List<Product>());
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("No products found")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldNotInitiateTransport_WhenOrderServiceDoesNotProcessOrders()
    {
        var orders = new Models.Order[1];
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Returns(new List<Product>());
        
        _subjectUnderTest!.GetShipment();

        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
        _transportServiceMock!.VerifyNoOtherCalls();
    }
    
    [Test]
    public void GetShipment_ShouldAddInformationLog_WhenOrderServiceProcessesOrders()
    {
        var orders = new Models.Order[1];
        var products = new List<Product>
        {
            new()
        };
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Returns(products);
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Delivering products")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
    }

    [Test]
    public void GetShipment_ShouldLogException_WhenTransportServiceThrowsAnException()
    {
        var orders = new Models.Order[1];
        var products = new List<Product>
        {
            new()
        };
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Returns(products);
        _transportServiceMock!.Setup(x => x.Deliver(products)).Throws<Exception>();
        
        _subjectUnderTest!.GetShipment();
        
        _loggerMock!.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occured while getting shipment")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
        _transportServiceMock.Verify(x => x.Deliver(products), Times.Once);
    }
    
    [Test]
    public void GetShipment_ShouldGetShipments()
    {
        var orders = new Models.Order[1];
        var products = new List<Product>
        {
            new()
        };
        _orderProviderMock!.Setup(x => x.GetLatestOrders()).Returns(orders);
        _orderServiceMock!.Setup(x => x.Process(orders)).Returns(products);
        _transportServiceMock!.Setup(x => x.Deliver(products));
        
        _subjectUnderTest!.GetShipment();
        
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once);
        _orderServiceMock.Verify(x => x.Process(orders), Times.Once);
        _transportServiceMock.Verify(x => x.Deliver(products), Times.Once);
    }
}