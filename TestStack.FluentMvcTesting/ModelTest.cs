using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public interface IModelTest<TModel>
    {
        IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError);
        IModelTest<TModel> AndNoModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithNoError);
        IModelErrorTest<TModel> AndModelError(string errorKey);
        void AndNoModelErrors();
    }

    public class ModelTest<TModel> : IModelTest<TModel>
    {
        private readonly Controller _controller;
        private string ControllerName => _controller.GetType().Name;

        public ModelTest(Controller controller)
        {
            _controller = controller;
        }

        public IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError)
        {
            var member = ((MemberExpression)memberWithError.Body).Member.Name;
            if (!_controller.ModelState.ContainsKey(member) || _controller.ModelState[member].Errors.Count == 0)
                throw new ViewResultModelAssertionException($"Expected controller '{ControllerName}' to have a model error for member '{member}', but none found.");
            return new ModelErrorTest<TModel>(this, member, _controller.ModelState[member].Errors);
        }

        public IModelTest<TModel> AndNoModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithNoError)
        {
            var member = ((MemberExpression)memberWithNoError.Body).Member.Name;
            if (_controller.ModelState.ContainsKey(member))
                throw new ViewResultModelAssertionException($"Expected controller '{ControllerName}' to have no model errors for member '{member}', but found some.");
            return this;
        }

        public IModelErrorTest<TModel> AndModelError(string errorKey)
        {
            if (!_controller.ModelState.ContainsKey(errorKey) || _controller.ModelState[errorKey].Errors.Count == 0)
                throw new ViewResultModelAssertionException($"Expected controller '{ControllerName}' to have a model error against key '{errorKey}', but none found.");
            return new ModelErrorTest<TModel>(this, errorKey, _controller.ModelState[errorKey].Errors);
        }

        public void AndNoModelErrors()
        {
            if (!_controller.ModelState.IsValid)
                throw new ViewResultModelAssertionException($"Expected controller '{ControllerName}' to have no model errors, but it had some.");
        }
    }
}