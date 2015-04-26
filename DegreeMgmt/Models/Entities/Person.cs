using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DegreeMgmt.Models.Entities
{
    public class Person
    {
        private List<Degree> _degrees = new List<Degree>();

        #region Properties

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "姓名是必填项")]        
        [StringLength(20, ErrorMessage = "姓名过长")]
        [Display(Name = "姓名")]
        public String Name { get; set; }
                
        [Required(ErrorMessage = "性别是必填项")]
        [Display(Name = "性别")]
        public bool Gender { get; set; } // true means male; flase means female
                
        [Display(Name = "出生日期")]
        public DateTime Birthday { get; set; }  // 生日

        [Required(ErrorMessage = "身份证号码是必填项")]
        [MinLength(18, ErrorMessage = "身份证号码格式不正确")]
        [MaxLength(18, ErrorMessage = "身份证号码格式不正确")]
        [Display(Name = "身份证号")]
        public String IDNumber { get; set; }  // 身份证号

        [StringLength(50, ErrorMessage = "籍贯过长")]
        [Display(Name = "籍贯")]
        public String Birthplace { get; set; }  // 籍贯

        [StringLength(20)]
        [RegularExpression(@"(\+86)?-?1[3-8]\d{9}", ErrorMessage = "移动电话格式不正确")]
        [Display(Name = "移动电话")]
        public String MobilePhone { get; set; }
                
        [RegularExpression(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$",
            ErrorMessage = "电子邮件格式不正确")]
        [Display(Name = "电子邮箱")]
        public String Email { get; set; }

        public byte[] Portrait { get; set; }  // 头像图片，存储为二进制数据
              
        [StringLength(50)]
        public String ImageMimeType { get; set; } 

        // one degree may have more than one degrees
        public List<Degree> Degrees { get { return _degrees; } }

        #endregion Properties

        //public void AddDegree(Degree degree)
        //{
        //    if (null == degree)
        //        return;

        //    if (null == this.Degrees.FirstOrDefault(x => x.ID == degree.ID))
        //        this.Degrees.Add(degree);
        //}

        //public void RemoveDegree(Degree degree)
        //{
        //    if (null == degree)
        //        return;

        //    var target = this.Degrees.FirstOrDefault(x => x.ID == degree.ID);
        //    if (null != target)
        //        this.Degrees.Remove(target);
        //}
    }
}
