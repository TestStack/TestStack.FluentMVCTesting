using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using RazorEngine;

namespace Xania.AspNet.Simulator
{
    internal class SimulatorActionInvoker : ControllerActionInvoker
    {
        private readonly ControllerContext _controllerContext;
        private readonly ActionDescriptor _actionDescriptor;
        private readonly FilterInfo _filterInfo;

        public SimulatorActionInvoker(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IEnumerable<Filter> filters)
        {
            _controllerContext = controllerContext;
            _actionDescriptor = actionDescriptor;
            _filterInfo = new FilterInfo(filters);
        }

        public virtual ActionResult AuthorizeAction()
        {
            var authorizationContext = InvokeAuthorizationFilters(_controllerContext,
                _filterInfo.AuthorizationFilters, _actionDescriptor);

            return authorizationContext.Result;
        }

        public virtual ActionResult InvokeAction()
        {
            return AuthorizeAction() ?? InvokeActionMethodWithFilters();
        }

        private ActionResult InvokeActionMethodWithFilters()
        {
            var controller = _controllerContext.Controller as Controller;
            if (controller != null)
            {
                controller.ViewEngineCollection = new ViewEngineCollection(new IViewEngine[]{ new RazorViewEngineSimulator() });
            }

            var parameters = GetParameterValues(_controllerContext, _actionDescriptor);
            var actionExecutedContext = InvokeActionMethodWithFilters(_controllerContext,
                _filterInfo.ActionFilters, _actionDescriptor, parameters);

            if (actionExecutedContext == null)
                throw new Exception("InvokeActionMethodWithFilters returned null");

            return actionExecutedContext.Result;
        }

        protected override object GetParameterValue(ControllerContext controllerContext, ParameterDescriptor parameterDescriptor)
        {
            var parameterName = parameterDescriptor.ParameterName;
            var value = base.GetParameterValue(controllerContext, parameterDescriptor);

            if (value == null)
                return null;

            var validationResults = ValidateModel(parameterDescriptor.ParameterType, value, controllerContext);

            var modelState = controllerContext.Controller.ViewData.ModelState;
            Func<ModelValidationResult, bool> isValidField = res => modelState.IsValidField(String.Format("{0}.{1}", parameterName, res.MemberName));

            foreach (var validationResult in validationResults.Where(isValidField).ToArray())
            {
                var subPropertyName = String.Format("{0}.{1}", parameterDescriptor.ParameterName, validationResult.MemberName);
                modelState.AddModelError(subPropertyName, validationResult.Message);
            }

            return value;
        }

        protected virtual IEnumerable<ModelValidationResult> ValidateModel(Type modelType, object modelValue, ControllerContext controllerContext)
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => modelValue, modelType);
            return ModelValidator.GetModelValidator(modelMetadata, controllerContext).Validate(null);
        }
    }

    internal class RazorViewEngineSimulator : IViewEngine
    {
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return new ViewEngineResult(new ViewSimulator(viewName), this);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }
    }

    internal class ViewSimulator : IView
    {
        private readonly string _viewName;

        public ViewSimulator(string viewName)
        {
            _viewName = viewName;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var model = viewContext.ViewData.Model;
            var html = Razor.Parse<string>("<h1>hello @Model</h1>", "simulator");

            writer.Write(html);
        }
    }
}