namespace PipingInfoSystem.model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        [Key]
        [StringLength(32)]
        public string KeID { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public int UserType { get; set; }

        [StringLength(100)]
        public string TrueName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string ID { get; set; }

        public int? IsDelete { get; set; }

        public DateTime? AuditDate { get; set; }

        [StringLength(100)]
        public string AuditUser { get; set; }

        public DateTime? ApplyDate { get; set; }

        [Required]
        [StringLength(32)]
        public string UserID { get; set; }
    }
}
