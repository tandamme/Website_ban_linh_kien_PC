using System.ComponentModel.DataAnnotations;

namespace Do_an_lap_trinh_c_.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string XacNhanMatKhau { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
    }
}
