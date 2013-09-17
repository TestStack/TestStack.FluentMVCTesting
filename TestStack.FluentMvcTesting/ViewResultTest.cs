//
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
//
using ImpromptuInterface;

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
        
        public TMember WithViewBag<TMember>(string psMemberName)
        {
            TMember member = default(TMember);
            var target = _viewResult.ViewBag;

            IEnumerable<string> memberNames = Impromptu.GetMemberNames(_viewResult.ViewBag, true);
            if ( !memberNames.Contains(psMemberName) )
                throw new MissingMemberException("ViewBag",psMemberName);

            member = Impromptu.InvokeGet(target, psMemberName);

            return member;
        }
        public TProperty WithViewData<TProperty>(string psKeyName)
        {
            if (_viewResult.ViewData == null || _viewResult.ViewData.Count <= 0 || !_viewResult.ViewData.ContainsKey(psKeyName))
                throw new KeyNotFoundException(string.Format("Exception with ViewData, '{0}' key not found",psKeyName));

            var target = _viewResult.ViewData;
            TProperty propertyValue = default(TProperty);

            if (target != null)
                propertyValue = (TProperty)target[psKeyName];

            return propertyValue;
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

        public ModelTest<TModel> WithModel<TModel>(Func<TModel, bool> predicate) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (!predicate(model))
                throw new ViewResultModelAssertionException("Expected view model to pass the given condition, but it failed.");

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