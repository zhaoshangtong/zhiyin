using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Rays.Model;

namespace Rays.DAL
{
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前数据上下文
        /// </summary>
        /// <returns></returns>
        public static DbContext GetCurrentContext(string key)
        {
            DbContext nContext = CallContext.GetData(key + "DBContext") as DbContext;
            if (nContext == null)
            {
                nContext = GetContextByKey(key);
                CallContext.SetData(key + "DBContext", nContext);
            }
            return nContext;
        }

        internal static DbContext GetContextByKey(string key)
        {
            DbContext db = new zhiyin();
            return db;
        }
    }
}
