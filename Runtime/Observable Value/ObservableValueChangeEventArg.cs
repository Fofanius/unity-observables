using System;

namespace Fofanius.Observables.ObservableValue
{
    [Serializable]
    public struct ObservableValueChangeEventArg<T>
    {
        public T Previous;
        public T Current;

        public ObservableValueChangeEventArg(T current, T previous = default)
        {
            Current = current;
            Previous = previous;
        }

        public override string ToString() => $"Change: {Current?.ToString() ?? "NULL"} (was {Previous?.ToString() ?? "NULL"})";

        public static explicit operator T(ObservableValueChangeEventArg<T> eventArg)
        {
            return eventArg.Current;
        }
    }
}