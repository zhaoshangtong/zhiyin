using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using Rays.Utility.DataTables;
using System.Data;

namespace Rays.DAL.Adviser
{
    public class CompetitionDAL
    {
        private DbContext db = ContextFactory.GetCurrentContext("");

        #region 作品管理查询
        /// <summary>
        /// 作品管理查询
        /// </summary>
        /// <param name="keyword">查询的关键字：作品编号/标题/作者</param>
        /// <param name="state">-1全部，0初始，1审核不通过，2审核通过,3半决赛，4决赛</param>
        /// <param name="zone_id">0全部</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="orderby">排序字段：vote 按票数顺序排，votedesc 按票数倒叙排，date 按投稿时间顺序排，datedesc 按投稿时间倒叙排</param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">10</param>
        /// <returns></returns>
        public ApiPageResult GetArticleList(string keyword = null, int state = -1,int competition_season_id=0, int zone_id = 0, DateTime? start = null, DateTime? end = null, string orderby = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            apiResult.pageIndex = pageIndex;
            apiResult.pageSize = pageSize;
            var article_list = from a in db.Set<articles>()
                               join b in db.Set<author_info>() on a.uid equals b.uid
                               join c in db.Set<article_states>() on a.article_id equals c.article_id
                               join d in db.Set<article_competition_season>() on a.article_id equals d.article_id
                               where d.competiontion_season_id==competition_season_id
                               select new {a.article_id,d.zone_id,a.article_no,a.article_title,b.user_name,a.create_time,c.article_state,c.adviser_score,c.expert_score,c.expert_name,c.return_tag,c.return_remark };
            if (state >= 0)
            {
                article_list = article_list.Where(o => o.article_state == state);
            }
            else
            {
                article_list = article_list.Where(o => o.article_state <= 2);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                article_list = article_list.Where(o => o.article_no.Contains(keyword) || o.user_name.Contains(keyword) || o.article_title.Contains(keyword));
            }
            if (zone_id > 0)
            {
                article_list = article_list.Where(o => o.zone_id == zone_id);
            }
            if (start != null)
            {
                article_list = article_list.Where(o => o.create_time>=start);
            }
            if (end != null)
            {
                end = end.Value.AddDays(1);
                article_list = article_list.Where(o => o.create_time < end);
            }
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id=b.Key, vote_count = (int?)b.Count() };
            var data = from a in article_list join b in vote_count on a.article_id equals b.article_id into ab1 from ab in ab1.DefaultIfEmpty() select new { article=a, vote_count=ab.vote_count??0,banjuesai=a.adviser_score*0.6 + ab.vote_count??0*0.4 };
            int count = data.Count();
            //初赛作品
            if (state <= 2)
            {
                if (orderby == "vote")
                {
                    data = data.OrderBy(o => o.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    data = data.OrderByDescending(o => o.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    data = data.OrderBy(o => o.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    data = data.OrderByDescending(o => o.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    data = data.OrderBy(o => o.article.article_id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = data;
                apiResult.totalCount = count;
            }
            //半决赛
            else if (state == 3)
            {

                var _data = data.AsEnumerable().OrderByDescending(o => o.banjuesai).Select((o, index) => new {row=index+1,data=o });
                if (orderby == "vote")
                {
                    _data = _data.OrderBy(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    _data = _data.OrderBy(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    _data = _data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = _data;
                apiResult.totalCount = count;
            }
            //决赛
            else if (state == 4)
            {
                var _data = data.AsEnumerable().OrderByDescending(o => o.article.expert_score).Select((o, index) => new { row = index+1, data = o });
                if (orderby == "vote")
                {
                    _data = _data.OrderBy(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    _data = _data.OrderBy(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    _data = _data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                
                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = _data;
                apiResult.totalCount = count;
            }
            else
            {
                return new ApiPageResult()
                {
                    success=false,
                    message="参数错误"
                };
            }
            
            return apiResult;
        }
        #endregion

        #region 作品管理查询（弃用）
        /// <summary>
        /// 作品管理查询
        /// </summary>
        /// <param name="keyword">查询的关键字：作品编号/标题/作者</param>
        /// <param name="state">-1全部，0初始，1审核不通过，2审核通过,3半决赛，4决赛</param>
        /// <param name="zone_id">0全部</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="orderby">排序字段：vote 按票数顺序排，votedesc 按票数倒叙排，date 按投稿时间顺序排，datedesc 按投稿时间倒叙排</param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">10</param>
        /// <returns></returns>
        public ApiPageResult GetArticleList1(string keyword = null, int state = -1, int competition_season_id = 0, int zone_id = 0, DateTime? start = null, DateTime? end = null, string orderby = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            apiResult.pageIndex = pageIndex;
            apiResult.pageSize = pageSize;
            var article_list = from a in db.Set<articles>()
                               join b in db.Set<author_info>() on a.uid equals b.uid
                               join c in db.Set<article_states>() on a.article_id equals c.article_id
                               join d in db.Set<article_competition_season>() on a.article_id equals d.article_id
                               where d.competiontion_season_id == competition_season_id
                               select new { a.article_id, d.zone_id, a.article_no, a.article_title, b.user_name, a.create_time, c.article_state, c.adviser_score, c.expert_score, c.expert_name, c.return_tag, c.return_remark };
            if (state >= 0)
            {
                article_list = article_list.Where(o => o.article_state == state);
            }
            else
            {
                article_list = article_list.Where(o => o.article_state <= 2);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                article_list = article_list.Where(o => o.article_no.Contains(keyword) || o.user_name.Contains(keyword) || o.article_title.Contains(keyword));
            }
            if (zone_id > 0)
            {
                article_list = article_list.Where(o => o.zone_id == zone_id);
            }
            if (start != null)
            {
                article_list = article_list.Where(o => o.create_time >= start);
            }
            if (end != null)
            {
                article_list = article_list.Where(o => o.create_time >= end);
            }
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id = b.Key, vote_count = (int?)b.Count() };
            var data = from a in article_list join b in vote_count on a.article_id equals b.article_id into ab1 from ab in ab1.DefaultIfEmpty() select new { article = a, vote_count = ab.vote_count ?? 0, banjuesai = a.adviser_score * 0.6 + ab.vote_count ?? 0 * 0.4 };
            int count = data.Count();
            //初赛作品
            if (state <= 2)
            {
                if (orderby == "vote")
                {
                    data = data.OrderBy(o => o.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    data = data.OrderByDescending(o => o.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    data = data.OrderBy(o => o.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    data = data.OrderByDescending(o => o.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    data = data.OrderBy(o => o.article.article_id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = data;
                apiResult.totalCount = count;
            }
            //半决赛
            else if (state == 3)
            {

                var _data = data.ToList().OrderByDescending(o => o.banjuesai).Select((o, index) => new { row = index, data = o });
                if (orderby == "vote")
                {
                    _data = _data.OrderBy(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    _data = _data.OrderBy(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    _data = _data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = _data;
                apiResult.totalCount = count;
            }
            //决赛
            else if (state == 4)
            {
                var _data = data.ToList().OrderByDescending(o => o.article.expert_score).Select((o, index) => new { row = index, data = o });
                if (orderby == "vote")
                {
                    _data = _data.OrderBy(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "votedesc")
                {
                    _data = _data.OrderByDescending(o => o.data.vote_count).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "date")
                {
                    _data = _data.OrderBy(o => o.data.article.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                else if (orderby == "datedesc")
                {
                    _data = _data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }

                apiResult.success = true;
                apiResult.message = "作品管理查询";
                apiResult.data = _data;
                apiResult.totalCount = count;
            }
            else
            {
                return new ApiPageResult()
                {
                    success = false,
                    message = "参数错误"
                };
            }

            return apiResult;
        }
        #endregion

        #region 获取所有作品 ，导出excel
        /// <summary>
        /// 获取所有作品
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public DataTable GetAllArticle(int state, int competition_season_id = 0)
        {
            var article_list = from a in db.Set<articles>()
                               join b in db.Set<author_info>() on a.uid equals b.uid
                               join c in db.Set<article_states>() on a.article_id equals c.article_id
                               join d in db.Set<article_competition_season>() on a.article_id equals d.article_id
                               where d.competiontion_season_id == competition_season_id
                               select new { a.article_id, d.zone_id, a.article_no, a.article_title, b.user_name, a.create_time, c.article_state, c.adviser_score, c.expert_score, c.expert_name, c.return_tag, c.return_remark };

            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id = b.Key, vote_count = (int?)b.Count() };
            var data = from a in article_list join b in vote_count on a.article_id equals b.article_id into ab1 from ab in ab1.DefaultIfEmpty() select new { article = a, vote_count = ab.vote_count ?? 0, banjuesai = a.adviser_score * 0.6 + ab.vote_count ?? 0 * 0.4 };

            if (state == 2)
            {
                #region 初赛
                data = data.Where(o => o.article.article_state <= state).OrderByDescending(o=>o.vote_count);
                List<PreliminariesData> list = new List<PreliminariesData>();
                foreach (var row in data)
                {
                    PreliminariesData preli = new PreliminariesData();
                    preli.作品名称 = row.article.article_title;
                    preli.作品编号 = row.article.article_no;
                    preli.作者姓名 = row.article.user_name;
                    preli.投稿时间 = row.article.create_time.HasValue ? row.article.create_time.Value.ToString() : "";
                    if (row.article.article_state == 0)
                    {
                        preli.状态 = "未审核";
                    }
                    else if (row.article.article_state == 1)
                    {
                        preli.状态 = "审核未通过";
                    }
                    else if (row.article.article_state == 2)
                    {
                        preli.状态 = "审核通过";
                    }
                    preli.票数 = row.vote_count.ToString();
                    list.Add(preli);
                }
                return  list.ToDataTable();
                #endregion
            }
            else if (state == 3)
            {
                #region 初赛
                data = data.Where(o => o.article.article_state == state).OrderByDescending(o=>o.banjuesai);
                List<SemifinalsData> list = new List<SemifinalsData>();
                int i = 1;
                foreach (var row in data)
                {
                    SemifinalsData semi = new SemifinalsData();
                    semi.作品名称 = row.article.article_title;
                    semi.作品编号 = row.article.article_no;
                    semi.作者姓名 = row.article.user_name;
                    semi.投稿时间 = row.article.create_time.HasValue ? row.article.create_time.Value.ToString() : "";
                    semi.票数 = row.vote_count.ToString();
                    semi.编辑打分 = row.article.adviser_score.ToString();
                    semi.当前排名 = i.ToString();
                    i++;
                    list.Add(semi);
                }
                return list.ToDataTable();
                #endregion
            }
            else if (state == 4)
            {
                #region 决赛
                data = data.Where(o => o.article.article_state == state).OrderByDescending(o=>o.article.expert_score);
                List<FinalsData> list = new List<FinalsData>();
                int i = 1;
                foreach (var row in data)
                {
                    FinalsData final = new FinalsData();
                    final.作品名称 = row.article.article_title;
                    final.作品编号 = row.article.article_no;
                    final.作者姓名 = row.article.user_name;
                    final.投稿时间 = row.article.create_time.HasValue ? row.article.create_time.Value.ToString() : "";
                    final.专家打分 = row.article.expert_score.ToString();
                    final.专家姓名 = row.article.expert_score.ToString();
                    final.当前排名 = i.ToString();
                    i++;
                    list.Add(final);
                }
                return list.ToDataTable<FinalsData>();
                #endregion
            }
            return null;
        }
        #endregion

        #region 自动更新排名(暂时不管)
        /// <summary>
        /// 自动更新排名
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ApiResult UpdateRanks(int state)
        {
            ApiResult apiResult = new ApiResult();
            //符合排名的数据
            var article_states = from a in db.Set<articles>()
                                 join b in db.Set<article_states>() on a.article_id equals b.article_id
                                 where b.article_state == state
                                 select b;
            //
            return apiResult;
        }

        #endregion
    }
}
