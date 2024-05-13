using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Fofanius.Observables.ObservableValue
{
    [Serializable]
    public class RangedObservableValue<T> : ObservableValue<T> where T : IComparable<T>
    {
        public event ObservableChangedEventHandler<ObservableValueChangeEventArg<T>> MinValueChanged;
        public event ObservableChangedEventHandler<ObservableValueChangeEventArg<T>> MaxValueChanged;

        [SerializeField, JsonProperty(nameof(MinValue))] private T _minValue;
        [SerializeField, JsonProperty(nameof(MaxValue))] private T _maxValue;

        [JsonIgnore] public T MinValue
        {
            get => _minValue;
            set => SetMinValue(value);
        }

        [JsonIgnore] public T MaxValue
        {
            get => _maxValue;
            set => SetMaxValue(value);
        }

        [JsonConstructor]
        protected internal RangedObservableValue() { }

        public RangedObservableValue(T value, T minValue, T maxValue)
        {
            if (minValue is not null && maxValue is not null && minValue.CompareTo(maxValue) >= 0)
            {
                throw new ArgumentException($"Min value ({minValue}) is equal or greater then Max value ({maxValue})!");
            }

            _minValue = minValue;
            _maxValue = maxValue;

            _value = Clamp(value);
        }

        public override void SetValue(T value)
        {
            var previous = _value;

            value = Clamp(value);

            if (Equals(previous, value)) return;

            SetValueWithoutNotify(value);
            RiseValueChangedInternal(value, previous);
        }

        private void SetMinValue(T minValue)
        {
            var previousValue = _minValue;

            minValue = ClampMax(minValue);
            if (Equals(previousValue, minValue)) return;

            _minValue = minValue;
            MinValueChanged?.Invoke(new ObservableValueChangeEventArg<T>(_minValue, previousValue));

            if (_minValue is not null && _value.CompareTo(_minValue) < 0)
            {
                SetValue(_minValue);
            }
        }

        private void SetMaxValue(T maxValue)
        {
            var previousValue = _maxValue;

            maxValue = ClampMin(maxValue);
            if (Equals(previousValue, maxValue)) return;

            _maxValue = maxValue;
            MaxValueChanged?.Invoke(new ObservableValueChangeEventArg<T>(_maxValue, previousValue));

            if (_maxValue is not null && _value.CompareTo(_maxValue) > 0)
            {
                SetValue(_maxValue);
            }
        }

        public T Clamp(T value)
        {
            value = ClampMin(value);
            value = ClampMax(value);

            return value;
        }

        public T ClampMin(T value)
        {
            if (_minValue is not null && value.CompareTo(_minValue) < 0) return _minValue;
            return value;
        }

        public T ClampMax(T value)
        {
            if (_maxValue is not null && value.CompareTo(_maxValue) > 0) return _maxValue;
            return value;
        }

        public override string ToString() => $"Ranged Observable: {(_value?.ToString() ?? "NULL")} [{(_minValue?.ToString() ?? "NULL")}, {(_maxValue?.ToString() ?? "NULL")}]";
    }
}