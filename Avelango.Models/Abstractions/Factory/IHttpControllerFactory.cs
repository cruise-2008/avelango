using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Controllers;

namespace Avelango.Models.Abstractions.Factory
{
    public interface IHttpControllerFactory
    {
        IHttpController CreateController(HttpControllerContext controllerContext, String controllerName);

        void ReleaseController(HttpControllerContext controllerContext, IHttpController controller);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is better handled as a method.")]
        IDictionary<string, HttpControllerDescriptor> GetControllerMapping();
    }
}
