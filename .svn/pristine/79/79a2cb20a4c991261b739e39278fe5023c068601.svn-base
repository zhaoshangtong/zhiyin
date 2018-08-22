using Rays.Model.DBModels;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.DAL.Article
{
    public class ArticleDAL
    {
        private DbContext db= ContextFactory.GetCurrentContext("");
        /// <summary>
        /// 获取作品列表
        /// </summary>
        /// <param name="zone_id">赛区id</param>
        /// <param name="keyword">作品编号/标题/作者</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiPageResult GetArticleList(int uid,int zone_id, int competiontion_season_id, string keyword = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            apiResult.pageIndex = pageIndex;
            apiResult.pageSize = pageSize;
            //作品列表(必须是审核通过了的)
            var article_list = from a in db.Set<articles>()
                               join b in db.Set<article_competition_season>() on a.article_id equals b.article_id 
                               into ab1 from ab in ab1.DefaultIfEmpty()
                               where ab.competiontion_season_id == competiontion_season_id
                               join c in db.Set<author_info>() on a.uid equals c.uid
                               join d in db.Set<article_states>() on a.article_id equals d.article_id
                               where d.article_state>=2
                               select new { a.article_id,ab.zone_id,a.article_no,a.article_pic,a.article_title,a.uid,c.user_name};
            if (!string.IsNullOrEmpty(keyword))
            {
                article_list = article_list.Where(o => o.article_no.Contains(keyword) || o.article_title.Contains(keyword) || o.user_name.Contains(keyword));
            }
            if (zone_id > 0)
            {
                article_list = article_list.Where(o => o.zone_id == zone_id);
            }
            //票数计算
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { b.Key, vote_count = (int?)b.Count() };
            int count = article_list.Count();
            var _data = article_list.OrderByDescending(o => o.article_id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var data = from a in _data join b in vote_count on a.article_id equals b.Key into ab1 from ab in ab1.DefaultIfEmpty() select new { article=a, vote_count=ab.vote_count??0 };
            //我的票数
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.AddDays(1).Date;
            //我的投票数
            var my_vote_count = from a in db.Set<vote_record>() where a.create_time >= start && a.create_time < end group a by a.uid into b where b.Key == uid select new { vote_count = (int?)b.Count() };
            //我对哪些作品投票了，投了多少票
            var my_vote_article_count = from a in db.Set<vote_record>()
                                        where a.create_time >= start && a.create_time < end && a.uid == uid
                                        group a by a.article_id into b
                                        select new { article_id = b.Key, vote_count = (int?)b.Count() };
            var list = from a in data
                       join b in my_vote_article_count on a.article.article_id equals b.article_id
                       into ab1 from ab in ab1.DefaultIfEmpty()
                       select new {data=a, my_vote_article_count=ab.vote_count };
            apiResult.success = true;
            apiResult.message = "获取作品列表";
            apiResult.data = new { list, my_vote_count };
            apiResult.totalCount = count;
            return apiResult;
        }
        /// <summary>
        /// 获取作品内容并获取上一篇，下一篇的id
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult GetArticleInfoByAdmin(int state,int article_id)
        {
            ApiResult apiResult = new ApiResult();
            var articles = from a in db.Set<articles>()
                           join b in db.Set<article_states>() on a.article_id equals b.article_id
                           join c in db.Set<author_info>() on a.uid equals c.uid
                           into ac1 from ac in ac1.DefaultIfEmpty()
                           orderby a.article_id
                           select new { article = a, b.article_state,b.adviser_score,b.expert_name,b.expert_score ,ac.user_name,ac.school };
            if (state == 2)
            {
                articles = articles.Where(o => o.article_state <= 2);
            }
            else
            {
                articles = articles.Where(o => o.article_state == state);
            }
            var current_article = articles.Where(o => o.article.article_id == article_id);
            var pre_article = articles.Where(o => o.article.article_id < article_id).OrderByDescending(o => o.article.article_id).Take(1).FirstOrDefault();
            var next_article = articles.Where(o => o.article.article_id > article_id).Take(1).FirstOrDefault();
            int pre_article_id = pre_article?.article.article_id ?? 0;
            int next_article_id = next_article?.article.article_id ?? 0;
            apiResult.success = true;
            apiResult.message = "获取作品内容并获取上一篇，下一篇的id";
            apiResult.data = new { current_article, pre_article_id , next_article_id };
            return apiResult;
        }



        /// <summary>
        /// 当前赛区当前用户是否已经有作品了
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="zone_id"></param>
        /// <returns>true:有了，false：没有</returns>
        public bool ExistUserArticle(int uid,int competition_season_id)
        {
            var article = from a in db.Set<articles>()
                          join b in db.Set<article_competition_season>() on a.article_id equals b.article_id
                          where a.uid == uid && b.competiontion_season_id == competition_season_id
                          select a;
            if (article.Count() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取作品详情
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult GetArticleInfo(int uid,int article_id)
        {
            ApiResult apiResult = new ApiResult();
            var article = from a in db.Set<articles>() join b in db.Set<author_info>() on a.uid equals b.uid join c in db.Set<weixin_user>() on a.uid equals c.uid into ac1 from ac in ac1.DefaultIfEmpty()  where a.article_id == article_id select new { article=a, b.school,b.user_name,ac.headimgurl };
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b where b.Key == article_id  select new { vote_count = (int?)b.Count() };
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.AddDays(1).Date;
            var my_vote_count = from a in db.Set<vote_record>() where a.create_time>= start && a.create_time< end group a by a.uid into b where b.Key == uid select new { vote_count = (int?)b.Count() };
            //我对当前作品投票了，投了多少票
            var my_vote_article_count = (from a in db.Set<vote_record>()
                                        where a.create_time >= start && a.create_time < end && a.uid == uid&&a.article_id==article_id
                                        group a by a.article_id into b
                                        select new { article_id = b.Key, vote_count = (int?)b.Count() }).FirstOrDefault();
            apiResult.success = true;
            apiResult.message = "获取作品内容";
            apiResult.data = new {article,vote_count,my_vote_count , my_vote_article_count };
            return apiResult;
        }

        
        /// <summary>
        /// 更新作品编号
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult UploadAritcleNo(articles article)
        {
            ApiResult apiResult = new ApiResult();
            int no = (from a in db.Set<articles>() where a.article_id <= article.article_id select a).Count();
            string article_no = string.Format("{0:D6}",no);
            article.article_no = article_no;
            db.Entry<articles>(article).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                apiResult.success = true;
                apiResult.message = "保存成功";
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "保存失败";
            }
            return apiResult;
        }

        /// <summary>
        /// 获取初赛作品，并更新状态（按所有数据取前多少名）
        /// </summary>
        /// <param name="competition_season_id">赛季id</param>
        /// <returns></returns>
        public ApiResult GetPreliminariesAndUpdateByAll(int competition_season_id,int count)
        {
            ApiResult apiResult = new ApiResult();
            //必须是审核过的
            var articles = from a in db.Set<articles>()
                           join b in db.Set<article_competition_season>() on a.article_id equals b.article_id
                           join c in db.Set<article_states>() on a.article_id equals c.article_id
                           where b.competiontion_season_id == competition_season_id&&c.article_state==2
                           select new { articlt=a ,b.zone_id};
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id=b.Key, vote_count = (int?)b.Count() };
            //所有作品的票数
            var _data = from a in articles
                       join b in vote_count on a.articlt.article_id equals b.article_id
                       into ab1 from ab in ab1.DefaultIfEmpty()
                       select new { article=a.articlt,a.zone_id, vote_count=ab.vote_count??0};

            //获取可以进半决赛的作品
            var data = _data.OrderByDescending(o => o.vote_count).Skip(0).Take(count);
            
            var update_articles_states = (from a in data
                                  join b in db.Set<article_states>() on a.article.article_id equals b.article_id
                                  select b).ToList();

            foreach(var article_states in update_articles_states)
            {
                article_states.article_state = 3;//半决赛
                article_states.update_time = DateTime.Now;
                //准备修改
                db.Entry<article_states>(article_states).State = EntityState.Modified;
            }
            bool result = db.SaveChanges()>0?true:false;
            apiResult.success = result;
            apiResult.message = result?"成功":"失败";
            return apiResult;
        }

        /// <summary>
        /// 获取初赛作品，并更新状态（按所有数据取前多少名）
        /// </summary>
        /// <param name="competition_season_id">赛季id</param>
        /// <returns></returns>
        public ApiResult GetPreliminariesAndUpdateByZone(int competition_season_id, int count)
        {
            ApiResult apiResult = new ApiResult();
            //必须是审核过的
            var articles = from a in db.Set<articles>()
                           join b in db.Set<article_competition_season>() on a.article_id equals b.article_id
                           join c in db.Set<article_states>() on a.article_id equals c.article_id
                           where b.competiontion_season_id == competition_season_id && c.article_state == 2
                           select new { articlt = a, b.zone_id };
            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id = b.Key, vote_count = (int?)b.Count() };
            //所有作品的票数
            var _data = from a in articles
                        join b in vote_count on a.articlt.article_id equals b.article_id
                        into ab1
                        from ab in ab1.DefaultIfEmpty()
                        select new { article = a.articlt, a.zone_id, vote_count = ab.vote_count ?? 0 };

            //获取可以进半决赛的作品
            var data = _data.OrderByDescending(o => o.vote_count);
            var zone_id_list = data.Select(o => o.zone_id).Distinct().ToList();
            var update_articles_states = new List<article_states>();
            foreach (var zone_id in zone_id_list)
            {
                //取前30
                var zone_articles = data.Where(o => o.zone_id == zone_id).OrderByDescending(o => o.vote_count).Take(count).Select(o=>o.article);
                var zone_data= (from a in zone_articles
                                join b in db.Set<article_states>() on a.article_id equals b.article_id
                                select b).ToList();
                update_articles_states.AddRange(zone_data);

            }
            foreach (var article_states in update_articles_states)
            {
                article_states.article_state = 3;//半决赛
                article_states.update_time = DateTime.Now;
                //准备修改
                db.Entry<article_states>(article_states).State = EntityState.Modified;
            }
            bool result = db.SaveChanges() > 0 ? true : false;
            apiResult.success = result;
            apiResult.message = result ? "成功" : "失败";
            return apiResult;
        }
        /// <summary>
        /// 获取半决赛作品，并更新状态
        /// </summary>
        /// <param name="competition_season_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ApiResult GetSemifinalsAndUpdate(int competition_season_id, int count)
        {
            ApiResult apiResult = new ApiResult();
            //必须是过了初赛的
            var articles = from a in db.Set<articles>()
                           join b in db.Set<article_competition_season>() on a.article_id equals b.article_id
                           join c in db.Set<article_states>() on a.article_id equals c.article_id
                           where b.competiontion_season_id == competition_season_id && c.article_state == 3
                           select new { a, c.adviser_score};

            var vote_count = from a in db.Set<vote_record>() group a by a.article_id into b select new { article_id = b.Key, vote_count = (int?)b.Count() };
            var _data = from a in articles
                        join b in vote_count on a.a.article_id equals b.article_id
                        into ab1 from ab in ab1.DefaultIfEmpty()
                        select new { article = a ,paixu= ab.vote_count??0*40 +a.adviser_score*60};
            //获取可以进半决赛的作品
            var data = _data.OrderByDescending(o => o.paixu).Skip(0).Take(count);

            var update_articles_states = (from a in data
                                          join b in db.Set<article_states>() on a.article.a.article_id equals b.article_id
                                          select b).ToList();

            foreach (var article_states in update_articles_states)
            {
                article_states.article_state = 4;//决赛
                article_states.update_time = DateTime.Now;
                //准备修改
                db.Entry<article_states>(article_states).State = EntityState.Modified;
            }
            bool result = db.SaveChanges() > 0 ? true : false;
            apiResult.success = result;
            apiResult.message = result ? "成功" : "失败";
            return apiResult;
        }
    }
}
