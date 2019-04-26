using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DailyPoetry.Models;

namespace DailyPoetry.Services
{
    public class UserRecordService
    {
        private static UserContext userContext;

        public UserRecordService()
        {
            Task.Run(InitDatabase).Wait();
        }

        private async Task InitDatabase()
        {
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("user.sqlite");
            if (dbFile == null)
            {

            }
        }
    }
}
