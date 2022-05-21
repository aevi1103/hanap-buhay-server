using System.ComponentModel.DataAnnotations;

namespace hanap_buhay_server.Models
{
    public class Login
    {
        private string _userName;

        [Required]
        public string UserName
        {
            get => _userName.ToLower().Trim();
            set => _userName = value;
        }

        [Required]
        public string Password { get; set; }
    }
}
