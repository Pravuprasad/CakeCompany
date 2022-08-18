using System;
using System.Linq;
using CakeCompany.Provider.Order;
using CakeCompany.Provider.Time;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace CakeCompany.UnitTest.Provider.Order;

[TestFixture]
public class OrderProviderTests
{
    private readonly Mock<ITimeProvider> _timeProviderMock;
    private readonly OrderProvider _subjectUnderTest;
    
    public OrderProviderTests()
    {
        _timeProviderMock = new Mock<ITimeProvider>();
        _subjectUnderTest = new OrderProvider(_timeProviderMock.Object);
    }

    [Test]
    public void GetLatestOrders_ShouldReturnOrders()
    {
        var time = DateTime.Now;
        _timeProviderMock.Setup(x => x.Now).Returns(time);
        
        var response = _subjectUnderTest.GetLatestOrders();

        response.ShouldNotBeNull();
        response.Length.ShouldBe(5);
        response[0].ClientName.ShouldBe("CakeBox");
        response[1].ClientName.ShouldBe("ImportantCakeShop");
        response.All(x=>x.Id.Equals(1)).ShouldBeTrue();
        response.All(x=>x.Quantity.Equals(120.25)).ShouldBeTrue();
    }
}