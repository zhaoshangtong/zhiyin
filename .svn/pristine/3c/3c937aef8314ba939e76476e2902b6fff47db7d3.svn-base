using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.Sys
{
    public class Parameter
    {
        /// <summary>
        /// 待验证的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 正则表达式 （可空）
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// 是否对 Value 值进行验证 默认为 true
        /// <para>当IsCheck达成某个条件时，才对Value进行验证</para>
        /// <para>例如：IsCheck = string.IsNullOrEmpty(Temp) 即Temp不为空时，才对Value进行验证</para>
        /// </summary>
        public bool IsCheck { get; set; } = true;
    }
}
