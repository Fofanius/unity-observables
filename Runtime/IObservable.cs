namespace Fofanius.Observables
{
    public interface IObservable<T>
    {
        event ObservableChangedEventHandler<T> Changed;
    }
}