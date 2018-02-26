using System;

namespace Avelango.Models.Application
{
    public class ApplicationChats
    {
        public Guid PublicKey { get; set; }
        public ApplicationChatColocutor Collocutor { get; set; }
        public DateTime LastAction { get; set; }
        public bool IsBidirectional { get; set; }
    }
}
