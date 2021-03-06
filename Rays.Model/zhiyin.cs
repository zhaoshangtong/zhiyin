﻿namespace Rays.Model
{
    using DBModels;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class zhiyin : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“zhiyin”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“Rays.Model.zhiyin”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“zhiyin”
        //连接字符串。
        public zhiyin()
            : base("name=zhiyin")
        {
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。
        public DbSet<weixin> weixin { get; set; }
        /// <summary>
        /// 小程序
        /// </summary>
        public  DbSet<weixin_applet> weixin_applet { get; set; }
        public  DbSet<weixin_applet_code> weixin_applet_code { get; set; }
        public  DbSet<weixin_applet_log> weixin_applet_log { get; set; }
        public  DbSet<weixin_user> weixin_user { get; set; }
        public  DbSet<iuser> iuser { get; set; }
        /// <summary>
        /// 作者详情表
        /// </summary>
        public  DbSet<author_info> author_info { get; set; }
        /// <summary>
        /// 小程序模板消息
        /// </summary>
        public  DbSet<weixin_applet_template> weixin_applet_template { get; set; }
        /// <summary>
        /// 大赛公告
        /// </summary>
        public  DbSet<competition_notice> competition_notice { get; set; }
        /// <summary>
        /// 作品
        /// </summary>
        public  DbSet<articles> articles { get; set; }
        /// <summary>
        /// 赛区
        /// </summary>
        public  DbSet<competition_zone> competition_zone { get; set; }
        /// <summary>
        /// 赛区-作品
        /// </summary>
        public  DbSet<article_competition_season> article_competition_season { get; set; }
        /// <summary>
        /// 投票记录
        /// </summary>
        public  DbSet<vote_record> vote_record { get; set; }
        /// <summary>
        /// 用户通知
        /// </summary>
        public DbSet<user_notice> user_notices { get; set; }

        public  DbSet<adviser> adviser { get; set; }
        public  DbSet<article_states> article_states { get; set; }
        public  DbSet<competition_date> competition_date { get; set; }
        public DbSet<job_detail> job_detail { get; set; }
        public DbSet<weixin_open> weixin_open { get; set; }
        public DbSet<article_pic> article_pic { get; set; }
        public DbSet<artilce_sensitive_vocabulary> artilce_sensitive_vocabulary { get; set; }
    }

}