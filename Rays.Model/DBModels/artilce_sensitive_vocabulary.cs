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
    /// 敏感词汇表
    /// </summary>
    public partial class artilce_sensitive_vocabulary
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int artilce_sensitive_vocabulary_id { get; set; }
        [Display(Name ="敏感词")]
        [StringLength(500)]
        public string vocabulary { get; set; }
        public DateTime? create_time { get; set; }

    }
}
