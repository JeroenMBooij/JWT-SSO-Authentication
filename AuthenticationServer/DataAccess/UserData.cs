using AuthenticationServer.DataAccess.DataContext;
using AuthenticationServer.DataAccess.Interfaces;
using AuthenticationServer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess _db)
        {
            this._db = _db;
        }
        public Task<List<UserVM>> GetAll<UserVM>()
        {
            throw new NotImplementedException();
        }

        public Task Insert<UserVM>(UserVM user)
        {
            string sql = @"insert into dbo.Users (Name, Email)
                            values (@Name, @Email);";

            return _db.SaveData(sql, user);
        }
    }
}
