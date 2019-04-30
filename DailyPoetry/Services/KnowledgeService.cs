using DailyPoetry.Models.KnowledgeModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Collections;
using System.Threading;

namespace DailyPoetry.Services
{ 
    /// <summary>
    /// * 线程安全问题（等问题）没有研究
    /// * 此外需要注意 IEnumerable 对象并没有将数据装入内存，
    ///   EF Core只是保存了获取数据的路径，调用 ToList 等方法才生效将数据调入内存。
    ///   在此之前释放context，可能会导致崩溃（未测试）
    /// </summary>
    public class KnowledgeService
    {
        public KnowledgeService()
        {
            Task.Run(InitDatabase).Wait();
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

        /// <summary>
        /// 根据 Id 获取摘要
        /// </summary>
        /// <param name="id">诗词的Id</param>
        /// <returns></returns>
        public string GetAbstractById(int id)
        {
            using (var context = new KnowledgeContext())
            {
                var onePoem = context.PoetryItems
                    .Single(poetryItem => poetryItem.Id == id);
                return onePoem != null ? 
                    GetAbstractByString(onePoem.Content) : "";
            }
        }

        /// <summary>
        /// 从文本获取摘要
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GetAbstractByString(string content)
        {
            return "Placeholder";
        }

        /// <summary>
        /// 根据id获取简略信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SimplifiedPoetryItem GetOneById(int id)
        {
            using (var context = new KnowledgeContext())
            {
                var oneSimplifiedPoetry = context.PoetryItems
                    .Where(poetryItem => poetryItem.Id == id)
                    .Select(poetryItem => new SimplifiedPoetryItem()
                    {
                        Id = poetryItem.Id,
                        Name = poetryItem.Name,
                        Dynasty = poetryItem.Dynasty,
                        AuthorName = poetryItem.AuthorName,
                        Abstract = GetAbstractByString(poetryItem.Content)
                    });
                return oneSimplifiedPoetry.Any() ? oneSimplifiedPoetry.First() : null;
            }
        }
    }

    /// <summary>
    /// KnowledgeService的所有查询函数返回DataSource，数据通过DataSource存取
    /// 注意DataSource只能存在于context中
    /// </summary>
    public class DataSource : IIncrementalSource<SimplifiedPoetryItem>
    {

        public Task<IEnumerable<SimplifiedPoetryItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
