using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    public partial class weixin
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        ///<summary>
        ///  
        ///</summary>
        public int id { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string name { get; set; }
        [StringLength(255)]
        ///<summary>
        ///  
        ///</summary>
        public string url { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string token { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string appid { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string appsecret { get; set; }
        [StringLength(200)]
        ///<summary>
        ///  
        ///</summary>
        public string access_token { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> access_token_time { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string openid { get; set; }
        [StringLength(200)]
        ///<summary>
        ///  
        ///</summary>
        public string jsapi_ticket { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> jsapi_ticket_time { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string pay_appid { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string pay_secret { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string pay_mch_id { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string pay_partner_key { get; set; }
        ///<summary>
        ///是否跨号支付  
        ///</summary>
        public Nullable<int> pay_is_across { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<int> pay_across_weixin_id { get; set; }
        ///<summary>
        ///1：平台公号，0：普通公号，平台公号不参与运营，普通公号不参与管理  
        ///</summary>
        public Nullable<int> isplatform { get; set; }
        ///<summary>
        ///是否默认为运营公号提供给新渠道使用，1：是，0：否  
        ///</summary>
        public Nullable<int> isdefault { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<int> isdelete { get; set; }
        [StringLength(255)]
        ///<summary>
        ///默认二维码图片路径  
        ///</summary>
        public string default_ticket_path { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> createtime { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> updatetime { get; set; }
        ///<summary>
        ///模版最后更新时间  
        ///</summary>
        public Nullable<System.DateTime> template_lasttime { get; set; }
        [StringLength(50)]
        ///<summary>
        ///  
        ///</summary>
        public string random { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<int> copy_data_id { get; set; }
    }
}
