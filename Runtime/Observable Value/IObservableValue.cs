namespace Fofanius.Observables.ObservableValue
{
    public interface IObservableValue<T> : IReadOnlyObservableValue<T>
    {
        void SetValue(T value);
        void SetValueWithoutNotify(T value);
    }
}