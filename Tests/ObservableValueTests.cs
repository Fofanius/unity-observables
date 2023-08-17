using System.Collections.Generic;
using Fofanius.Observables.ObservableValue;
using NUnit.Framework;

public class ObservableValueTests
{
    [Test]
    public void Test_ValueInitializeViaConstructor_Struct()
    {
        const int originValue = 47;

        var observableInt = new ObservableValue<int>(originValue);

        Assert.AreEqual(originValue, observableInt.Value, "Wrong value assigned in constructor!");
        Assert.AreEqual(originValue, (int)observableInt, "Wrong value returned by explicit operator!");
    }

    [Test]
    public void Test_ValueSet_Struct()
    {
        const int originValue = 47;

        var observableInt = new ObservableValue<int>(originValue);
        Assert.AreEqual(originValue, observableInt.Value, "Wrong value assigned in constructor!");

        const int nextValue = 53;

        observableInt.Value = nextValue;
        Assert.AreEqual(nextValue, observableInt.Value, "Value wasn't updated!");
    }
    
    [Test]
    public void Test_ValueSet_Class()
    {
        var observable = new ObservableValue<TestClassForReferenceComparing>();
        Assert.AreEqual(default, observable.Value , "Wrong value assigned in constructor!");

        var first = new TestClassForReferenceComparing();

        observable.Value = first;
        Assert.AreEqual(first, observable.Value, "Value wasn't updated from null to new instance!");

        var second = new TestClassForReferenceComparing();
        observable.Value = second;
        
        Assert.AreEqual(second, observable.Value, "Value wasn't updated to new instance!");

        observable.Value = default;
        Assert.AreEqual(default, observable.Value, "Value wasn't updated to null (default)!");
    }

    [Test]
    public void Test_ChangeEventRaised_Struct()
    {
        Test_ChangedEventRaised(1, 2);
    }
    
    [Test]
    public void Test_ChangeEventRaised_Class()
    {
        Test_ChangedEventRaised("first", "second");
    }

    private static void Test_ChangedEventRaised<T>(T a, T b)
    {
        var receiver = new List<ObservableValueChangeEventArg<T>>();
        var observable = new ObservableValue<T>();

        observable.Changed += delegate(ObservableValueChangeEventArg<T> arg) { receiver.Add(arg); };

        observable.Value = a;
        observable.Value = b;

        Assert.AreEqual(2, receiver.Count);
        Assert.AreEqual(a, receiver[0].Current);
        Assert.AreEqual(b, receiver[1].Current);
    }

    private class TestClassForReferenceComparing
    {
        
    }
    
}