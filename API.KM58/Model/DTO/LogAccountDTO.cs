namespace API.KM58.Model.DTO
{
	public class LogAccountDTO
	{
		public int Id { get; set; }
		public string? Account { get; set; }
		public string? IP { get; set; }
		public string? FP { get; set; }
        public string? Project { get; set; }
        public string? Token { get; set; }
        public DateTime? CreatedTime { get; set; }
	}
}