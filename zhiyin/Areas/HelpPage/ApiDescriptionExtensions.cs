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
            //��ȡcontroller��fullname
            string controllerFullName = description.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            string controllerName = description.ActionDescriptor.ControllerDescriptor.ControllerType.Name;
            //ƥ��Ŀ¼
            string[] dirName = controllerFullName.Split('.');
            //ƥ��areaName/categoryName
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
                //������areas�µ�controller,��·�ɸ�ʽ�е�{area}ȥ��
                description.RelativePath = description.RelativePath.Replace("{area}/", "");
            }
            else
            {
                //����areas�µ�controller,��·�ɸ�ʽ�е�{area}�滻Ϊ��ʵareaname
                description.RelativePath = description.RelativePath.Replace("{area}", areaName);
            }
            if (string.IsNullOrEmpty(categoryName))
            {
                //������category�µ�controller,��·�ɸ�ʽ�е�{category}ȥ��
                description.RelativePath = description.RelativePath.Replace("{category}/", "");
            }
            else
            {
                //����category�µ�controller,��·�ɸ�ʽ�е�{category}�滻Ϊ��ʵcategoryName
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
        /// ��ȡ��ĿĿ¼��Ϣ��area�ֶΣ�
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetAreaName(this ApiDescription description)
        {
            //��ȡcontroller��fullname
            string controllerFullName = description.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            //ƥ��Ŀ¼
            string[] dirName = controllerFullName.Split('.');
            //ƥ��areaName/categoryName
            string areaName = "";
            if (dirName.Length > 3)
            {

                areaName = dirName[2];

            }
            return areaName;
        }
    }
}