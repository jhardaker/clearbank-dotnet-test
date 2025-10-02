using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        public  MakePaymentResult MakePayment(MakePaymentRequest request)
        {

            var account = new Account();

            var makePaymentResult = new MakePaymentResult();


            if (request.DataStoreTypeIsBackUp == true)
            {
                var accountDataStore = new BackupAccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }


            if (account == null)
            {
                makePaymentResult.Success = false;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        makePaymentResult.Success = false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        makePaymentResult.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        makePaymentResult.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        makePaymentResult.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        makePaymentResult.Success = false;
                    }
                    break;
            }

            if (makePaymentResult.Success)
            {
                account.Balance -= request.Amount;

                if (request.DataStoreTypeIsBackUp == true)
                {
                    var accountDataStore = new BackupAccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = new AccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
            }

            return makePaymentResult;
        }
    }
}
