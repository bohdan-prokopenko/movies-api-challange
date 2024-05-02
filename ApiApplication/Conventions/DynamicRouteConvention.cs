using ApiApplication.Controllers;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ApiApplication.Conventions {
    public class DynamicRouteConvention : IControllerModelConvention {
        public void Apply(ControllerModel controller) {

            controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel {
                Template = controller.ControllerType.Name switch {
                    nameof(ShowTimesController) => "api/[controller]",
                    _ => throw new System.NotImplementedException()
                }
            };
        }
    }
}
