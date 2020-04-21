namespace UsefulServices.Dtos.Users
{
    public class ChangePasswordDto : AuthUserDto
    {
        public string NewPassword { get; set; }
    }
}
