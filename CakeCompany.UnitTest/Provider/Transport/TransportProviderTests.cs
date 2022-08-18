using System.Collections.Generic;
using CakeCompany.Provider.Transport;
using NUnit.Framework;
using Shouldly;

namespace CakeCompany.UnitTest.Provider.Transport;

[TestFixture]
public class TransportProviderTests
{
    private readonly TransportProvider _subjectUnderTest;
    
    public TransportProviderTests()
    {
        _subjectUnderTest = new TransportProvider();
    }
    
    [TestCase(100, "Van")]
    [TestCase(2000, "Truck")]
    [TestCase(5001, "Ship")]
    public void CheckForAvailability_ShouldReturnTransportProviderName_WhenAnProductsAreSupplied(
        double quantity, 
        string expectedTransportProviderName)
    {
        var products = new List<Models.Product>
        {
            new()
            {
                Quantity = quantity
            }
        };

        var response = _subjectUnderTest.CheckForAvailability(products);

        response.ShouldNotBeNull();
        response.ShouldBe(expectedTransportProviderName);
    }
}