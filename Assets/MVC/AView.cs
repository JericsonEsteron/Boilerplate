using System.Collections;
using System.Collections.Generic;
using MVC;
using UnityEngine;

namespace MVC
{
    public abstract class AView<M> : MonoBehaviour, IView where M : IModel
    {
        private M _model;

        protected M Model => _model;

        protected abstract void OnModelBound();
        protected abstract void OnModelUnBound();

        public void Bind(M model)
        {
            _model = model;
            OnModelBound();
        }

        public void UnBind()
        {
            _model = default;
            OnModelUnBound();
        }
    }

}
