using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.Mvc;
using TestStack.FluentMVCTesting.Internal;

namespace TestStack.FluentMVCTesting
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
            if (_viewResult.Model == null)
                throw new ViewResultModelAssertionException("Expected view model, but was null.");

            var castedModel = _viewResult.Model as TModel;
            if (castedModel == null)
                throw new ViewResultModelAssertionException(string.Format("Expected view model to be of type '{0}', but it is actually of type '{1}'.", typeof(TModel).Name, _viewResult.Model.GetType().Name));

            return new ModelTest<TModel>(_controller);
        }

        public ModelTest<TModel> WithModel<TModel>(TModel expectedModel) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (model != expectedModel)
                throw new ViewResultModelAssertionException("Expected view model to be the given model, but in fact it was a different model.");

            return test;
        }

        public ModelTest<TModel> WithModel<TModel>(Expression<Func<TModel, bool>> predicate) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;

            var inspector = new ExpressionInspector();
            var modelLex = Json.Encode(model);
            var predicateLex = inspector.Inspect(predicate);
            var compiledPredicate = predicate.Compile();

            if (!compiledPredicate(model))
                throw new ViewResultModelAssertionException(string.Format(
                    "Expected view model {0} to pass the given condition ({1}), but it failed.",
                    modelLex, 
                    predicateLex));

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