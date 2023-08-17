using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fofanius.Observables.ObservableValue
{
    [Serializable]
    public class ObservableValue<T> : IObservableValue<T>
    {
        public event ObservableChangedEventHandler<ObservableValueChangeEventArg<T>> Changed;

        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        public ObservableValue(T value = default)
        {
            _value = value;
        }

        public void SetValue(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;

            var previous = _value;
            SetValueWithoutNotify(value);
            RiseValueChangedInternal(value, previous);
        }

        public void SetValueWithoutNotify(T value)
        {
            _value = value;
        }

        protected internal void RiseValueChangedInternal(T current, T previous)
        {
            Changed?.Invoke(new ObservableValueChangeEventArg<T>(current, previous));
        }

        public override string ToString() => $"Observable: {(_value?.ToString() ?? "NULL")}";

        public static explicit operator T(ObservableValue<T> observableValue)
        {
            if (observableValue == null) throw new ArgumentNullException(nameof(observableValue));
            return observableValue.Value;
        }
    }
}