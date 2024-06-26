﻿using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class SMSRawData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Device { get; set; }
        [Required]
        public string ProjectCode { get; set; }
        [Required]
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
