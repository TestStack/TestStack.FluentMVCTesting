using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public interface IModelErrorTest<TModel>
    {
        IModelTest<TModel> ThatEquals(string errorMessage);
        IModelTest<TModel> BeginningWith(string beginMessage);
        IModelTest<TModel> EndingWith(string endMessage);
        IModelTest<TModel> Containing(string containsMessage);
        IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError);
        IModelTest<TModel> AndNoModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithNoError);
        IModelErrorTest<TModel> AndModelError(string errorKey);
    }

    public class ModelErrorTest<TModel> : IModelErrorTest<TModel>
    {
        private readonly IModelTest<TModel> _modelTest;
        private readonly string _errorKey;
        private readonly List<string> _errors;
        private string InflectedErrors => string.Join(", ", _errors);

        public ModelErrorTest(IModelTest<TModel> modelTest, string errorKey, IEnumerable<ModelError> errors)
        {
            _modelTest = modelTest;
            _errorKey = errorKey;
            _errors = errors.Select(e => e.ErrorMessage).ToList();
        }

        public IModelTest<TModel> ThatEquals(string errorMessage)
        {
            if (!_errors.Any(e => e == errorMessage))
                throw new ModelErrorAssertionException($"Expected error message for key '{_errorKey}' to be '{errorMessage}', but instead found '{InflectedErrors}'.");
            return _modelTest;
        }

        public IModelTest<TModel> BeginningWith(string beginMessage)
        {
            if (!_errors.Any(e => e.StartsWith(beginMessage)))
                throw new ModelErrorAssertionException($"Expected error message for key '{_errorKey}' to start with '{beginMessage}', but instead found '{InflectedErrors}'.");
            return _modelTest;
        }

        public IModelTest<TModel> EndingWith(string endMessage)
        {
            if (!_errors.Any(e => e.EndsWith(endMessage)))
                throw new ModelErrorAssertionException($"Expected error message for key '{_errorKey}' to end with '{endMessage}', but instead found '{InflectedErrors}'.");
            return _modelTest;
        }

        public IModelTest<TModel> Containing(string containsMessage)
        {
            if (!_errors.Any(e => e.Contains(containsMessage)))
                throw new ModelErrorAssertionException($"Expected error message for key '{_errorKey}' to contain '{containsMessage}', but instead found '{InflectedErrors}'.");
            return _modelTest;
        }

        public IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError) => _modelTest.AndModelErrorFor(memberWithError);

        public IModelErrorTest<TModel> AndModelError(string errorKey) => _modelTest.AndModelError(errorKey);

        public IModelTest<TModel> AndNoModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithNoError) => _modelTest.AndNoModelErrorFor(memberWithNoError);
    }
}