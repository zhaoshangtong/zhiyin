using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    public partial class weixin_applet
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string applet_name { get; set; }
        public string applet_desc { get; set; }
        public string applet_pic { get; set; }
        public string appid { get; set; }
        public string secret { get; set; }
        public string mchid { get; set; }
        public string partner_key { get; set; }
        public string appcode { get; set; }
        public string access_token { get; set; }
        public Nullable<System.DateTime> access_token_time { get; set; }
        public Nullable<int> is_open_poster { get; set; }
        public Nullable<int> applet_category { get; set; }
        public string msg_token { get; set; }
        public string encoding_AESkey { get; set; }
    }
}
