using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rays.DAL.Adviser;
using Rays.Model.Sys;
using System.Data;

namespace Rays.BLL.Adviser
{
    public class CompetitionBLL
    {
        private CompetitionDAL dal = new CompetitionDAL();

        /// <summary>
        /// 作品管理查询
        /// </summary>
        /// <param name="keyword">查询的关键字：作品编号/标题/作者</param>
        /// <param name="state">-1全部，0初始，1审核不通过，2审核通过,3半决赛，4决赛</param>
        /// <param name="competition_season_id">赛季</param>
        /// <param name="zone_id">0全部</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="orderby">排序字段：vote 按票数顺序排，votedesc 按票数倒叙排，date 按投稿时间顺序排，datedesc 按投稿时间倒叙排</param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">10</param>
        /// <returns></returns>
        public ApiPageResult GetArticleList(string keyword = null, int state = -1,int competition_season_id=0, int zone_id = 0, DateTime? start = null, DateTime? end = null, string orderby = null, int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            return dal.GetArticleList(keyword,state, competition_season_id, zone_id, start,end, orderby, pageIndex,pageSize);
        }

        /// <summary>
        /// 获取所有作品
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public DataTable GetAllArticle(int state, int competition_season_id = 0)
        {
            return dal.GetAllArticle(state, competition_season_id);
        }
    }
}
