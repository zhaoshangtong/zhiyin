using Rays.BLL;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Zhiyin.Controllers.UserNotice
{
    /// <summary>
    /// 通知的增删查改类
    /// </summary>
    public class UserNoticeController : ApiController
    {
        /// <summary>
        /// 获取通知
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="end">截止日期</param>
        /// <param name="pageIndex">页序号</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="is_show">0是不显示还没到发布时间的，1是显示所有</param>
        /// <returns></returns>
        [HttpGet]
        public ApiPageResult GetAllNotices(DateTime? start = null, DateTime? end = null, int is_show=1,int pageIndex = GloabManager.PAGEINDEX, int pageSize = GloabManager.PAGESIZE)
        {
            ApiPageResult apiPageResult = new ApiPageResult();
            apiPageResult.pageIndex = pageIndex;
            apiPageResult.pageSize = pageSize;
            BaseBLL<user_notice> bll = new BaseBLL<user_notice>();
            var list = bll.FindList<int>(notice => true);
            if (is_show == 0)
            {
                list = list.Where(o => o.publish_time <= DateTime.Now);
            }
            if (start.HasValue)
            {
                list = list.Where(notice => notice.update_time > start.Value &&
                                  notice.update_time < (end ?? DateTime.Now));
            }
            int count = list.Count();
            var top_notice = list.Where(o => o.is_top == 1);
            list = list.OrderByDescending(notice => notice.update_time).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var result = list.Select(notice => new
            {
                notice.notice_id,
                notice.title,
                notice.content,
                notice.is_top,
                notice.publish_time,
            });
            apiPageResult.success = true;
            apiPageResult.message = "获取所有通知";
            apiPageResult.data = new { result,top_notice };
            apiPageResult.totalCount = count;
            return apiPageResult;
        }

        /// <summary>
        /// 按照 id 获取通知
        /// </summary>
        /// <param name="notice_id">通知 id</param>
        /// <returns></returns>
        public ApiResult GetNoticeById(int notice_id)
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<user_notice> bll = new BaseBLL<user_notice>();
            var notice = bll.Find(n => n.notice_id == notice_id);
            if (notice == null)
            {
                apiResult.success = false;
                apiResult.message = "该通知不存在";
                return apiResult;
            }
            else
            {
                apiResult.success = true;
                apiResult.data = notice;
                return apiResult;
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="data">
        /// {
        ///   "notice_id": 若大于零则为修改，小于零则为新增,
        ///   "title": 通知标题，
        ///   "content": 通知内容，
        ///   "logo": 通知的 logo,
        ///   "is_top": 0 不置顶，1 置顶，
        ///   "publish_time": 发布时间，在该时间后才能查询到该通知，若不设置，则为当前时间
        /// }
        /// </param>
        /// <returns></returns>
        public ApiResult AddNotice(dynamic data)
        {
            ApiResult apiResult = new ApiResult();
            if (Util.isNotNull(data))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var notice = Newtonsoft.Json.JsonConvert.DeserializeObject<user_notice>(json);
                BaseBLL<user_notice> bll = new BaseBLL<user_notice>();
                if (notice?.notice_id > 0)
                {
                    //如果是置顶，应先将之前置顶的数据取消
                    if (notice.is_top == 1)
                    {
                        var is_top_notices = bll.FindList<int>(o => o.is_top == 1);
                        List<user_notice> is_top_notice_list = new List<user_notice>();
                        foreach(var _notice in is_top_notices)
                        {
                            _notice.is_top = 0;
                            _notice.update_time = DateTime.Now;
                            is_top_notice_list.Add(_notice);
                        }
                        bll.UpdateMore(is_top_notice_list);
                    }
                    // 修改
                    var findNotice = bll.Find(n => n.notice_id == notice.notice_id);
                    if (findNotice == null)
                    {
                        apiResult.success = false;
                        apiResult.message = "不存在该通知";
                        return apiResult;
                    }
                    notice.create_time = findNotice.create_time;
                    notice.update_time = DateTime.Now;
                    bll.Update(notice);
                }
                else if (Util.isNotNull(notice))
                {
                    //如果是置顶，应先将之前置顶的数据取消
                    if (notice.is_top == 1)
                    {
                        var is_top_notices = bll.FindList<int>(o => o.is_top == 1);
                        List<user_notice> is_top_notice_list = new List<user_notice>();
                        foreach (var _notice in is_top_notices)
                        {
                            _notice.is_top = 0;
                            _notice.update_time = DateTime.Now;
                            is_top_notice_list.Add(_notice);
                        }
                        bll.UpdateMore(is_top_notice_list);
                    }
                    // 新增
                    if (!notice.publish_time.HasValue)
                    {
                        notice.publish_time = DateTime.Now;
                    }
                    notice.create_time = DateTime.Now;
                    notice.update_time = DateTime.Now;
                    bll.Add(notice);
                }
                else
                {
                    apiResult.success = false;
                    apiResult.message = "参数错误";
                    return apiResult;
                }
            }
            else
            {
                apiResult.success = false;
                apiResult.message = "参数错误";
            }

            apiResult.success = true;
            apiResult.message = "成功";
            return apiResult;
        }

        /// <summary>
        /// 按照 id 删除通知
        /// </summary>
        /// <param name="notice_id">通知的 id</param>
        /// <returns></returns>
        public ApiResult DelNotice(int notice_id)
        {
            ApiResult apiResult = new ApiResult();

            BaseBLL<user_notice> bll = new BaseBLL<user_notice>();
            var notice = bll.Find(n => n.notice_id == notice_id);
            if (notice == null)
            {
                apiResult.success = false;
                apiResult.message = "不存在该通知";
                return apiResult;
            }
            else
            {
                var success = bll.Delete(notice);
                apiResult.success = success;
                apiResult.message = success ? "成功" : "删除失败";
            }
            return apiResult;
        }
    }
}