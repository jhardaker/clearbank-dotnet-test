using ClearBank.DeveloperTest.Data.DataStores;
using ClearBank.DeveloperTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]

    public class IntegrationTests
    {
        [Test]
        public void ThatAccountBackupDataStoreIsUsed()
        {
            //add account to back up data store

            var _accountRepoitory = new AccountBackupDataStore();

            var paymentService = new PaymentService(_accountRepoitory);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "test",
                DebtorAccountNumber = "test",
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = PaymentScheme.Bacs,
                DataStoreTypeIsBackUp = true
            };
            var result = paymentService.MakePayment(makePaymentRequest);

            //some test that checks back up data store to see if payment is updated
        }

        [Test]
        public void ThatAccountDataStoreIsUsed()
        {
            //add account to data store

            var _accountRepoitory = new AccountDataStore();

            var paymentService = new PaymentService(_accountRepoitory);

            var makePaymentRequest = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "test",
                DebtorAccountNumber = "test",
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = PaymentScheme.Bacs,
                DataStoreTypeIsBackUp = true
            };
            var result = paymentService.MakePayment(makePaymentRequest);

            //some test that checks data store to see if payment is updated

        }
    }
}
