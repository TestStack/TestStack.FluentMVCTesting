using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TerseControllerTesting
{
    public interface IModelErrorTest<TModel>
    {
        IModelTest<TModel> ThatEquals(string errorMessage);
        IModelTest<TModel> BeginningWith(string beginMessage);
        IModelTest<TModel> EndingWith(string endMessage);
        IModelTest<TModel> Containing(string containsMessage);
        IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError);
        IModelErrorTest<TModel> AndModelError(string errorKey);
    }

    public class ModelErrorTest<TModel> : IModelErrorTest<TModel>
    {
        private readonly IModelTest<TModel> _modelTest;
        private readonly string _errorKey;
        private readonly List<string> _errors;

        public ModelErrorTest(IModelTest<TModel> modelTest, string errorKey, IEnumerable<ModelError> errors)
        {
            _modelTest = modelTest;
            _errorKey = errorKey;
            _errors = errors.Select(e => e.ErrorMessage).ToList();
        }

        public IModelTest<TModel> ThatEquals(string errorMessage)
        {
            if (!_errors.Any(e => e == errorMessage))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to be '{1}', but instead found '{2}'.", _errorKey, errorMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public IModelTest<TModel> BeginningWith(string beginMessage)
        {
            if (!_errors.Any(e => e.StartsWith(beginMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to start with '{1}', but instead found '{2}'.", _errorKey, beginMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public IModelTest<TModel> EndingWith(string endMessage)
        {
            if (!_errors.Any(e => e.EndsWith(endMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to end with '{1}', but instead found '{2}'.", _errorKey, endMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public IModelTest<TModel> Containing(string containsMessage)
        {
            if (!_errors.Any(e => e.Contains(containsMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to contain '{1}', but instead found '{2}'.", _errorKey, containsMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public IModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError)
        {
            return _modelTest.AndModelErrorFor(memberWithError);
        }

        public IModelErrorTest<TModel> AndModelError(string errorKey)
        {
            return _modelTest.AndModelError(errorKey);
        }
    }
}