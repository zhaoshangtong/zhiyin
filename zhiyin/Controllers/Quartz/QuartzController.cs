using Rays.BLL;
using Rays.BLL.Article;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zhiyin.Common;

namespace Zhiyin.Controllers.Quartz
{
    /// <summary>
    /// 任务调度接口
    /// </summary>
    public class QuartzController : ApiController
    {
        /// <summary>
        /// 初赛数据自动评选（按赛区取前几十名）
        /// </summary>
        /// <returns></returns>
        public ApiResult UpdatePreliminariesData()
        {
            ApiResult apiResult = new ApiResult();
            //开启的赛区
            BaseBLL<competition_notice> notice_bll = new BaseBLL<competition_notice>();
            var notice = notice_bll.Find(o => o.is_delete == 0 && o.is_open == 1);
            if (notice?.competition_season_id > 0)
            {
                //是否是初赛截至时间(即需要同步数据的时间)
                BaseBLL<competition_date> date_bll = new BaseBLL<competition_date>();
                var competiton_date = date_bll.Find(o => o.competition_season_id == notice.competition_season_id);
                if (competiton_date?.competition_date_id > 0)
                {
                    //时间是否是今天
                    if (Util.isNotNull(competiton_date.preliminaries_date)&&competiton_date.preliminaries_date.Value.Date == DateTime.Now.Date)
                    {
                        //找出前几百名作品，更新作品状态（必须是审核通过的）
                        ArticleBLL bll = new ArticleBLL();
                        apiResult = bll.GetPreliminariesAndUpdateByZone(notice.competition_season_id, 200);
                        LogHelper.Info("同步更新初赛作品时间："+DateTime.Now+",结果："+Newtonsoft.Json.JsonConvert.SerializeObject(apiResult));
                    }
                }
                else
                {
                    LogHelper.Error(DateTime.Now + ":初赛数据自动评选，没有找到初赛结束时间");
                    apiResult.success = false;
                    apiResult.message = "没有找到初赛结束时间";
                }

            }
            else
            {
                LogHelper.Error(DateTime.Now+":初赛数据自动评选，没有找到开启的赛季");
                apiResult.success = false;
                apiResult.message = "没有找到开启的赛季";
            }
            return apiResult;
        }

        /// <summary>
        /// 半决赛数据自动评选
        /// </summary>
        /// <returns></returns>
        public ApiResult UpdateSemifinalsData()
        {
            ApiResult apiResult = new ApiResult();
            //开启的赛区
            BaseBLL<competition_notice> notice_bll = new BaseBLL<competition_notice>();
            var notice = notice_bll.Find(o => o.is_delete == 0 && o.is_open == 1);
            if (notice?.competition_season_id > 0)
            {
                //是否是半决赛截至时间(即需要同步数据的时间)
                BaseBLL<competition_date> date_bll = new BaseBLL<competition_date>();
                var competiton_date = date_bll.Find(o => o.competition_season_id == notice.competition_season_id);
                if (competiton_date?.competition_date_id > 0)
                {
                    //时间是否是今天
                    if (Util.isNotNull(competiton_date.semifinals_date) && competiton_date.semifinals_date.Value.Date == DateTime.Now.Date)
                    {
                        //找出前几百名作品，更新作品状态（必须是审核通过的）
                        ArticleBLL bll = new ArticleBLL();
                        apiResult = bll.GetSemifinalsAndUpdate(notice.competition_season_id, 200);
                        LogHelper.Info("同步更新半决赛作品时间：" + DateTime.Now + ",结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(apiResult));
                    }
                }
                else
                {
                    LogHelper.Error(DateTime.Now + ":半决赛数据自动评选，没有找到半决赛结束时间");
                    apiResult.success = false;
                    apiResult.message = "没有找到半决赛结束时间";
                }

            }
            else
            {
                LogHelper.Error(DateTime.Now + ":初赛数据自动评选，没有找到开启的赛季");
                apiResult.success = false;
                apiResult.message = "没有找到开启的赛季";
            }
            return apiResult;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public ApiResult Test(dynamic data)
        {
            ApiResult apiResult = new ApiResult();
            LogHelper.Info("调度测试Test：" + DateTime.Now+",参数为："+data);
            apiResult.success = true;
            apiResult.message = "执行成功";
            return apiResult;
        }
        
    }
}
