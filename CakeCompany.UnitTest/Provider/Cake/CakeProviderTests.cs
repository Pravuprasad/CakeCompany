using System;
using CakeCompany.Provider.Cake;
using CakeCompany.Provider.Time;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace CakeCompany.UnitTest.Provider.Cake;

[TestFixture]
public class CakeProviderTests
{
    private readonly Mock<ITimeProvider> _timeProviderMock;
    private readonly CakeProvider _subjectUnderTest;
    
    public CakeProviderTests()
    {
        _timeProviderMock = new Mock<ITimeProvider>();
        _subjectUnderTest = new CakeProvider(_timeProviderMock.Object);
    }
    
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void Check_ShouldReturnDeliveryTime_WhenCakeNameInOrderIsSupplied(int cakeType)
    {
        var time = DateTime.Now;
        _timeProviderMock.Setup(x => x.Now).Returns(time);
        var order = new Models.Order("Test", time, 1, (Models.Cake)cakeType, 1);

        var response = _subjectUnderTest.Check(order);

        switch (order.Name)
        {
            case Models.Cake.Chocolate:
                (response- time).TotalMinutes.ShouldBe(30);
                break;
            case Models.Cake.RedVelvet:
                (response- time).TotalMinutes.ShouldBe(60);
                break;
            case Models.Cake.Vanilla:
                (response- time).TotalHours.ShouldBe(15);
                break;
        }
    }
    
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void Bake_ShouldReturnProduct_WhenCakeNameInOrderIsSupplied(int cakeType)
    {
        var order = new Models.Order("Test", DateTime.Now, 1, (Models.Cake)cakeType, 1);

        var response = _subjectUnderTest.Bake(order);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(new Guid());
        response.Quantity.ShouldBe(order.Quantity);
        response.Cake.ShouldBe(order.Name);
    }
}