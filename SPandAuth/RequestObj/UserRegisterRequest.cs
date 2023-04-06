using System.ComponentModel.DataAnnotations;

namespace SPandAuth.RequestObj
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Balance { get; set; }
        public string password { get; set; }
    }
}
