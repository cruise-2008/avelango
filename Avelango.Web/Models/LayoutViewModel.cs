using Avelango.Web.Models;

namespace Avelango.Models
{
    public class LayoutViewModel
    {
        public bool IsAuthanticated { get; set; }
        public string ImagePath { get; set; }
        public string ParlourPath { get; set; }

        public LayoutViewModel() {
            var user = new PrivateSession().Current.User;
            if (user == null) {
                IsAuthanticated = false;
                ImagePath = null;
                ParlourPath = null;
            }
            else {
                IsAuthanticated = true;
                ImagePath = !string.IsNullOrEmpty(user.UserLogoPath) ? user.UserLogoPath : "/Storage/Avatars/defaultuser.png";
                if (user.IsAdmin) { ParlourPath = "/Addmin/Parlour"; return;}
                if (user.IsModerator) { ParlourPath = "/Modderator/Parlour"; return;}
                ParlourPath = "/MyUser/Parlour";
            }
        }
    }
}