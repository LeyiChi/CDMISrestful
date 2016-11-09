using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class CmMstInfoItem
    {
        /// <summary>
        /// 模块大分类，如M1，M2
        /// </summary>
        public string CategoryCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public int SortNo { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int GroupHeaderFlag { get; set; }
        public string ControlType { get; set; }
        public string OptionCategory { get; set; }
        public string RevUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }

        [Range(0, 3)]
        public int DeviceType { get; set; }

    }
}