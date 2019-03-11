using UnityEngine;

public class SampleA : MonoBehaviour
{
    protected virtual void Start()
    {
        RemoteCommander.Instance.Initialize();

        Debug.Log("### Do Command ###");

        RemoteCommander.Instance.Command(0);

        object randomValue = RemoteCommander.Instance.Command(1, 999, 3.14f) ?? -1;
        Debug.Log(randomValue);

        RemoteCommander.Instance.Command(2);

        RemoteCommander.Instance.Command(3);

        // NOTE:
        // If you want to check commands.

        //Debug.Log("### Command List ###");
        //
        //foreach (var command in RemoteCommander.Instance.Commands)
        //{
        //    Debug.Log(command.Key + " : " + command.Value);
        //}
    }

    [RemoteCommand(ID = 0)]
    public void SampleCommand()
    {
        Debug.Log("SampleCommand");
    }

    [RemoteCommand(ID = 0)]
    public void SampleCommandConflict()
    {
        Debug.Log("SampleCommandConflict");
    }

    [RemoteCommand(ID = 1)]
    public float SampleCommandArgsReturn(int ivalue, float fvalue)
    {
        Debug.Log("SampleCommandReturn " + ivalue + ", " + fvalue);
        return Random.value;
    }

    [RemoteCommand(ID = 2)]
    private void SampleCommandPrivate()
    {
        Debug.Log("SampleCommandPrivate");
    }

    [RemoteCommand(ID = 3)]
    public static void SampleCommandStatic()
    {
        Debug.Log("SampleCommandStatic");
    }

    [RemoteCommand(ID = 4)]
    public virtual void SampleCommandOverride()
    {
        Debug.Log("SampleA.SampleCommandOverride");
    }
}