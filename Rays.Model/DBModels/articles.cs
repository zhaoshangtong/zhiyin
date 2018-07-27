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
    /// 一个作者只能上传一个作品
    /// </summary>
    public partial class articles
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int article_id { get; set; }
        [Display(Name ="作品编号")]
        public string article_no { get; set; }
        [Display(Name = "作品标题")]
        [Required]
        public string article_title { get; set; }
        [Display(Name = "作品作者对应的uid")]
        [DefaultValue(0)]
        public int uid { get; set; }
        [Display(Name = "作品插图")]
        public string article_pic { get; set; }
        [Display(Name = "作品内容")]
        [MaxLength]
        public string article_content { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
