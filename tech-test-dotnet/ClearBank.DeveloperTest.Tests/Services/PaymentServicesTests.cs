using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentServicesTests
    {
        [Test]
        [TestCase(1.50, "8745849", "83948594", 0)]
        [TestCase(22.5, "jgjgt485468", "0", 18)]
        [TestCase(0.5, "84958392932", "-1", 36)]

        public void MakePaymentServiceShouldUseBackupDataStore(decimal amount, string creditorAccountNumber, string debtorAccountNumber, int addDays)
        {
            var paymentService = new PaymentService();

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = amount,
                CreditorAccountNumber = creditorAccountNumber,
                DebtorAccountNumber = debtorAccountNumber,
                PaymentDate = DateTime.Now.AddDays(addDays),
                PaymentScheme = PaymentScheme.Bacs,
                DataStoreTypeIsBackUp = true

            };
            paymentService.MakePayment(makePaymentRequest);
        }
    }
}
