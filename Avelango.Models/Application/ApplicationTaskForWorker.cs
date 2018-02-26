using System;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationTaskForWorker
    {
        public Guid TaskPublicKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TopicalTo { get; set; }
        public int CustomerPrice { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Closed { get; set; }
        public string PlaceName { get; set; }

        // Not explicited
        public bool ApprovedToMe { get; set; }
        public bool ItsMyTask { get; set; }
        public ApplicationTaskBid Bid { get; set; }
        public ApplicationCustomer Customer { get; set; }
        //public List<ApplicationTaskAttachment> Attachments { get; set; }


        public static explicit operator ApplicationTaskForWorker(Tasks task) {
            return new ApplicationTaskForWorker {
                TaskPublicKey = task.PublicKey,
                Title = task.Name,
                Description = task.Description,
                TopicalTo = task.TopicalTo,
                CustomerPrice = task.Price,
                Status = task.ProccessStatus,
                Created = task.Created,
                Closed = task.Closed,
                PlaceName = task.PlaceName
            };
        }
    }
}
