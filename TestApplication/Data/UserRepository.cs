using System;
using TestApplication.Data.DbModels;
using System.Linq;

namespace TestApplication.Data
{
    public class UserRepository : IUserRepository
    {
        DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Account GetAccountByUsername(string username) {
            var user = (from account in _dataContext.accounts
                            where account.Username == username
                            select account).FirstOrDefault();

            return user;
        }

        public Account GetAccountById(int id) {

            var user = (from account in _dataContext.accounts
                        where account.UserrId == id
                        select account).FirstOrDefault();

            return user;
        }
    }
}
