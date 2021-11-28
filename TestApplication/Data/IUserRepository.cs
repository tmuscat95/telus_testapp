using System;
using TestApplication.Data.DbModels;

namespace TestApplication.Data
{
    public interface IUserRepository
    {
        public Account GetAccountByUsername(string username);

        public Account GetAccountById(int id);
    }
}
