namespace Fofanius.Observables.ObservableValue
{
    public interface IReadOnlyObservableValue<T> : IObservable<ObservableValueChangeEventArg<T>>
    {
        T Value { get; }
    }
}