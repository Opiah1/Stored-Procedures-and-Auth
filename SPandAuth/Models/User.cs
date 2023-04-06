namespace SPandAuth.Models
{
    public class User
    {
        public Guid ID { get; set; }
        public int SN { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? LastLoginTime { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string AccountNUmber { get; set; } = string.Empty;
        public int Balance { get; set; } 

    }
}
