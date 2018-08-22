using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Rays.Model.DBModels
{
    /// <summary>
    /// 作品状态
    /// </summary>
    public partial class article_states
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int article_state_id { get; set; }
        [Required]
        [System.ComponentModel.DefaultValue(0)]
        public int article_id { get; set; }
        [Display(Name ="作品状态：0初始，1审核不通过，2审核通过，3半决赛，4决赛")]
        [Required]
        [System.ComponentModel.DefaultValue(0)]
        public int article_state { get; set; }
        [Display(Name = "回退标签：[xxxx,xxxx,xxxx]")]
        public string return_tag { get; set; }
        [Display(Name = "回退理由")]
        public string return_remark { get; set; }
        [Display(Name = "编辑打分")]
        public double adviser_score { get; set; }
        //[Display(Name = "半决赛排名")]
        //[DefaultValue(0)]
        //public int semifinal_index { get; set; }
        [Display(Name = "专家打分")]
        public double expert_score { get; set; }
        [Display(Name = "专家姓名")]
        public string expert_name { get; set; }
        //[Display(Name = "决赛排名")]
        //[DefaultValue(0)]
        //public int final_index { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
