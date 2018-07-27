using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rays.Model.Sys;
using Rays.BLL.Adviser;
using System.Web;
using Rays.BLL;
using Rays.Model.DBModels;
using Zhiyin.Common;
using System.IO;
using Rays.BLL.Article;

namespace Zhiyin.Controllers.Adviser
{
    /// <summary>
    /// 赛程管理
    /// </summary>
    public class CompetitionController : ApiController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult AdviserLoginIn()
        {
            string user_name = HttpContext.Current.Request.Form["user_name"];
            string password = HttpContext.Current.Request.Form["password"];
            ApiResult apiResult = new ApiResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = user_name, Msg = "user_name 不能为空值" },
                new Parameter { Value = password, Msg = "password 不能为空值" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            BaseBLL<adviser> bll = new BaseBLL<adviser>();
            var adviser = bll.Find(o => o.user_name == user_name && o.password == password);
            if (adviser?.adviser_id > 0)
            {
                apiResult.success = true;
                apiResult.message = "登陆成功";
                apiResult.data = adviser;
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "登陆失败";
            }
            return apiResult;
        }
        /// <summary>
        /// 作品管理查询
        /// </summary>
        /// <param name="keyword">查询的关键字：作品编号/标题/作者</param>
        /// <param name="state">-1全部，0初始，1审核不通过，2审核通过,3半决赛，4决赛</param>
        /// <param name="zone_id">0全部</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="orderby">排序字段：vote按票数顺序排，votedesc按票数倒叙排，date按投稿时间顺序排，datedesc按投稿时间倒叙排</param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">10</param>
        /// <returns></returns>
        public ApiPageResult GetArticleList(string keyword=null,int state=-1,int zone_id=0,DateTime? start=null,DateTime? end=null, string orderby = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            //赛季
            BaseBLL<competition_notice> notice_bll = new BaseBLL<competition_notice>();
            var notice = notice_bll.Find(o => o.is_delete == 0 && o.is_open == 1);
            if (notice?.competition_season_id > 0)
            {
                CompetitionBLL bll = new CompetitionBLL();
                return bll.GetArticleList(keyword, state,notice.competition_season_id ,zone_id, start, end, orderby, pageIndex, pageSize);
            }
            else
            {
                return new ApiPageResult()
                {
                    success=false,
                    message="当前没有开启的赛季"
                };
            }
        }

        /// <summary>
        /// 审核/退回
        /// formdata
        /// state:1退回，2审核
        /// return_tag（退回标签）:xxxx,xxxx,xxxx
        /// return_remark（退回理由备注）:xxxxxxxxxxx
        /// ids:1,2,3,4,5,6,7,8,9,10
        /// </summary>
        /// <returns></returns>
        public ApiResult SubmitArticle()
        {
            ApiResult apiResult = new ApiResult();
            string _state = HttpContext.Current.Request.Form["state"];
            string return_tag = HttpContext.Current.Request.Form["return_tag"];
            string return_remark = HttpContext.Current.Request.Form["return_remark"];
            string _article_id = HttpContext.Current.Request.Form["ids"];
            var checkResult = Util.CheckParameters(
                new Parameter { Value = _state, Msg = "state 不能为空值" },
                new Parameter { Value = _state, Msg = "state 必须是数字类型", Regex = @"^[1-9]\d*$" },
                new Parameter { Value = _article_id, Msg = "ids 不能为空值" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            int state = int.Parse(_state);
            List<int> id_list = new List<int>();
            string[] article_ids = _article_id.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var id in article_ids)
            {
                int _id = 0;
                int.TryParse(id, out _id);
                if (_id > 0)
                {
                    id_list.Add(_id);
                }
            }
            BaseBLL<article_states> state_bll = new BaseBLL<article_states>();
            var article_states = state_bll.FindList<int>(o => id_list.Contains(o.article_id));
            if (state == 1)
            {
                List<article_states> back_list = new List<article_states>();
                foreach(var article_state in article_states)
                {
                    article_state.article_state = 1;
                    article_state.update_time = DateTime.Now;
                    article_state.return_tag = return_tag;
                    article_state.return_remark = return_remark;
                    back_list.Add(article_state);
                }
                state_bll.UpdateMore(back_list);
            }
            else if (state == 2)
            {
                List<article_states> sumbit_list = new List<article_states>();
                foreach (var article_state in article_states)
                {
                    article_state.article_state = 2;
                    article_state.update_time = DateTime.Now;
                    article_state.return_remark = "";
                    article_state.return_tag = "";
                    sumbit_list.Add(article_state);
                }
                state_bll.UpdateMore(sumbit_list);
            }
            else
            {
                return new ApiResult()
                {
                    success=false,
                    message="参数错误"
                };
            }
            apiResult.success = true;
            apiResult.message = state == 1 ? "退回成功" : "审核成功";
            return apiResult;
        }

        /// <summary>
        /// 获取作品内容并获取上一篇，下一篇的id
        /// </summary>
        /// <param name="state">2初赛，3半决赛，4决赛</param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult GetArticleInfoByAdmin(int state,int article_id)
        {
            ApiPageResult apiResult = new ApiPageResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = state.ToString(), Msg = "state 不能为空值" },
                new Parameter { Value = state.ToString(), Msg = "state 必须是数字类型", Regex = @"^[1-9]\d*$" },
                new Parameter { Value = article_id.ToString(), Msg = "article_id 不能为空值" },
                new Parameter { Value = article_id.ToString(), Msg = "article_id 必须是数字类型", Regex = @"^[1-9]\d*$" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            ArticleBLL bll = new ArticleBLL();
            return bll.GetArticleInfoByAdmin(state,article_id);
        }
        /// <summary>
        /// 作品打分
        /// </summary>
        /// <param name="article_id"></param>
        /// <param name="score">分数</param>
        /// <param name="name">专家名字</param>
        /// <param name="sign">0是编辑，1是专家</param>
        /// <returns></returns>
        public ApiResult AddScore(int article_id,double score,string name,int sign=0)
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<article_states> state_bll = new BaseBLL<article_states>();
            var article_states = state_bll.Find(o => o.article_id == article_id);
            if (article_states == null)
            {
                return new ApiResult()
                {
                    success=false,
                    message="数据不存在"
                };
            }
            if (sign == 0)
            {
                article_states.adviser_score = score;
                article_states.update_time = DateTime.Now;
                state_bll.Update(article_states);
            }
            else if (sign == 1)
            {
                article_states.expert_name = name;
                article_states.expert_score = score;
                article_states.update_time = DateTime.Now;
                state_bll.Update(article_states);
            }
            else
            {
                return new ApiResult()
                {
                    success=true,
                    message="参数错误"
                };
            }
            apiResult.success = true;
            apiResult.message = "打分成功";
            return apiResult;
        }

        /// <summary>
        /// 导出比赛记录
        /// </summary>
        /// <param name="article_state">2：初赛，3是半决赛，4是决赛</param>
        /// <returns></returns>
        public ApiResult ExportCompetitionRecord(int article_state=2)
        {
            ApiResult apiResult = new ApiResult();
            //赛季
            BaseBLL<competition_notice> notice_bll = new BaseBLL<competition_notice>();
            var notice = notice_bll.Find(o => o.is_delete == 0 && o.is_open == 1);
            if (notice?.competition_season_id > 0)
            {
                CompetitionBLL bll = new CompetitionBLL();
                var dt = bll.GetAllArticle(article_state, notice.competition_season_id);
                //导出excel
                string folder = HttpContext.Current.Server.MapPath("/download/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string excel_file = folder + sys.getRandomStr() + ".xls";
                Rays.Utility.NPOIHelper.GridToExcelByNPOI(dt, excel_file, "导出结果");
                if (File.Exists(excel_file))
                {
                    string file=Util.UploadFileToServices(excel_file);
                    apiResult.success = true;
                    apiResult.message = "导出成功";
                    apiResult.data = file;
                }
                else
                {
                    return new ApiResult()
                    {
                        success=false,
                        message="导出失败"
                    };
                }
            }
            else
            {
                return new ApiPageResult()
                {
                    success = false,
                    message = "当前没有开启的赛季"
                };
            }
            return apiResult;
        }

        /// <summary>
        /// 获取初赛/半决赛的触发时间
        /// </summary>
        /// <returns></returns>
        public ApiResult GetCompetitionDate()
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<job_detail> job_detail_bll = new BaseBLL<job_detail>();
            var job = job_detail_bll.FindAllList();
            apiResult.success = true;
            apiResult.data = job;
            apiResult.message = "获取初赛/半决赛的触发时间";
            return apiResult;
        }
        /// <summary>
        /// 修改初赛时间和半决赛触发时间
        /// </summary>
        /// <param name="preliminaries_date">初赛同步时间</param>
        /// <param name="semifinals_date">决赛同步时间</param>
        /// <returns></returns>
        public ApiResult UpdateCompetitonDate(DateTime? preliminaries_date ,DateTime? semifinals_date)
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<job_detail> job_detail_bll = new BaseBLL<job_detail>();
            //修改初赛
            if (Util.isNotNull(preliminaries_date))
            {
                var job_detail = job_detail_bll.Find(o => o.job_name .Contains("preliminaries_"));
                if (job_detail?.id > 0)
                {
                    job_detail.status = "MODIFY";
                    job_detail.job_name = "preliminaries_"+sys.getRandomStr();
                    job_detail.start_time = preliminaries_date.Value.Date;
                    job_detail.end_time = preliminaries_date.Value.AddDays(1).Date;
                    job_detail_bll.Update(job_detail);
                }
                else
                {
                    LogHelper.Error("修改初赛时间错误：" + DateTime.Now);
                    return new ApiResult()
                    {
                        success = false,
                        message = "修改初赛时间错误"
                    };
                }
            }

            //修改半决赛
            if (Util.isNotNull(semifinals_date))
            {
                var job_detail = job_detail_bll.Find(o => o.job_name .Contains("semifinals_"));
                if (job_detail?.id > 0)
                {
                    job_detail.status = "MODIFY";
                    job_detail.job_name = "semifinals_" + sys.getRandomStr();
                    job_detail.start_time= semifinals_date.Value.Date;//开始时间
                    job_detail.end_time = semifinals_date.Value.AddDays(1).Date;//结束时间
                    job_detail_bll.Update(job_detail);
                }
                else
                {
                    LogHelper.Error("修改半决赛时间错误：" + DateTime.Now);
                    return new ApiResult()
                    {
                        success = false,
                        message = "修改半决赛时间错误"
                    };
                }
            }
            apiResult.success = true;
            apiResult.message = "修改成功";
            return apiResult;
        }
    }
}
