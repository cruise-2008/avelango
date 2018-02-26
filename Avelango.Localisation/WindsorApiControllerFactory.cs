using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using Avelango.Models.Abstractions.Factory;

namespace Avelango.Localisation
{
    public class ReleasingControllerFactory : IHttpControllerFactory
    {
        public IHttpController CreateController(HttpControllerContext controllerContext, String controllerName)
        {
            return new HttpControllerContext() as IHttpController;
        }

        public void ReleaseController(HttpControllerContext controllerContext, IHttpController controller)
        {
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return new Dictionary<string, HttpControllerDescriptor>();
        }
    }
}