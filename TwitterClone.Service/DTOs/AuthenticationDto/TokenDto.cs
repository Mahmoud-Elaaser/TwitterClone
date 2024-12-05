namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
