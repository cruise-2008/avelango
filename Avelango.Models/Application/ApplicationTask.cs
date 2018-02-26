using System;
using System.Collections.Generic;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationTask
    {
        public Guid PublicKey { get; set; }
        public ApplicationCustomer Customer { get; set; }
        public ApplicationWorker Worker { get; set; }
        public List<ApplicationWorker> Biders { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TopicalTo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public int Price { get; set; }
        public string ProccessStatus { get; set; }
        public string PlaceName { get; set; }
        public double? PlaceLat { get; set; }
        public double? PlaceLng { get; set; }
        public string Status { get; set; }
        public bool AlreadyBided { get; set; }
        public bool HasBiders { get; set; }
        public bool RemovedForCustomer { get; set; }


        public bool ClosedByCustomer { get; set; }


        public List<ApplicationTaskAttachment> Attachments { get; set; }
        public ApplicationTaskBid BidInfo { get; set; }



        public static explicit operator ApplicationTask(Tasks task) {
            return new ApplicationTask {
                PublicKey = task.PublicKey,
                Customer = null,
                Name = task.Name,
                Description = task.Description,
                TopicalTo = task.TopicalTo,
                Created = task.Created,
                Closed = task.Closed ?? DateTime.MinValue,
                Group = task.Groop,
                SubGroup = task.SubGroop,
                Price = task.Price,
                ProccessStatus = task.ProccessStatus,
                PlaceLat = task.PlaceLat,
                PlaceLng = task.PlaceLng,
                PlaceName = task.PlaceName,
                Status = task.ProccessStatus,
                AlreadyBided = false,
                RemovedForCustomer = task.IsRemovedToCustomer ?? false
            };
        }
    }
}
