using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    public abstract class AController<V, M> : MonoBehaviour, IController where V : AView<M> where M : IModel, new()
    {
        private V _view;
        private M _model;
        
        protected V View => _view;
        protected M Model => _model;

        protected abstract void OnModelBound();
        protected abstract void OnModelUnBound();
        
        protected void OnEnable() 
        {
            Bind();
        }

        protected void OnDisable() 
        {
            UnBind();
        }

        private void Bind()
        {
            if(_model != null)
                return;

            _model = CreateModel();
            _view = FindView();
            
            _view.Bind(_model);
            OnModelBound();
        }

        private void UnBind()
        {
            if(_model == null)
                return;

            _model = default;
            _view.UnBind();
            OnModelUnBound();
        }

        private M CreateModel()
        {
            return new M();
        }

        private V FindView()
        {
            return GetComponent<V>();
        }
    }

}
