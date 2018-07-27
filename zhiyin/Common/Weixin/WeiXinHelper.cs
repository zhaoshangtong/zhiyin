using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rays.BLL;
using Rays.Model;
using Rays.Utility;
using Rays.Model.DBModels;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 小程序二维码生成
    /// </summary>
    public class WeiXinHelper
    {
        #region 生成小程序二维码（方形）
        /// <summary>
        ///  生成小程序二维码（方形）
        /// </summary>
        /// <param name ="path">地址</param >
        /// <param name="appcode">小程序标识</param>
        /// <returns ></returns >
        public static string CreateWxaCodeByPath(string path, string appcode)
        {
            string img_path = string.Empty;
            BaseBLL<weixin_applet> appBLL = new BaseBLL<weixin_applet>();
            weixin_applet app = appBLL.Find(x => x.appcode == appcode);
            string code_name = app.applet_name;
            string code_desc = $"{app.applet_name}小程序二维码";
            //1、进入首先记录调用情况    
            //2、根据path去搜索，如果找到则返回没找到继续执行
            //3、生成小程序图片上传到服务器
            //4、存入数据库
            #region 记录调用情况
            BaseBLL<weixin_applet_log> logBLL = new BaseBLL<weixin_applet_log>();
            weixin_applet_log log = new weixin_applet_log();
            log.appcode = appcode;
            log.title = "后台生成小程序码WeiXinHelper.CreateWxaCodeByPath";
            log.ip = Util.GetUserIp();
            log.createtime = DateTime.Now;
            log.request_url = System.Web.HttpContext.Current.Request.RawUrl;
            log.info = $"接口参数为：path:{path},appcode:{appcode}";
            logBLL.Add(log);
            #endregion

            //2、根据path去搜索，如果找到则返回没找到继续执行
            BaseBLL<weixin_applet_code> appCodeBLL = new BaseBLL<weixin_applet_code>();
            weixin_applet_code appletCode = appCodeBLL.Find(x => x.xcx_url == path && x.weixin_applet_id == app.id);
            if (appletCode != null)
            {
                img_path = appletCode.img_path;
            }
            else
            {
                //3、生成小程序图片上传到服务器
                WeixinXAPI weixinApi = new WeixinXAPI(app.appid, app.secret, app.access_token,
                    app.access_token_time?.ToString() ?? "", app.id);
                if (weixinApi.rootSuccess)
                {
                    string imgpath = weixinApi.CreateQRCode(path);
                    if (!string.IsNullOrEmpty(imgpath))
                    {
                        //4、存入数据库
                        #region 存入数据库
                        var newAppCode = new weixin_applet_code
                        {
                            code_name = code_name,
                            code_desc = code_desc,
                            weixin_applet_id = app.id,
                            create_time = DateTime.Now,
                            random = sys.getRandomStr(),
                            img_path = imgpath,
                            is_delete = 0,
                            xcx_url = path,
                            code_type = 1
                        };
                        appCodeBLL.Add(newAppCode);
                        #endregion
                    }
                    img_path = imgpath;
                }
            }
            return img_path;
        }
        #endregion
 
        #region 生成小程序码（圆形）
        /// <summary>
        ///  生成小程序码（圆形）
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="appcode">小程序标识</param>
        /// <returns></returns>
        public static string CreateCircleWxaCodeByPath(string path, string appcode)
        {
            string img_path = string.Empty;
            BaseBLL<weixin_applet> appBLL = new BaseBLL<weixin_applet>();
            weixin_applet app = appBLL.Find(x => x.appcode == appcode);
            string code_name = app.applet_name;
            string code_desc = $"{app.applet_name}小程序码";
            //1、进入首先记录调用情况    
            //2、根据path去搜索，如果找到则返回没找到继续执行
            //3、生成小程序图片上传到服务器
            //4、存入数据库
            #region 记录调用情况
            BaseBLL<weixin_applet_log> logBLL = new BaseBLL<weixin_applet_log>();
            weixin_applet_log log = new weixin_applet_log();
            log.appcode = appcode;
            log.title = "后台生成小程序码WeiXinHelper.CreateCircleWxaCodeByPath";
            log.ip = Util.GetUserIp();
            log.createtime = DateTime.Now;
            log.request_url = System.Web.HttpContext.Current.Request.RawUrl;
            log.info = $"接口参数为：path:{path},appcode:{appcode}";
            logBLL.Add(log);
            #endregion

            //2、根据path去搜索，如果找到则返回没找到继续执行
            BaseBLL<weixin_applet_code> appCodeBLL = new BaseBLL<weixin_applet_code>();
            weixin_applet_code appletCode = appCodeBLL.Find(x => x.xcx_url == path && x.weixin_applet_id == app.id && x.code_type == 2);
            if (appletCode != null)
            {
                img_path = appletCode.img_path;
            }
            else
            {
                //3、生成小程序图片上传到服务器
                WeixinXAPI weixinApi = new WeixinXAPI(app.appid, app.secret, app.access_token,
                    app.access_token_time?.ToString() ?? "", app.id);
                if (weixinApi.rootSuccess)
                {
                    string imgpath = weixinApi.CreateWxaCode(path);
                    if (!string.IsNullOrEmpty(imgpath))
                    {
                        //4、存入数据库
                        #region 存入数据库
                        var newAppCode = new weixin_applet_code
                        {
                            code_name = code_name,
                            code_desc = code_desc,
                            weixin_applet_id = app.id,
                            create_time = DateTime.Now,
                            random = sys.getRandomStr(),
                            img_path = imgpath,
                            is_delete = 0,
                            xcx_url = path,
                            code_type = 2
                        };
                        appCodeBLL.Add(newAppCode);
                        #endregion
                    }
                    img_path = imgpath;
                }
            }
            return img_path;
        }
        #endregion

        
    }

}