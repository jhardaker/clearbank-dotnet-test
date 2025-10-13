using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        [Test]
       public void PaymentServiceShouldBeSuccessfullAcountIsReturned()
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs });

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = debitorAccountNumber,
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = PaymentScheme.Bacs,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.True(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.Chaps, AccountStatus.Live)]
        public void PaymentServiceShouldBeUnsuccessfullWhenNullAcountIsReturned(PaymentScheme paymentScheme, AccountStatus accountStatus)
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(value: null);

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = debitorAccountNumber,
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.False(result.Success);
        }
    }
}
