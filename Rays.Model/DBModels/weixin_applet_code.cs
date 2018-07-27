using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rays.Model.DBModels
{
    /// <summary>
    /// 小程序码
    /// </summary>
    public partial class weixin_applet_code
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        ///<summary>
        ///  
        ///</summary>
        public int id { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string code_name { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string code_desc { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<int> weixin_applet_id { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> create_time { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string random { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string img_path { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<int> is_delete { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string xcx_url { get; set; }
        ///<summary>
        ///1：小程序二维码2：小程序码 3：无限量小程序码   
        ///</summary>
        public Nullable<int> code_type { get; set; }
        ///<summary>
        ///场景值  
        ///</summary>
        public string scence { get; set; }
    }
}
