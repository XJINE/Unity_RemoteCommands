using UnityEngine;
using RemoteCommands;

public class OverrideSampleDerived : OverrideSampleBase
{
    private void Start()
    {
        // NOTE:
        // These are get same result.
        RemoteCommander.Instance.Command("SampleCommandVirtual");
        RemoteCommander.Instance.Command("SampleCommandOverride");
    }

    // NOTE:
    // Override method doesn't inherit base RemoteCommand attribute.

    [RemoteCommand]
    public override void SampleCommandOverride()
    {
        Debug.Log(nameof(OverrideSampleDerived) + "." + nameof(SampleCommandOverride) + " is called.");
    }
}