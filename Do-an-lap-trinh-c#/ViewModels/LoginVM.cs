using System.ComponentModel.DataAnnotations;

namespace Do_an_lap_trinh_c_.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string userName { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string passWord { get; set; }
    }
}
