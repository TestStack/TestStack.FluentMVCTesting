using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TerseControllerTesting
{
    public interface IModelTest<TModel>
    {
        IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError);
        IModelErrorTest<TModel> AndModelError(string errorKey);
        void AndNoModelErrors();
    }

    public class ModelTest<TModel> : IModelTest<TModel>
    {
        private readonly Controller _controller;

        public ModelTest(Controller controller)
        {
            _controller = controller;
        }

        public IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError)
        {
            var member = ((MemberExpression)memberWithError.Body).Member.Name;
            if (!_controller.ModelState.ContainsKey(member) || _controller.ModelState[member].Errors.Count == 0)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have a model error for member '{1}', but none found.", _controller.GetType().Name, member));
            return new ModelErrorTest<TModel>(this, member, _controller.ModelState[member].Errors);
        }

        public IModelErrorTest<TModel> AndModelError(string errorKey)
        {
            if (!_controller.ModelState.ContainsKey(errorKey) || _controller.ModelState[errorKey].Errors.Count == 0)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have a model error against key '{1}', but none found.", _controller.GetType().Name, errorKey));
            return new ModelErrorTest<TModel>(this, errorKey, _controller.ModelState[errorKey].Errors);
        }

        public void AndNoModelErrors()
        {
            if (!_controller.ModelState.IsValid)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have no model errors, but it had some.", _controller.GetType().Name));
        }
    }
}