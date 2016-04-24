namespace PipingInfoSystem.model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PipingDetectionInfo")]
    public partial class PipingDetectionInfo
    {
        [Key]
        [StringLength(32)]
        public string KeyID { get; set; }

        
        [StringLength(20)]
        public string PipingID { get; set; }

        
        [StringLength(20)]
        public string VideoFile { get; set; }

        
        [StringLength(20)]
        public string StartWellNo { get; set; }

        
        [StringLength(20)]
        public string EndWellNo { get; set; }

        
        [StringLength(20)]
        public string LayingYear { get; set; }

        
        [StringLength(20)]
        public string StartPointDepth { get; set; }

        
        [StringLength(20)]
        public string EndPointDepth { get; set; }

        
        [StringLength(20)]
        public string TubulationType { get; set; }

        
        [StringLength(20)]
        public string TubulationMaterial { get; set; }

        
        [StringLength(20)]
        public string TubulationDiameter { get; set; }

        
        [StringLength(20)]
        public string DetectionDirect { get; set; }

        
        [StringLength(20)]
        public string TubulationLength { get; set; }

        
        [StringLength(50)]
        public string DetectionAddress { get; set; }

        
        [StringLength(20)]
        public string DetectionTime { get; set; }

        
        public DateTime? AddTime { get; set; }

        
        [StringLength(20)]
        public string AddPerson { get; set; }

        
        public byte? IsEnable { get; set; }

        
        public byte? IsDelete { get; set; }

        
        [StringLength(20)]
        public string DetectionLength { get; set; }

        
        [StringLength(20)]
        public string DetectionFun { get; set; }
    }
}
