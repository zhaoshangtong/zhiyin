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
    /// 用户通知类
    /// </summary>
    public class user_notice
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int notice_id { get; set; }
        [MaxLength(250)]
        public string title { get; set; }
        public string content { get; set; }
        [MaxLength(250)]
        public string logo { get; set; }
        [DefaultValue(0)]
        public int is_top { get; set; }
        public DateTime? publish_time { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
