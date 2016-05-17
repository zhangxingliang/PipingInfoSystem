namespace BasicService
{
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

[Table("PipingPictureInfo")]
public partial class PipingPictureInfo
    {
        [Key]
        [StringLength(32)]
        public string KeyID { get; set; }


        [StringLength(20)]
        public string PipingID { get; set; }


        [StringLength(20)]
        public string PictureID { get; set; }


        [StringLength(100)]
        public string PictureFilePath { get; set; }


        [StringLength(150)]
        public string PipingInternalState { get; set; }


        [StringLength(20)]
        public string Grade { get; set; }


        [StringLength(20)]
        public string Score { get; set; }


        [StringLength(20)]
        public string DefectCode { get; set; }


        [StringLength(20)]
        public string Distance { get; set; }


        public DateTime? AddTime { get; set; }


        [StringLength(20)]
        public string AddPerson { get; set; }


        public byte? IsDelete { get; set; }


        public byte? IsEnable { get; set; }


        [StringLength(200)]
        public string Remark { get; set; }


        public byte? PictureType { get; set; }


        [StringLength(200)]
        public string PictureRemark { get; set; }
    }
}
