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
    /// 微信开发平台
    /// </summary>
    public partial class weixin_open
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int weixin_open_id { get; set; }
        [StringLength(250)]
        public string weixin_open_name { get; set; }
        public string weixin_open_desc { get; set; }
        [StringLength(250)]
        public string weixin_open_pic { get; set; }
        [StringLength(50)]
        public string appid { get; set; }
        [StringLength(50)]
        public string secret { get; set; }
        [StringLength(250)]
        public string access_token { get; set; }
        [StringLength(250)]
        public string refresh_token { get; set; }
        public Nullable<System.DateTime> access_token_time { get; set; }

    }
}
