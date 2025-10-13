using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class CheckIfPaymentSchemeIsValidTests
    {
        [Test]
        [TestCase(PaymentScheme.Chaps, AccountStatus.Live)]
        public void ChapsServiceShouldReturnSuccessWhenAccountStatusIsLivePaymentSchemesAreChaps(PaymentScheme paymentScheme, AccountStatus accountStatus)
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = accountStatus});

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
            Assert.True(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.Chaps, AccountStatus.InboundPaymentsOnly)]
        public void ChapsServiceShouldReturnUnsuccessfulIfChapsIsDisallowedPayment(PaymentScheme paymentScheme, AccountStatus accountStatus)
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = accountStatus });

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

        [Test]
        [TestCase(PaymentScheme.Chaps, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.Chaps, AccountStatus.InboundPaymentsOnly)]
        public void ChapsServiceShouldReturnUnsuccessfulIfChapsIsDisallowedPaymentAndAccountStatusIsNotLive(PaymentScheme paymentScheme, AccountStatus accountStatus)
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = accountStatus });

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

        [Test]
        [TestCase(PaymentScheme.FasterPayments, 10, 100)]
        [TestCase(PaymentScheme.FasterPayments, 0.2343, 0.2344)]
        [TestCase(PaymentScheme.FasterPayments, 14.54, 14.60)]
        public void FasterPaymentServiceShouldReturnSuccessWhenAccountHasBalanceAndAllowedPaymentSchemesAreFasterPayments(PaymentScheme paymentScheme, decimal amount, decimal balance)
        {

            string debitorAccountNumber = "485948394";
            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = balance });

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = amount,
                CreditorAccountNumber = debitorAccountNumber,
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.True(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.FasterPayments)]
        public void FasterPaymentServiceShouldReturnUnsuccessfulIfFasterPaymenerIsDisallowedPayment(PaymentScheme paymentScheme)
        {
            string debitorAccountNumber = "485948394";

            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs , Balance = 40});

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 50,
                CreditorAccountNumber = "485948394",
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.False(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.FasterPayments, 100, 10)]
        [TestCase(PaymentScheme.FasterPayments, 0.2344, 0.2343)]
        [TestCase(PaymentScheme.FasterPayments, 14.60, 14.54)]

        public void FasterPaymentServiceShouldReturnUnsuccessfulIfBalanceIsLowerThanAmount(PaymentScheme paymentScheme, decimal amount, decimal balance)
        {
            string debitorAccountNumber = "485948394";

            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = balance });

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = amount,
                CreditorAccountNumber = "485948394",
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.False(result.Success);
        }
        [Test]
        [TestCase(PaymentScheme.Bacs)]
        public void BacPaymentServiceShouldReturnSuccess(PaymentScheme paymentScheme)
        {
            string debitorAccountNumber = "485948394";


            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs});

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "485948394",
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.True(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.Bacs)]
        public void BacPaymentServiceShouldReturnUnsuccessfulIfBacsIsDisallowedPayment(PaymentScheme paymentScheme)
        {
            string debitorAccountNumber = "485948394";

            var mock = new Mock<IAccountRepoitory>();
            mock.Setup(r => r.GetAccount(debitorAccountNumber)).Returns(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments});

            IPaymentService paymentService = new PaymentService(mock.Object);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "485948394",
                DebtorAccountNumber = debitorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = paymentScheme,
            };
            var result = paymentService.MakePayment(makePaymentRequest);
            Assert.False(result.Success);
        }
    }
}
