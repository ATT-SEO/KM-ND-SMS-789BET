﻿namespace API.KM58.Model.DTO
{
    public class AccountRegistersDTO
    {
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string? Content { get; set; }
        public string? Device { get; set; }
        public string? ProjectCode { get; set; }
        public int Status { get; set; }
        public bool AutoPoint { get; set; } = false;
        public bool isSMS { get; set; }
        public string Account { get; set; }
        public int Point { get; set; }
        public string? UserPoint { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
