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
    /// 赛程时间表----任务调度
    /// </summary>
    public partial class competition_date
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int competition_date_id { get; set; }
        [Display(Name = "赛季id（公告id）")]
        public int competition_season_id { get; set; }
        [Display(Name ="初赛结束时间")]
        public DateTime? preliminaries_date  { get; set; }
        [Display(Name = "半决赛结束时间")]
        public DateTime? semifinals_date { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
