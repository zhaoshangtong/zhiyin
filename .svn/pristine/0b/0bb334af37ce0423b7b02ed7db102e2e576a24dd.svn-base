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
    /// 投票记录
    /// </summary>
    public partial class vote_record
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int vote_id { get; set; }
        [Required]
        [DefaultValue(0)]
        public int uid { get; set; }
        [Required]
        [DefaultValue(0)]
        public int article_id { get; set; }
        public DateTime? create_time { get; set; }
    }
}
