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
    /// 作品图片
    /// </summary>
    public partial class article_pic
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int article_pic_id { get; set; }
        [StringLength(250)]
        public string article_pic_url { get; set; }
        [DefaultValue(0)]
        public int topsize { get; set; }
    }
}
