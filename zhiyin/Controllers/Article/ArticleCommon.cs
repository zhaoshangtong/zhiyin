using Rays.BLL;
using Rays.Model.DBModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Zhiyin.Common;

namespace Zhiyin.Controllers.Article
{
    public class ArticleCommon
    {
        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static string GetSaveFilePath(string file_name)
        {
            string folder = HttpContext.Current.Server.MapPath("/upload/");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder+sys.getRandomStr()+Path.GetExtension(file_name);
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string UploadFile(HttpPostedFile file)
        {
            string file_name = file.FileName;
            string file_path = GetSaveFilePath(file_name);
            file.SaveAs(file_path);
            return Util.UploadFileToServices(file_path);
        }


        
        
    }
}