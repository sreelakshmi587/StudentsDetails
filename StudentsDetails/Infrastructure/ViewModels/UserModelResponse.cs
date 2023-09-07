namespace StudentsDetails.Infrastructure.ViewModels
{
    public class UserModelResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string[] RolesArray
        {
            get { return Roles?.Split(','); }
            set { Roles = string.Join(",", value); }
        }
    }
}
