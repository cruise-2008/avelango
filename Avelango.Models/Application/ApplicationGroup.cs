using System.Collections.Generic;

namespace Avelango.Models.Application
{
    public class ApplicationGroup {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Ico { get; set; }

        public IList<SubGroup> SubGroups { get; set; }

        public ApplicationGroup() {}

        public ApplicationGroup(string name, string text, string ico) {
            Name = name;
            Text = text;
            Ico = ico;
            SubGroups = new List<SubGroup>();
        }
    }


    public class SubGroup {
        public string Name { get; set; }
        public string Text { get; set; }
        public SubGroup(string name, string text) {
            Name = name;
            Text = text;
        }
    }
}
