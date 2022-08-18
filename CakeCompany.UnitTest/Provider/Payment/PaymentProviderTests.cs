using System;
using CakeCompany.Provider.Payment;
using NUnit.Framework;
using Shouldly;

namespace CakeCompany.UnitTest.Provider.Payment;

[TestFixture]
public class PaymentProviderTests
{
    private readonly PaymentProvider _subjectUnderTest;
    
    public PaymentProviderTests()
    {
        _subjectUnderTest = new PaymentProvider();
    }

    [TestCase("Important", false, true)]
    [TestCase("Important ", false, true)]
    [TestCase(" Important", false, true)]
    [TestCase(" Important ", false, true)]
    [TestCase("ImportantTest", false, true)]
    [TestCase("TestImportantTest", false, true)]
    [TestCase("Test", true, true)]
    [TestCase("", true, true)]
    [TestCase(" ", true, true)]
    [TestCase(null, true, true)]
    public void Process_ShouldProcessPayment_WhenAnOrderIsSupplied(
        string clientName, 
        bool expectedHasCreditLimit,
        bool expectedIsIsSuccessful)
    {
        var order = new Models.Order(clientName, DateTime.Now, 1, Models.Cake.Chocolate, 1);

        var response = _subjectUnderTest.Process(order);

        response.ShouldNotBeNull();
        response.HasCreditLimit.ShouldBe(expectedHasCreditLimit);
        response.IsSuccessful.ShouldBe(expectedIsIsSuccessful);
    }
}