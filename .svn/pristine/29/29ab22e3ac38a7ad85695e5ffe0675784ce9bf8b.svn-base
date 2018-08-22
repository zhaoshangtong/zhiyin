using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rays.Model.Sys;
using Rays.Model.DBModels;
using Rays.BLL;

namespace Zhiyin.Controllers.Vote
{
    /// <summary>
    /// 投票管理
    /// </summary>
    public class VoteController : ApiController
    {
        /// <summary>
        /// 投票(每人每天最多5票)
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ApiResult Vote(int uid,int article_id)
        {
            ApiResult apiResult = new ApiResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = uid.ToString(), Msg = "uid 不能为空值" },
                new Parameter { Value = uid.ToString(), Msg = "uid 必须是数字类型", Regex = @"^[1-9]\d*$" },
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
            BaseBLL<vote_record> vote_blll = new BaseBLL<vote_record>();
            //今天已投票数
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.AddDays(1).Date;
            int vote_count = vote_blll.Count(o => o.uid == uid && o.create_time >= start && o.create_time < end);
            if (vote_count < 5)
            {
                vote_blll.Add(new vote_record
                {
                    article_id=article_id,
                    uid=uid,
                    create_time=DateTime.Now
                });
                apiResult.success = true;
                apiResult.message = "您今天已投了" + (vote_count+1) + "票啦！";
                apiResult.data = vote_count + 1;
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "您今天已投了"+vote_count+"票啦！";
            }
            return apiResult;
        }

        
    }
}
