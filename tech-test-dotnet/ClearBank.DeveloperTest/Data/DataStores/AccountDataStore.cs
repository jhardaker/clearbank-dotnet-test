using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.DataStores
{
    public class AccountDataStore : IAccountRepoitory
    {
        

        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return new Account();
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
