using System;
using System.Threading;

// The point of the FacadeEvent classes is to act like a thread-safe action that doesn't support the assignment operator.
// This prevents stupid bugs when implementing the Facade pattern
public class FacadeEvent
{
    private Action _action;

    public void Connect(Action action) => _action += action;
    public void Disconnect(Action action) => _action -= action;

    // Let's be thread-safe: https://stackoverflow.com/questions/6349125/are-c-sharp-delegates-thread-safe
    public void Invoke() => Interlocked.CompareExchange(ref _action, null, null)?.Invoke();
}

// ----------------------------------------------------------------------------
// The following classes are copy-pastes of the class above, 
// but with different numbers of parameters for the Action.
// We could consider writing a script that generates this code for more parameters
// But we can also assume that above 4 parameters, we should use a payload struct
// ----------------------------------------------------------------------------

public class FacadeEvent<T>
{
    private Action<T> _action;
    public void Connect(Action<T> action) => _action += action;
    public void Disconnect(Action<T> action) => _action -= action;
    public void Invoke(T param) => Interlocked.CompareExchange(ref _action, null, null)?.Invoke(param);

}

public class FacadeEvent<T0, T1>
{
    private Action<T0, T1> _action;
    public void Connect(Action<T0, T1> action) => _action += action;
    public void Disconnect(Action<T0, T1> action) => _action -= action;
    public void Invoke(T0 param0, T1 param1) => Interlocked.CompareExchange(ref _action, null, null)?.Invoke(param0, param1);
}

public class FacadeEvent<T0, T1, T2>
{
    private Action<T0, T1, T2> _action;
    public void Connect(Action<T0, T1, T2> action) => _action += action;
    public void Disconnect(Action<T0, T1, T2> action) => _action -= action;
    public void Invoke(T0 param0, T1 param1, T2 param2) => Interlocked.CompareExchange(ref _action, null, null)?.Invoke(param0, param1, param2);
}

public class FacadeEvent<T0, T1, T2, T3>
{
    private Action<T0, T1, T2, T3> _action;
    public void Connect(Action<T0, T1, T2, T3> action) => _action += action;
    public void Disconnect(Action<T0, T1, T2, T3> action) => _action -= action;
    public void Invoke(T0 param0, T1 param1, T2 param2, T3 param3) => Interlocked.CompareExchange(ref _action, null, null)?.Invoke(param0, param1, param2, param3);
}
