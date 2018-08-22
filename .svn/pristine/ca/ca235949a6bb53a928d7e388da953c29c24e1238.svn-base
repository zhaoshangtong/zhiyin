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
    /// 赛区-作品关联表
    /// </summary>
    public partial class article_competition_season
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int article_season_id { get; set; }
        [Required]
        [DefaultValue(0)]
        [Display(Name = "赛区")]
        public int zone_id { get; set; }
        [Required]
        [DefaultValue(0)]
        [Display(Name ="赛季id（公告id）")]
        public int competiontion_season_id { get; set; }
        [Required]
        [DefaultValue(0)]
        public int article_id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
