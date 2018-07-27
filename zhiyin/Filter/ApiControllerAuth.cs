using Listening.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Zhiyin.Filter;
using Zhiyin.Models;

namespace Zhiyin
{
    /// <summary>
    /// API授权控制器基类
    /// </summary>
    [ApiAuth2]
    public class ApiControllerAuth : ApiController
    {

    }
}