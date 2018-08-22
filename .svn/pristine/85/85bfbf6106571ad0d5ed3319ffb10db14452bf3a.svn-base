using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    /// <summary>
    /// 作者详情表
    /// </summary>
    public partial class author_info
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int author_info_id { get; set; }
        [DefaultValue(0)]
        [Required]
        public int uid { get; set; }
        public string user_name { get; set; }
        public string sex { get; set; }
        [Display(Name ="地区")]
        public string area { get; set; }
        public string school { get; set; }
        public string grade { get; set; }
        public int age { get; set; }
        public string phone { get; set; }
        [Display(Name = "指导老师")]
        public string teacher { get; set; }

        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
