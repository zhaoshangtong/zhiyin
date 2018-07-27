using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    /// <summary>
    /// 任务调度表
    /// </summary>
    public partial class job_detail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(200)]
        public string job_name { get; set; }
        [StringLength(200)]
        public string group_name { get; set; }
        [StringLength(500)]
        public string job_description { get; set; }
        [StringLength(500)]
        public string job_classname { get; set; }
        [StringLength(50)]
        public string cron { get; set; }
        [Required]
        public bool is_start_now { get; set; }
        [StringLength(500)]
        public string url { get; set; }
        [StringLength(10)]
        public string request_type { get; set; }
        public string job_data { get; set; }
        public DateTime createtime { get; set; }
        [StringLength(50)]
        public string random { get; set; }
        [StringLength(50)]
        public string status { get; set; }
        [StringLength(50)]
        public string update_indentify { get; set; }
        [StringLength(10)]
        public string handler { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        
        
    }
}
