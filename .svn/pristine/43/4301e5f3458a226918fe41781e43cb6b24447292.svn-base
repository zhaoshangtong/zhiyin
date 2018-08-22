using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.DAL;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Zhiyin.Controllers.Notice
{
    /// <summary>
    /// 大赛公告
    /// </summary>
    public class NoticeController : ApiController
    {
        /// <summary>
        /// 获取所有开启大赛公告
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiPageResult GetAllNotices(int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            apiResult.pageIndex = pageIndex;
            apiResult.pageSize = pageSize;
            BaseBLL<competition_notice> bll = new BaseBLL<competition_notice>();
            var list = bll.FindList<int>(o => o.is_delete == 0&&o.is_open==1);
            int count = list.Count();
            list = list.OrderBy(o => new { o.topsize, o.competition_season_id }).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            apiResult.success = true;
            apiResult.message = "获取所有大赛公告";
            apiResult.data = list;
            apiResult.totalCount = count;
            return apiResult;
        }

        /// <summary>
        /// 根据id获取大赛公告
        /// </summary>
        /// <param name="competition_season_id">公告id</param>
        /// <returns></returns>
        public ApiResult GetNoticeById(int competition_season_id)
        {
            ApiResult apiResult = new ApiResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = competition_season_id.ToString(), Msg = "competition_season_id 不能为空值" },
                new Parameter { Value = competition_season_id.ToString(), Msg = "competition_season_id 必须是数字类型", Regex = @"^[1-9]\d*$" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            BaseBLL<competition_notice> bll = new BaseBLL<competition_notice>();
            var notice = bll.Find(o => o.competition_season_id == competition_season_id && o.is_delete == 0&&o.is_open==1);
            if (notice?.competition_season_id > 0)
            {
                apiResult.success = true;
                apiResult.message = "获取成功";
                apiResult.data = notice;
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "数据不存在";
            }
            return apiResult;
        }

        /// <summary>
        /// 新增/修改 大赛公告
        /// </summary>
        /// <param name="data">
        /// competition_notice对象
        /// </param>
        /// <returns></returns>
        public ApiResult AddNotice(dynamic data)
        {
            ApiResult apiResult = new ApiResult();
            if (Util.isNotNull(data))
            {
                BaseBLL<competition_notice> bll = new BaseBLL<competition_notice>();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var notice = Newtonsoft.Json.JsonConvert.DeserializeObject<competition_notice>(json);
                if (notice?.competition_season_id > 0)
                {
                    //修改
                    notice.update_time = DateTime.Now;
                    bll.Update(notice);
                }
                else if (Util.isNotNull(notice))
                {
                    //新增
                    notice.create_time = DateTime.Now;
                    notice.update_time = DateTime.Now;
                    bll.Add(notice);
                }
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "参数错误";
            }
            return apiResult;
        }

        /// <summary>
        /// 删除大赛公告
        /// </summary>
        /// <param name="competition_season_id"></param>
        /// <returns></returns>
        public ApiResult DelNotice(int competition_season_id)
        {
            ApiResult apiResult = new ApiResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = competition_season_id.ToString(), Msg = "competition_season_id 不能为空值" },
                new Parameter { Value = competition_season_id.ToString(), Msg = "competition_season_id 必须是数字类型", Regex = @"^[1-9]\d*$" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            BaseBLL<competition_notice> bll = new BaseBLL<competition_notice>();
            var notice = bll.Find(o => o.competition_season_id == competition_season_id && o.is_delete == 0);
            if (notice?.competition_season_id > 0)
            {
                bool result=bll.Delete(notice);
                apiResult.success = result;
                apiResult.message = result?"删除成功":"删除失败";
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "数据不存在";
            }
            return apiResult;
        }
    }
}
