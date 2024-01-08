﻿namespace FE.ADMIN.Models
{
    public class SiteDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}