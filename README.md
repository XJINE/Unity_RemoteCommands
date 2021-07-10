# Unity_RemoteCommand

``RemoteCommand`` provides a simple logic to execute your methods from remote.

## Import to Your Project

Import this .unitypackage and the dependencies.

- [Unity_SingletonMonoBehaviour](https://github.com/XJINE/Unity_SingletonMonoBehaviour)
- [Unity_IInitializable](https://github.com/XJINE/Unity_IInitializable)

## How to Use

```csharp
[RemoteCommand(ID = "SampleCommandA")]
public void SampleCommandA()
{
    Debug.Log("SampleCommandA");
}
```

```csharp
RemoteCommander.Instance.Command("SampleCommandA");
```

Set ``RemoteCommand`` attribute and the unique id.
Then, call ``RemoteCommander.Command()`` with id. That's all.

```csharp
[RemoteCommand]
public void SampleCommandB()
{
    Debug.Log("SampleCommand");
}
```

Function name will be set as ID when omitting it.

### Various Types

``RemoteCommand`` attribute is valid for ``public``, ``private``, ``protected`` and ``static`` methods.

```csharp
[RemoteCommand]
private void SampleCommandPrivate(){}

[RemoteCommand]
public static void SampleCommandStatic(){}
```

### Args and Return Value

``Command`` method can set some args and we can get the return value.

```csharp
[RemoteCommand]
public float SampleCommandArgsReturn(int ivalue, float fvalue)
{
    Debug.Log("SampleCommandReturn " + ivalue + ", " + fvalue);
    return ivalue + fvalue;
}
…
object sum = RemoteCommander.Instance.Command(1, 999, 3.14f);
```

## Limitation

``RemoteCommand`` must be declared in ``MonoBehaviour`` (or that inheritance).
And the instances are must be in the scene before it starts.

``RemoteCommand.ID`` is must be unique in your scene.

### Override

```csharp
public class SampleA : MonoBehaviour
{
    [RemoteCommand(ID = "SampleCommandVirtual")]
    public virtual void SampleCommandOverride()
    {
        Debug.Log("SampleA.SampleCommandOverride");
    }
}
public class SampleB : SampleA
{
    [RemoteCommand(ID = "SampleCommandOverride")]
    public override void SampleCommandOverride()
    {
        Debug.Log("SampleB.SampleCommandOverride");
    }
}
…
RemoteCommander.Instance.Command("SampleCommandVirtual");
RemoteCommander.Instance.Command("SampleCommandOverride");
```

When the ``Command("SampleCommandVirtual")`` and ``Command("SampleCommandOverride")`` are invoked with ``SampleB`` instance, it shows the same result.