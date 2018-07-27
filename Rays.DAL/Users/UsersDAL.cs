using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rays.Model.Sys;
using Rays.Model.DBModels;

namespace Rays.DAL.Users
{
    public class UsersDAL
    {
        private System.Data.Entity.DbContext db = ContextFactory.GetCurrentContext("");


        /// <summary>
        /// 我的作品
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="competiontion_season_id"></param>
        /// <returns></returns>
        public ApiResult GetMyArticle(int uid, int img_count, int competiontion_season_id)
        {
            ApiResult apiResult = new ApiResult();
            var articles = from a in db.Set<articles>()
                           join b in db.Set<article_competition_season>() on a.article_id equals b.article_id
                           into ab1
                           from ab in ab1.DefaultIfEmpty()
                           join c in db.Set<author_info>() on a.uid equals c.uid
                           into ac1 from ac in ac1.DefaultIfEmpty()
                           join d in db.Set<article_states>() on a.article_id equals d.article_id
                           where ab.competiontion_season_id == competiontion_season_id && a.uid == uid
                           select new { a.article_id,a.article_no, a.article_title, a.article_pic, a.article_content, ac.school, ac.user_name,d.article_state,ab.zone_id,d.return_tag,d.return_remark };
            List<dynamic> list = new List<dynamic>();
            
            //我的票数
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.AddDays(1).Date;
            var my_vote_count = from a in db.Set<vote_record>() where a.create_time >= start && a.create_time < end group a by a.uid into b where b.Key == uid select new { vote_count = (int?)b.Count() };
            //我对哪些作品投票了，投了多少票
            var _my_vote_article_count = from a in db.Set<vote_record>()
                                        where a.create_time >= start && a.create_time < end && a.uid == uid
                                        group a by a.article_id into b
                                        select new { article_id = b.Key, vote_count = (int?)b.Count() };
            foreach (var article in articles)
            {
                var votes = from a in db.Set<vote_record>()
                            join b in db.Set<weixin_user>() on a.uid equals b.uid
                            where a.article_id == article.article_id
                            select new { a.uid, b.headimgurl };
                //多少个人
                int user_count = votes.Select(o => o.uid).Distinct().Count();
                //多少张票
                int vote_count = votes.Count();
                //取前面几个人
                var img_list = votes.Select(o => o.headimgurl).Distinct().Take(img_count).ToList();
                var my_vote_article_count = _my_vote_article_count.Where(o => o.article_id == article.article_id).FirstOrDefault();
                list.Add(new { article, user_count, vote_count, img_list, my_vote_article_count });
            }
            apiResult.data = new { list, my_vote_count};
            apiResult.message = "我的作品";
            apiResult.success = true;
            return apiResult;
        }


        /// <summary>
        /// 给我投票的观众
        /// </summary>
        /// <param name="article_id">作品id</param>
        /// <returns></returns>
        public ApiResult GetVoteToMeUsers(int article_id)
        {
            ApiResult apiResult = new ApiResult();
            var user_vote_count = from a in db.Set<vote_record>()
                                  where a.article_id == article_id
                                  group a by a.uid into b
                                  select new { uid = b.Key, count = b.Count() };
            var data = from a in user_vote_count
                       join b in db.Set<weixin_user>() on a.uid equals b.uid
                       select new { b.nickname, b.headimgurl, a.count };
            apiResult.data = data;
            apiResult.success = true;
            apiResult.message = "给我投票的观众";
            return apiResult;
        }

        /// <summary>
        /// 我的投票记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiPageResult GetMyVoteRecord(int uid, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiResult = new ApiPageResult();
            apiResult.pageIndex = pageIndex;
            apiResult.pageSize = pageSize;
            var article = from a in db.Set<vote_record>()
                          join b in db.Set<articles>() on a.article_id equals b.article_id
                          join c in db.Set<author_info>() on b.uid equals c.uid
                          into ac1 from ac in ac1.DefaultIfEmpty()
                          where a.uid == uid
                          select new {b.article_id,b.article_pic,b.article_title,b.article_content, ac.user_name };
            int count = article.Distinct().Count();
            var _data = article.Distinct().OrderBy(o => o.article_id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var vote_count = from a in db.Set<vote_record>()  group a by a.article_id into b select new { b.Key, vote_count = (int?)b.Count() };
            var data = from a in _data join b in vote_count on a.article_id equals b.Key into ab1 from ab in ab1.DefaultIfEmpty() select new { article = a, vote_count= ab.vote_count??0 };
            apiResult.data = data;
            apiResult.success = true;
            apiResult.message = "我的投票记录";
            apiResult.totalCount = count;
            return apiResult;
        }
    }
}
