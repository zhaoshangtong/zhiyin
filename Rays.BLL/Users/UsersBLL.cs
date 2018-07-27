using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rays.DAL.Users;
using Rays.Model.Sys;

namespace Rays.BLL.Users
{
    public class UsersBLL
    {
        private UsersDAL dal = new UsersDAL();

        /// <summary>
        /// 我的作品
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="competiontion_season_id"></param>
        /// <returns></returns>
        public ApiResult GetMyArticle(int uid, int img_count, int competiontion_season_id)
        {
            return dal.GetMyArticle(uid, img_count, competiontion_season_id);
        }

        /// <summary>
        /// 给我投票的观众
        /// </summary>
        /// <param name="article_id">作品id</param>
        /// <returns></returns>
        public ApiResult GetVoteToMeUsers(int article_id)
        {
            return dal.GetVoteToMeUsers(article_id);
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
            return dal.GetMyVoteRecord(uid, pageIndex, pageSize);
        }
    }
}
