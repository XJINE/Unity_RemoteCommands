using UnityEngine;
using RemoteCommands;

public class SampleA : MonoBehaviour
{
    protected virtual void Start()
    {
        RemoteCommander.Instance.Initialize();

        Debug.Log("### Do Command ###");

        RemoteCommander.Instance.Command("SampleCommand");

        var sum = RemoteCommander.Instance.Command("SampleCommandArgsReturn", 999, 3.14f) ?? -1;
        Debug.Log(sum);

        RemoteCommander.Instance.Command("SampleCommandPrivate");

        RemoteCommander.Instance.Command("SampleCommandStatic");

        // NOTE:
        // If you want to check commands.

        //Debug.Log("### Command List ###");
        //
        //foreach (var command in RemoteCommander.Instance.Commands)
        //{
        //    Debug.Log(command.Key + " : " + command.Value);
        //}
    }

    [RemoteCommand(ID = "SampleCommand")]
    public void SampleCommand()
    {
        Debug.Log("SampleCommand");
    }

    [RemoteCommand(ID = "SampleCommand")]
    public void SampleCommandConflict()
    {
        Debug.Log("SampleCommandConflict");
    }

    [RemoteCommand]
    public float SampleCommandArgsReturn(int ivalue, float fvalue)
    {
        Debug.Log("SampleCommandReturn " + ivalue + ", " + fvalue);
        return ivalue + fvalue;
    }

    [RemoteCommand]
    private void SampleCommandPrivate()
    {
        Debug.Log("SampleCommandPrivate");
    }

    [RemoteCommand]
    public static void SampleCommandStatic()
    {
        Debug.Log("SampleCommandStatic");
    }

    [RemoteCommand(ID = "SampleCommandVirtual")]
    public virtual void SampleCommandOverride()
    {
        Debug.Log("SampleA.SampleCommandOverride");
    }
}