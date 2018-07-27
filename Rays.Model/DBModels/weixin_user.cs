using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public partial class weixin_user
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int uid { get; set; }
        public int weixin_id { get; set; }
        public int subscribe { get; set; }
        public string openid { get; set; }
        public int groupid { get; set; }
        public string tagid_list { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string language { get; set; }
        public string headimgurl { get; set; }
        public int subscribe_time { get; set; }
        public string mobile { get; set; }
        public string access_token { get; set; }
        public Nullable<System.DateTime> access_token_time { get; set; }
        public string refresh_token { get; set; }
        public int authorinfo_id { get; set; }
        public Nullable<System.DateTime> sub_time { get; set; }
        public Nullable<System.DateTime> unsub_time { get; set; }
        public Nullable<System.DateTime> first_sub_time { get; set; }
        public int authorize { get; set; }
        public string code_book_font { get; set; }
        public int append_id { get; set; }
        public int copy_data_id { get; set; }
        public string random { get; set; }
        public string unionid { get; set; }
        public int old_uid { get; set; }
        public string source_code { get; set; }
        public int weixin_applet_id { get; set; }
    }
}
