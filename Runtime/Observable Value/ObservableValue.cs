using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Fofanius.Observables.ObservableValue
{
    [Serializable]
    public class ObservableValue<T> : IObservableValue<T>
    {
        public event ObservableChangedEventHandler<ObservableValueChangeEventArg<T>> Changed;

        [SerializeField, JsonProperty(nameof(Value))]
        protected internal T _value;

        [JsonIgnore]
        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        [JsonConstructor]
        protected internal ObservableValue() { }

        public ObservableValue(T value)
        {
            _value = value;
        }

        protected internal virtual bool Equals(T a, T b) => EqualityComparer<T>.Default.Equals(a, b);

        public virtual void SetValue(T value)
        {
            if (Equals(_value, value)) return;

            var previous = _value;
            SetValueWithoutNotify(value);
            RiseValueChangedInternal(value, previous);
        }

        public virtual void SetValueWithoutNotify(T value)
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