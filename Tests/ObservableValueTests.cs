using System.Collections.Generic;
using Fofanius.Observables.ObservableValue;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

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
        var observable = new ObservableValue<TestClassForReferenceComparing>(default);
        Assert.AreEqual(default, observable.Value, "Wrong value assigned in constructor!");

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

    [Test]
    public void Test_RangedValue_Basic()
    {
        const float a = -5f;
        const float b = 5f;
        var observable = new RangedObservableValue<float>(1, a, b);

        observable.Value = 3;
        Assert.AreEqual(3, observable.Value);

        observable.Value = a - 1;
        Assert.AreEqual(a, observable.Value);

        observable.Value = b + 1;
        Assert.AreEqual(b, observable.Value);

        observable.MaxValue = b * 2;
        observable.Value = b + 1;
        Assert.AreEqual(b + 1, observable.Value);

        observable.MaxValue = 0;
        Assert.AreEqual(0, observable.Value);

        observable.MinValue = a * 2;
        observable.Value = a - 1;
        Assert.AreEqual(a - 1, observable.Value);

        observable.MinValue = a;
        Assert.AreEqual(a, observable.Value);
    }

    [Test]
    public void Test_RangedValue_Serialization_Float()
    {
        const float a = -5f;
        const float b = 5f;
        var observable = new RangedObservableValue<float>(1, a, b);

        var json = JsonConvert.SerializeObject(observable);
        var instance = JsonConvert.DeserializeObject<RangedObservableValue<float>>(json);

        Assert.AreEqual(observable.Value, instance.Value);
        Assert.AreEqual(observable.MinValue, instance.MinValue);
        Assert.AreEqual(observable.MaxValue, instance.MaxValue);
    }

    private static void Test_ChangedEventRaised<T>(T a, T b)
    {
        var receiver = new List<ObservableValueChangeEventArg<T>>();
        var observable = new ObservableValue<T>(default);

        observable.Changed += delegate(ObservableValueChangeEventArg<T> arg) { receiver.Add(arg); };

        observable.Value = a;
        observable.Value = b;

        Assert.AreEqual(2, receiver.Count);
        Assert.AreEqual(a, receiver[0].Current);
        Assert.AreEqual(b, receiver[1].Current);
    }

    [Test]
    public void Test_ObservableValueNewtonsoftSerialization_StringValue()
    {
        Test_ObservableValueNewtonsoftSerialization("Hello, Newtonsoft!");
    }

    [Test]
    public void Test_ObservableValueNewtonsoftSerialization_Float()
    {
        Test_ObservableValueNewtonsoftSerialization(Mathf.PI);
    }

    private void Test_ObservableValueNewtonsoftSerialization<T>(T valueToTest)
    {
        var observableOrigin = new ObservableValue<T>(valueToTest);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(observableOrigin);
        var observableDeserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableValue<T>>(json);

        Assert.AreEqual(observableOrigin.Value, observableDeserialized.Value, $"Serialization progress failed! '{observableDeserialized.Value}' is not equal to '{observableOrigin.Value}'!");
    }

    private class TestClassForReferenceComparing { }
}