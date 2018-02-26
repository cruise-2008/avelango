using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avelango.Models.Orm;

namespace Avelango.Models.Application
{
    public class ApplicationTaskBid
    {
        public Guid PublicKey { get; set; }
        public string Message { get; set; }
        public int Price { get; set; }
        public DateTime Created { get; set; }
        public bool Denied { get; set; }



        public static explicit operator ApplicationTaskBid(TaskBids taskBid) {
            return new ApplicationTaskBid {
                PublicKey = taskBid.PublicKey,
                Message = taskBid.Message,
                Price = taskBid.Price,
                Created = taskBid.Created,
                Denied = taskBid.Denied,
            };
        }
    }
}
