using Rays.DAL.Article;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.BLL.Article
{
    public class ArticleBLL
    {
        private ArticleDAL dal = new ArticleDAL();
        /// <summary>
        /// 获取作品列表
        /// </summary>
        /// <param name="zone_id"></param>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiPageResult GetArticleList(int uid,int zone_id,int competiontion_season_id, string keyword = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            return dal.GetArticleList(uid,zone_id, competiontion_season_id, keyword, pageIndex, pageSize);
        }
        /// <summary>
        /// 当前赛区当前用户是否已经有作品了
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="competition_season_id"></param>
        /// <returns>true:有了，false：没有</returns>
        public bool ExistUserArticle(int uid, int competition_season_id)
        {
            return dal.ExistUserArticle(uid, competition_season_id);
        }

        /// <summary>
        /// 获取作品详情
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult GetArticleInfo(int uid,int article_id)
        {
            return dal.GetArticleInfo(uid,article_id);
        }
        
        /// <summary>
        /// 更新作品编号
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult UploadAritcleNo(Model.DBModels.articles article)
        {
            return dal.UploadAritcleNo(article);
        }

        /// <summary>
        /// 获取初赛作品，并更新状态（按所有）
        /// </summary>
        /// <param name="competition_season_id">赛季id</param>
        /// <returns></returns>
        public ApiResult GetPreliminariesAndUpdateByAll(int competition_season_id, int count)
        {
            return dal.GetPreliminariesAndUpdateByAll(competition_season_id, count);
        }
        /// <summary>
        /// 获取初赛作品，并更新状态(按赛区)
        /// </summary>
        /// <param name="competition_season_id">赛季id</param>
        /// <returns></returns>
        public ApiResult GetPreliminariesAndUpdateByZone(int competition_season_id, int count)
        {
            return dal.GetPreliminariesAndUpdateByZone(competition_season_id, count);
        }
        /// <summary>
        /// 获取半决赛作品，并更新状态
        /// </summary>
        /// <param name="competition_season_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ApiResult GetSemifinalsAndUpdate(int competition_season_id, int count)
        {
            return dal.GetSemifinalsAndUpdate(competition_season_id, count);
        }

        /// <summary>
        /// 获取作品内容并获取上一篇，下一篇的id
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult GetArticleInfoByAdmin(int state,int article_id)
        {
            return dal.GetArticleInfoByAdmin(state,article_id);
        }
    }
}
