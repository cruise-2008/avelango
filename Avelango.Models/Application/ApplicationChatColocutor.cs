namespace Avelango.Models.Application
{
    public class ApplicationChatColocutor
    {
        public string Pk { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserLogoPath { get; set; }

        public static explicit operator ApplicationChatColocutor(ApplicationUser user) {
            return new ApplicationChatColocutor {
                Pk = user.Pk.ToString(),
                Name = user.Name,
                SurName = user.SurName,
                UserLogoPath = user.UserLogoPath
            };
        }
    }
}
