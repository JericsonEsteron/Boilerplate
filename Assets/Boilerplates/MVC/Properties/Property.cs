using System;
using System.Collections.Generic;

namespace MVC
{
    public class Property<P> 
    {
        private P _value;

        public Property()
        {
        }

        public Property(P initialValue)
        {
            _value = initialValue;
        }

        public P Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<P>.Default.Equals(_value, value))
                    return;

                var previousValue = _value;
                _value = value;

                OnValueChangedWithPreviousNewValue?.Invoke(previousValue, _value);
                OnValueChangedWithNewValue?.Invoke(_value);
                OnValueChanged?.Invoke();
            }
        }

        public event Action OnValueChanged;
        public event Action<P> OnValueChangedWithNewValue;

        /// <summary>
        /// First P is previous Value, Second is the new Value
        /// </summary>
        public event Action<P, P> OnValueChangedWithPreviousNewValue;

        public void Notify()
        {
            OnValueChangedWithPreviousNewValue?.Invoke(_value, _value);
            OnValueChangedWithNewValue?.Invoke(_value);
            OnValueChanged?.Invoke();
        }
    }

}
