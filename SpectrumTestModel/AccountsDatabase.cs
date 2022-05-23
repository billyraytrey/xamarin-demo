using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace XamarinAccountSetupDemoModel
{

    public class AccountsDatabase
    {
        static SQLiteAsyncConnection Database;

        public AccountsDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public static readonly AsyncLazy<AccountsDatabase> Instance = new AsyncLazy<AccountsDatabase>(async () =>
        {
            var instance = new AccountsDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Account>();
            return instance;
        });
        public Task<List<Account>> GetItemsAsync()
        {
            return Database.Table<Account>().ToListAsync();
        }

        public Task<List<Account>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<Account>("SELECT * FROM [Account] WHERE [Done] = 0");
        }

        public Task<Account> GetItemAsync(string uName)
        {
            return Database.Table<Account>().Where(i => i.UName == uName).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.UName))
            {
                return Database.UpdateAsync(account);
            }
            else
            {
                return Database.InsertAsync(account);
            }
        }

        public Task<int> DeleteItemAsync(Account item)
        {
            return Database.DeleteAsync(item);
        }

    }
    public static class Constants
    {
        public const string DBName = "AccountsSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache |
            SQLite.SQLiteOpenFlags.ProtectionComplete;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DBName);
            }
        }
    }
    public class AsyncLazy<T>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UName { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
    }
}
