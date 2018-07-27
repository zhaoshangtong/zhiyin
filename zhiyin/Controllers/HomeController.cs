using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Rays.BLL;
using Rays.Common;
using Rays.Model;
using Rays.Utility;
using Zhiyin.Common;
using Zhiyin.Models;
using System.Data;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Rays.Utility.RabbitMQ;
using Zhiyin.Common.Weixin;

namespace Zhiyin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "知音投稿服务";
            WeixinOpenAPI api = new WeixinOpenAPI("wxeb8de9f325fe8ae9", "f3529b9c4625b82c4eb7f5a006de7909");
            string result = api.GetCode("http%3A%2F%2Fwww.whtlkj.cn%2Fzhiyin%2Fmine.html", "1234567");
            return View();
        }
        /// <summary>
        /// 测试分布式
        /// </summary>
        /// <returns></returns>
        public ActionResult TestNginx()
        {
            string server = Util.getServerPath();
            ViewBag.Server = server;
            return View();
        }
        public ActionResult TestSocket()
        {
            return View();
        }
        public ActionResult TestSuperSocket()
        {
            return View();
        }

        public ActionResult TestApiAouth()
        {
            return View();
        }

    }
}
