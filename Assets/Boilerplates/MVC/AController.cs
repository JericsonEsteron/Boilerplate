using UnityEngine;

namespace MVC
{
    [DefaultExecutionOrder(-10)]
    public abstract class AController<V, M> : MonoBehaviour, IController where V : AView<M> where M : IModel, new()
    {
        private V _view;
        private M _model;
        private bool _isBound;

        protected V View => _view;
        protected M Model => _model;

        protected virtual void InitializeModel(M model) { }
        protected abstract void OnModelBound(M model);
        protected abstract void OnModelUnBound(M model);
        protected virtual void OnModelDisposed(M model) { }

        protected virtual void Awake()
        {
            _model = CreateModel();
            InitializeModel(_model);

            _view = FindView();
            if (_view == null)
                throw new MissingComponentException($"{GetType().Name} requires a {typeof(V).Name} component on the same GameObject.");
        }
        
        protected virtual void OnEnable() 
        {
            if (_isBound)
                return;

            _view.Bind(_model);
            OnModelBound(_model);
            _isBound = true;
        }

        protected virtual void OnDisable()
        {
            if (!_isBound)
                return;

            OnModelUnBound(_model);
            _view.UnBind();
            _isBound = false;
        }

        protected virtual void OnDestroy() 
        {
            if (_isBound)
            {
                OnModelUnBound(_model);
                _view.UnBind();
                _isBound = false;
            }

            OnModelDisposed(_model);
            _model = default;
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

    public abstract class AController<M> : MonoBehaviour, IController where M : IModel, new()
    {
        private M _model;
        private bool _isBound;
        
        public M Model => _model;

        protected virtual void InitializeModel(M model) { }
        protected abstract void OnModelBound();
        protected abstract void OnModelUnBound();
        protected virtual void OnModelDisposed(M model) { }

        protected virtual void Awake()
        {
            _model = CreateModel();
            InitializeModel(_model);
        }
        
        protected void OnEnable() 
        {
            Bind();
        }

        protected void OnDisable()
        {
            UnBind();
        }

        protected void OnDestroy() 
        {
            if (_isBound)
            {
                OnModelUnBound();
                _isBound = false;
            }

            OnModelDisposed(_model);
            _model = default;
        }

        private void Bind()
        {
            if(_isBound)
                return;

            if (_model == null)
                _model = CreateModel();

            OnModelBound();
            _isBound = true;
        }

        private void UnBind()
        {
            if(!_isBound)
                return;

            OnModelUnBound();
            _isBound = false;
        }

        private M CreateModel()
        {
            return new M();
        }
    }

}
