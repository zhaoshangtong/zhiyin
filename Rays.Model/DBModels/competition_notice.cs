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
    /// 大赛公告(赛季公共)
    /// </summary>
    public partial class competition_notice
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int competition_season_id { get; set; }

        [Display(Name ="大赛标题")]
        [Required]
        [StringLength(maximumLength:256,ErrorMessage ="标题太长",MinimumLength =2)]
        public string notice_title { get; set; }
        [Display(Name = "大赛引言")]
        public string notice_introduction { get; set; }
        [Display(Name = "大赛要求")]
        public string notice_requirement { get; set; }
        #region 大赛时间
        [Display(Name = "作品征集时间")]
        public DateTime article_collection_start { get; set; }
        [Display(Name = "作品征集时间")]
        public DateTime article_collection_end { get; set; }
        [Display(Name = "作品征集图片")]
        public string article_collection_pic { get; set; }
        [Display(Name = "作品评选时间")]
        public DateTime article_selection_start { get; set; }
        [Display(Name = "作品评选时间")]
        public DateTime article_selection_end { get; set; }
        [Display(Name = "作品评选图片")]
        public string article_selection_pic { get; set; }
        [Display(Name = "结果公布时间")]
        public DateTime result_publish_start { get; set; }
        [Display(Name = "结果公布时间")]
        public DateTime result_publish_end { get; set; }
        [Display(Name = "结果公布图片")]
        public string result_publish_pic { get; set; }
        #endregion

        #region 参赛流程
        [Display(Name = "填写信息图片")]
        public string information_pic { get; set; }
        [Display(Name = "提交作品图片")]
        public string article_submission_pic { get; set; }
        [Display(Name = "参赛成功图片")]
        public string competition_pic { get; set; }
        #endregion
        [Display(Name = "参赛说明")]
        [MaxLength()]
        public string notice_desc { get; set; }
        [Display(Name = "是否开启赛季")]
        [Required]
        [System.ComponentModel.DefaultValue(0)]
        public int is_open { get; set; }
        [Display(Name = "是否删除")]
        [DefaultValue(0)]
        public int is_delete { get; set; }
        [Display(Name = "排序")]
        [DefaultValue(0)]
        public int topsize { get; set; }
        [Display(Name = "创建时间")]
        public DateTime? create_time { get; set; }
        [Display(Name = "更新时间")]
        public DateTime? update_time { get; set; }
    }
}
