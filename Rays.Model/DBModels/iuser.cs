using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.DBModels
{
    public partial class iuser
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int space_id { get; set; }
        public int weixin_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public Nullable<System.DateTime> lastlogin { get; set; }
        public string headimgurl { get; set; }
        public int ismember { get; set; }
        public Nullable<System.DateTime> member_end_time { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public Nullable<System.DateTime> updatetime { get; set; }
        public string tempcode { get; set; }
        public string random { get; set; }
        public int append_id { get; set; }
        public int copy_data_id { get; set; }
    }
}
