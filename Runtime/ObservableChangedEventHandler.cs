namespace Fofanius.Observables
{
    // FIXME: заменить на Unity события 
    public delegate void ObservableChangedEventHandler<in T>(T arg);
}