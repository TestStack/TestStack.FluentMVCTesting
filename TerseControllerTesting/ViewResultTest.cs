using System;
using System.Web.Mvc;

namespace TerseControllerTesting
{
    public class ViewResultTest
    {
        private readonly ViewResultBase _viewResult;
        private readonly Controller _controller;

        public ViewResultTest(ViewResultBase viewResult, Controller controller)
        {
            _viewResult = viewResult;
            _controller = controller;
        }

        public ModelTest<TModel> WithModel<TModel>() where TModel : class
        {
            var castedModel = _viewResult.Model as TModel;
            if (castedModel == null)
                throw new ViewResultModelAssertionException(string.Format("Expected view model to be of type '{0}'. It is actually null.", typeof(TModel).Name));

            return new ModelTest<TModel>(_controller);
        }

        public ModelTest<TModel> WithModel<TModel>(TModel expectedModel) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (model != expectedModel)
                throw new ViewResultModelAssertionException("Expected view model to be passed in model, but in fact it was a different model.");

            return test;
        }

        public ModelTest<TModel> WithModel<TModel>(Func<TModel, bool> predicate) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (!predicate(model))
                throw new ViewResultModelAssertionException("Expected view model to pass a condition, but it failed.");

            return test;
        }

        public ModelTest<TModel> WithModel<TModel>(Action<TModel> assertions) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            assertions(model);

            return test;
        }
    }
}