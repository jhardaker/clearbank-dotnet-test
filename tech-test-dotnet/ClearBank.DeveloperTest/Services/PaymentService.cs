using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.DataStores;
using ClearBank.DeveloperTest.Types;
using System;
using System.Configuration;
using System.Security.Principal;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private IAccountRepoitory _accountRepoitory = null;

        public PaymentService(IAccountRepoitory accountRepoitory)
        {
            _accountRepoitory = accountRepoitory;

        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var makePaymentResult = new MakePaymentResult();

           // _accountRepoitory = request.DataStoreTypeIsBackUp == true ? new AccountBackupDataStore() : new AccountDataStore();

            var account = _accountRepoitory.GetAccount(request.DebtorAccountNumber);
            
            if (account == null)
            {
                makePaymentResult.Success = false;
            }

            makePaymentResult.Success = CheckIfPaymentIsValid(request.PaymentScheme, account, request.Amount);

            if (makePaymentResult.Success)
            {
                account.Balance -= request.Amount;
                _accountRepoitory.UpdateAccount(account);
            }

            return makePaymentResult;
        }
        private static bool CheckIfPaymentIsValid(PaymentScheme scheme, Account account, decimal ammout)
        {
            bool isSuccess = scheme switch
            {
                PaymentScheme.Bacs =>  account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs) ? true : false,
                PaymentScheme.FasterPayments => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) ? true : false && account.Balance < ammout ? false : true,
                PaymentScheme.Chaps => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) ? true : false &&  account.Status == AccountStatus.Live ? true : false,
                _ => true,
            };


            return isSuccess;
        }
    }
}
