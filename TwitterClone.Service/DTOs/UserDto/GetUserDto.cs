namespace TwitterClone.Service.DTOs.UserDto
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }

    }
}
