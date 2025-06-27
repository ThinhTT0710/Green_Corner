namespace GreenCorner.MVC.Models { 
public class StaffDTO
    {
		public string? ID { get; set; }
		public string FullName { get; set; }
		public string? Email { get; set; }
		public string Address { get; set; }
		public string Avatar { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsBan { get; set; } = false;
		public string? Role { get; set; }
        public string? Password { get; set; }
    }
}
