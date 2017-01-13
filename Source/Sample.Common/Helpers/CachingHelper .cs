using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Sample.Common.Entities;
using Xamarin.Auth;

namespace Sample.Common.Helpers
{
    public class CachingHelper
    {
        private static SQLiteConnection _dataConnection;

        public static void InitStorage()
        {
            var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);            
            var dataPath = Path.Combine(dataFolder, GlobleSetting.DB_NAME);

            _dataConnection = new SQLiteConnection(dataPath);
        }


        /// <summary>
        /// Saving Values to the Storage...
        /// </summary>
        public static void SaveLoginInfoToCache(LogInInfo logInInfo)
        {
            _dataConnection.CreateTable<LogInInfo>();
            var table = _dataConnection.Table<LogInInfo>();
            if (table.Any())
            {
                table.ElementAt(0).SocialId = logInInfo.SocialId;
                table.ElementAt(0).Email = logInInfo.Email;
                table.ElementAt(0).ProfileLink = logInInfo.ProfileLink;
            }
            else
            {
                _dataConnection.Insert(logInInfo);
            }
        }

        /// <summary>
        /// Reading Values from the Storage...
        /// </summary>
        public static LogInInfo GetLoginInfoFromCache()
        {
            _dataConnection.CreateTable<LogInInfo>();
            var table = _dataConnection.Table<LogInInfo>();
            if (table.Any())
            {
                return table.First();
            }
            return null;
        }

        /// <summary>
        /// Delete value from the Storage...
        /// </summary>
        public static void DeleteLoginInfoFromCache()
        {
            _dataConnection.DropTable<LogInInfo>();
        }

        public static async Task CleanAccountStorage(string serviceDomainName)
        {
            var accountStorage = AccountStore.Create();
            while (true)
            {
                var accounts = accountStorage.FindAccountsForService(serviceDomainName);
                var account = accounts.FirstOrDefault();
                if (account == null)
                    break;
                await accountStorage.DeleteAsync(account, serviceDomainName);
            }
        }
    }
}
