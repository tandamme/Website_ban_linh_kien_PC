using System.ComponentModel.DataAnnotations;

namespace Do_an_lap_trinh_c_.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string user_Name { get; set; }
        public string email { get; set; }
        public string passWord { get; set; }
        public string randomKey { get; set; }
    }
}
