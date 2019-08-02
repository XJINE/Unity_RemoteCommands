using UnityEngine;

public class SampleB : SampleA
{
    protected override void Start()
    {
        base.Start();

        // NOTE:
        // These are get same result.

        RemoteCommander.Instance.Command("SampleCommandVirtual");
        RemoteCommander.Instance.Command("SampleCommandOverride");
    }

    // NOTE:
    // Override method doesn't inherit base RemoteCommand attribute.

    [RemoteCommand(ID = "SampleCommandOverride")]
    public override void SampleCommandOverride()
    {
        Debug.Log("SampleB.SampleCommandOverride");
    }
}