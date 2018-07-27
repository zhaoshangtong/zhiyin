using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.Sys
{
    /// <summary>
    /// 半决赛
    /// </summary>
    public class SemifinalsData
    {
        public string 作品编号 { get; set; }
        public string 作品名称 { get; set; }
        public string 作者姓名 { get; set; }
        public string 投稿时间 { get; set; }
        public string 票数 { get; set; }
        public string 编辑打分 { get; set; }
        public string 当前排名 { get; set; }
    }
}
