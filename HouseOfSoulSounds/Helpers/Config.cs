namespace HouseOfSoulSounds.Helpers
{
    public class Config
    {
        //НУЖНО ПРОДУМАТЬ БЕЗОПАСНОСТЬ!!!!!
        public const string EmailPass = "WebAppLabs";

        public static readonly string
            Admin = "admin", 
            RoleModerator = "chatmoderator",
            RoleAdmin = Admin,
            DefaultAvatar="";//надо изменить
        
        public static string ConnectionString { get; set; }

        public static string Name { get; set; }
        public static string Email { get; set; }

        public static string WebRootPath { get; set; }
    }
}
