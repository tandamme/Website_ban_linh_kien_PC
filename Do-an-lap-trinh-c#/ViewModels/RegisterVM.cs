using System.ComponentModel.DataAnnotations;

namespace Do_an_lap_trinh_c_.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string passWord { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("passWord", ErrorMessage = "Password không khớp")]
        public string rePassWord { get; set; }
    }

}
