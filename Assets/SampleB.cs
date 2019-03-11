using UnityEngine;

public class SampleB : SampleA
{
    protected override void Start()
    {
        base.Start();

        // NOTE:
        // These are get same result.

        RemoteCommander.Instance.Command(4);
        RemoteCommander.Instance.Command(5);
    }

    // NOTE:
    // Override method doesn't inherit base RemoteCommand attribute.

    [RemoteCommand(ID = 5)]
    public override void SampleCommandOverride()
    {
        Debug.Log("SampleB.SampleCommandOverride");
    }
}