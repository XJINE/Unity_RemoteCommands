# Unity_RemoteCommands

``RemoteCommands`` provides a simple logic to execute your methods from remote.

## Importing

You can use Package Manager or import it directly.

```
https://github.com/XJINE/Unity_RemoteCommands.git?path=Assets/Packages/RemoteCommands
```

### Dependencies

This project use following resources.

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

``Command`` method can be set some args and we can get the return value from it.

```csharp
[RemoteCommand]
public float SampleCommandArgsReturn(int ivalue, float fvalue)
{
    Debug.Log("SampleCommandReturn " + ivalue + ", " + fvalue);
    return ivalue + fvalue;
}
…
object sum = RemoteCommander.Instance.Command("SampleCommandArgsReturn", 999, 3.14f);
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