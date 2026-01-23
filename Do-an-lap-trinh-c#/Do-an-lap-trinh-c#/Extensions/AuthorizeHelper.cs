namespace Do_an_lap_trinh_c_.Extensions
{
    public static class AuthorizeHelper
    {
        public static bool IsLoggedIn(this HttpContext context)
        {
            return context.Session.GetInt32("UserId") != null;
        }

        public static bool IsAdmin(this HttpContext context)
        {
            return context.Session.GetInt32("Role") == 0;
        }

        public static bool IsUser(this HttpContext context)
        {
            return context.Session.GetInt32("Role") == 1;
        }




    }


}
