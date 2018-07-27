using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rays.Model.DBModels
{
    public partial  class weixin_applet_log
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
        public string title { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string info { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string ip { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string request_url { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public string appcode { get; set; }
        ///<summary>
        ///  
        ///</summary>
        public Nullable<System.DateTime> createtime { get; set; }
    }
}
