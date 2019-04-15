using DailyPoetry.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DailyPoetry.Services
{
    public class KnowledgeService
    {
        private static KnowledgeContext dataContext;

        public KnowledgeService()
        {
            Task.Run(InitDatabase).Wait();
            dataContext = new KnowledgeContext();
        }

        private async Task InitDatabase()
        {
            // check whether the database file local in correct folder
            // because ef core? can not access assets folder
            // see https://social.msdn.microsoft.com/Forums/en-US/b6d7a970-0088-4bd4-aaa8-c86bca4387df/uwp-sqlitenet-path-question
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("db.sqlite") as StorageFile;
            if (dbFile == null)
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var originalDbFileUri = new Uri("ms-appx:///Assets/db.sqlite3");
                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);
                if (originalDbFile != null)
                {
                    dbFile = await originalDbFile.CopyAsync(localFolder, "db.sqlite",
                        NameCollisionOption.ReplaceExisting);
                }

                // todo: error handler
            }
        }

        public List<PoetryItem> GetAllPoetryData()
        {
            return dataContext.PoetryItems.ToList();
        }

    }
}
