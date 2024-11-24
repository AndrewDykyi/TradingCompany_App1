namespace DAL.Concrete
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}