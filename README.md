# Unity_RemoteCommand

``RemoteCommand`` provides a simple logic to execute your methods from remote.

## Import to Your Project

You can import this asset from UnityPackage.

- [RemoteCommand.unitypackage](https://github.com/XJINE/Unity_RemoteCommand/blob/master/RemoteCommand.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_SingletonMonoBehaviour](https://github.com/XJINE/Unity_SingletonMonoBehaviour)
- [Unity_IInitializable](https://github.com/XJINE/Unity_IInitializable)

## How to Use

Set ``RemoteCommand`` attribute and the unique id.

```csharp
[RemoteCommand(ID = 0)]
public void SampleCommand()
{
    Debug.Log("SampleCommand");
}
```

Then, call ``RemoteCommander.Command()`` with id.

```csharp
RemoteCommander.Instance.Command(0);
```

That's all.

### Various Types

``RemoteCommand`` attribute is valid for ``public``, ``private``, ``protected`` and ``static`` methods.

```csharp
[RemoteCommand(ID = 2)]
private void SampleCommandPrivate(){}

[RemoteCommand(ID = 3)]
public static void SampleCommandStatic(){}
```

### Args and Return Value

Command method can set some args and we can get the return value from it.

```csharp
[RemoteCommand(ID = 1)]
public float SampleCommandArgsReturn(int ivalue, float fvalue)
{
    Debug.Log("SampleCommandReturn " + ivalue + ", " + fvalue);
    return ivalue + fvalue;
}

…

object sum = RemoteCommander.Instance.Command(1, 999, 3.14f)
```

## Limitation

- ``RemoteCommand`` must be declared in ``MonoBehaviour`` (or that inheritance).
- ``RemoteCommand.ID`` is must be unique in your scene.
