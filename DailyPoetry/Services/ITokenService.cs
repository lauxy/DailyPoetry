using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Services
{
    /// <summary>
    /// Token服务。
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 获取用户的Token。
        /// </summary>
        /// <returns>用户的Token</returns>
        Task<TokenData> GetUsersToken();
    }

    public class TokenData
    {
        public string status { get; set; }
        public string data { get; set; }
    }

}
