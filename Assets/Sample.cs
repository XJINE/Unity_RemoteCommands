using UnityEngine;
using RemoteCommands;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        RemoteCommander.Instance.Initialize();

        Debug.Log("### Invoke Command ###");

        RemoteCommander.Instance.Command(nameof(SampleCommand));
        Debug.Log(RemoteCommander.Instance.Command(nameof(SampleCommandArgsReturn), 999, 3.14f) ?? -1);
        RemoteCommander.Instance.Command(nameof(SampleCommandPrivate));
        RemoteCommander.Instance.Command("SampleCommandStatic");

        Debug.Log("### Add Command Manually ###");

        var addManually = new RemoteCommand();
            addManually.Initialize(this, GetType().GetMethod(nameof(SampleCommandAddManually)));
        RemoteCommander.Instance.RegisterCommand(addManually);
        RemoteCommander.Instance.Command(nameof(SampleCommandAddManually));

        Debug.Log("### Command List ###");

        var commandList = "";
        foreach (var command in RemoteCommander.Instance.Commands)
        {
            commandList += command.Key + " : " + command.Value + "\n";
        }
        commandList = commandList.Trim();
        Debug.Log(commandList);
    }

    [RemoteCommand]
    public void SampleCommand()
    {
        Debug.Log(nameof(SampleCommand) + " is called.");
    }

    [RemoteCommand(ID = "SampleCommand")]
    public void SampleCommandConflict()
    {
        Debug.Log(nameof(SampleCommandConflict) + " is called.");
    }

    [RemoteCommand]
    public float SampleCommandArgsReturn(int iValue, float fValue)
    {
        Debug.Log(nameof(SampleCommandArgsReturn) + " is called.");
        return iValue + fValue;
    }

    [RemoteCommand]
    private void SampleCommandPrivate()
    {
        Debug.Log(nameof(SampleCommandPrivate) + " is called.");
    }

    [RemoteCommand]
    public static void SampleCommandStatic()
    {
        Debug.Log(nameof(SampleCommandStatic) + " is called.");
    }

    public void SampleCommandAddManually()
    {
        Debug.Log(nameof(SampleCommandAddManually) + " is called.");
    }
}