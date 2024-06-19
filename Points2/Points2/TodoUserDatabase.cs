using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points
{
    public class TodoUserDatabase
    {
        SQLiteAsyncConnection Database;
        public TodoUserDatabase()
        {

        }
        async Task Init()
        {
            if(Database is not null) {return;}
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<User>();
        }
        public async Task<List<User>> GetUsersAsync()
        {
            await Init();
            return await Database.Table<User>().ToListAsync();
        }

    }
}
