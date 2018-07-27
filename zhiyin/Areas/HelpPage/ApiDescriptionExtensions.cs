using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Description;

namespace Zhiyin.Areas.HelpPage
{
    public static class ApiDescriptionExtensions
    {
        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="ApiDescription"/>. E.g. "Get-Values-id_name" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="ApiDescription"/>.</param>
        /// <returns>The ID as a string.</returns>
        public static string GetFriendlyId(this ApiDescription description)
        {
            //获取controller的fullname
            string controllerFullName = description.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            string controllerName = description.ActionDescriptor.ControllerDescriptor.ControllerType.Name;
            //匹配目录
            string[] dirName = controllerFullName.Split('.');
            //匹配areaName/categoryName
            string areaName = "";
            string categoryName = "";
            if (dirName.Length > 3)
            {
                if (dirName.Length == 4)
                {
                    areaName = dirName[2];
                }
                else if (dirName.Length == 5)
                {
                    areaName = dirName[2];
                    categoryName = dirName[3];
                }
            }

            if (string.IsNullOrEmpty(areaName))
            {
                //若不是areas下的controller,将路由格式中的{area}去掉
                description.RelativePath = description.RelativePath.Replace("{area}/", "");
            }
            else
            {
                //若是areas下的controller,将路由格式中的{area}替换为真实areaname
                description.RelativePath = description.RelativePath.Replace("{area}", areaName);
            }
            if (string.IsNullOrEmpty(categoryName))
            {
                //若不是category下的controller,将路由格式中的{category}去掉
                description.RelativePath = description.RelativePath.Replace("{category}/", "");
            }
            else
            {
                //若是category下的controller,将路由格式中的{category}替换为真实categoryName
                description.RelativePath = description.RelativePath.Replace("{category}", categoryName);
            }
            string path = description.RelativePath;
            string[] urlParts = path.Split('?');
            string localPath = urlParts[0];
            string queryKeyString = null;
            if (urlParts.Length > 1)
            {
                string query = urlParts[1];
                string[] queryKeys = HttpUtility.ParseQueryString(query).AllKeys;
                queryKeyString = String.Join("_", queryKeys);
            }

            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}",
                description.HttpMethod.Method,
                localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            if (queryKeyString != null)
            {
                friendlyPath.AppendFormat("_{0}", queryKeyString.Replace('.', '-'));
            }
            return friendlyPath.ToString();
        }

        /// <summary>
        /// 获取项目目录信息（area字段）
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetAreaName(this ApiDescription description)
        {
            //获取controller的fullname
            string controllerFullName = description.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            //匹配目录
            string[] dirName = controllerFullName.Split('.');
            //匹配areaName/categoryName
            string areaName = "";
            if (dirName.Length > 3)
            {

                areaName = dirName[2];

            }
            return areaName;
        }
    }
}