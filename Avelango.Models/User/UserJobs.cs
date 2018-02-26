using System.Collections.Generic;

namespace Avelango.Models.User
{
    public class UserJobs
    {
        public string Pk { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ImagePair> Images { get; set; }
    }

    public class ImagePair {
        public string Small { get; set; }
        public string Large { get; set; }
    }
}
