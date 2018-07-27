using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    public partial class weixin_applet_template
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public Nullable<int> weixin_applet_id { get; set; }
        public string template_code { get; set; }
        public string template_id { get; set; }
        public string title { get; set; }
        public string template_content { get; set; }
        public string example { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public Nullable<System.DateTime> updatetime { get; set; }
        public Nullable<int> is_enable { get; set; }
    }
}
