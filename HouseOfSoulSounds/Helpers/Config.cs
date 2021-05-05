using HouseOfSoulSounds.Models.Domain;

namespace HouseOfSoulSounds.Helpers
{
    public class Config
    {
        //НУЖНО ПРОДУМАТЬ БЕЗОПАСНОСТЬ!!!!!
        public const string EmailPass = "34semyon344";
        public const string Email = "houseofsoulsounds@gmail.com";

        public static readonly string
            Admin = "admin",
            RoleModerator = "chatmoderator",
            RoleAdmin = Admin,
            RoleModer = RoleModerator,
            DefaultAvatar = "\\images\\avatars\\0.png",
            AvatarsPath = "\\images\\avatars\\",
            TitleInstrumentPath = "\\images\\DB\\",
            ImagePagePath = "\\images\\Page\\";

        public static string ConnectionString { get; set; }
        public static string ConnectionDictionary { get; set; }
        

        public static string Name { get; set; }

        public static string WebRootPath { get; set; }

    }
}
