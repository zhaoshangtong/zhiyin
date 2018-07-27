using Rays.BLL;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Zhiyin.Controllers.CompetitionZone
{
    /// <summary>
    /// 赛区
    /// </summary>
    public class ZoneController : ApiController
    {
        /// <summary>
        /// 获取所有赛区
        /// </summary>
        /// <returns></returns>
        public ApiResult GetAllZone()
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<competition_zone> zone_bll = new BaseBLL<competition_zone>();
            var zones = zone_bll.FindAllList().OrderBy(o => new {o.topsize,o.zone_id });
            apiResult.success = true;
            apiResult.message = "获取所有赛区";
            apiResult.data = zones;
            return apiResult;
        }
    }
}
