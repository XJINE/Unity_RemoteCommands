# Unity_RemoteCommand

Make Unity's window transparent and overlay on desktop.

![](https://github.com/XJINE/Unity_RemoteCommand/blob/master/screenshot.png)

## Import to Your Project

You can import this asset from UnityPackage.

- [RemoteCommand.unitypackage](https://github.com/XJINE/Unity_RemoteCommand/blob/master/RemoteCommand.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_SingletonMonoBehaviour](https://github.com/XJINE/Unity_SingletonMonoBehaviour)
- [IInitializable](https://github.com/XJINE/Unity_IInitializable)

## How to Use

Set ``RemoteCommand`` attribute and the unique id.

```csharp
[RemoteCommand(ID = 0)]
public void SampleCommand()
{
    Debug.Log("SampleCommand");
}
```

Then, call `` RemoteCommander.Command() ``


### Clear Color Settings

