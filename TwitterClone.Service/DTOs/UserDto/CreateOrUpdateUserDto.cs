﻿namespace TwitterClone.Service.DTOs.UserDto
{
    public class CreateOrUpdateUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverPhoto { get; set; }
        public string Email { get; set; }
    }


}
