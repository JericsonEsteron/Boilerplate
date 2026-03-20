using System;
using UnityEngine;

namespace MVC
{
    public abstract class AView<M> : MonoBehaviour, IView where M : IModel
    {
        private M _model;
        private bool _isBound;

        protected M Model => _model;
        protected bool IsBound => _isBound;

        protected abstract void OnModelBound(M model);
        protected abstract void OnModelUnBound(M model);

        protected virtual void OnDestroy() 
        {
            UnBind();
        }

        public void Bind(M model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_isBound)
                throw new InvalidOperationException($"{GetType().Name} is already bound to a model.");

            _model = model;
            _isBound = true;
            OnModelBound(_model);
        }

        public void UnBind()
        {
            if (!_isBound)
                return;

            var model = _model;
            OnModelUnBound(model);
            _isBound = false;
            _model = default;
        }
    }

}
