using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DegreeMgmt.Models.Entities
{
    public class Degree
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int HoldByWhom { get; set; }  // 由哪个人持有此学位
                
        [Required(ErrorMessage = "学校是必填项")]
        [StringLength(50, ErrorMessage = "学校名称过长")]
        [Display(Name = "学校")]
        public String School {get; set; }

        [Required(ErrorMessage = "院系是必填项")]
        [StringLength(50, ErrorMessage = "院系名称过长")]
        [Display(Name = "院系")]
        public String Academy { get; set; }  // 院系

        [StringLength(50, ErrorMessage = "专业名称过长")]
        [Display(Name = "专业")]
        public String Major { get; set; } // 专业

        /// <summary>
        /// 0 = 无，1 = 专科，2 = 本科，3 = 硕士研究生，4 = 博士研究生
        /// </summary>
        [Required(ErrorMessage = "学历是必填项")]
        [Range(0, 4)]        
        [Display(Name = "学历")]
        public int Edu { get; set; }  // 学历

        /// <summary>
        /// 0 = 无，2 = 学士，3 = 硕士，4 = 博士
        /// </summary>
        [Range(0, 4)]
        [Display(Name = "学位")]
        public int Class { get; set; }  // 学位

        [Required(ErrorMessage = "获得学位的日期是必填项")]        
        [Display(Name = "获得学位的日期")]
        public DateTime AcquiredDate { get; set; }  // 获得学位的日期
    }
}
