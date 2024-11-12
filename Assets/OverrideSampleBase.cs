using UnityEngine;
using RemoteCommands;

public class OverrideSampleBase : MonoBehaviour
{
    [RemoteCommand(ID = "SampleCommandVirtual")]
    public virtual void SampleCommandOverride()
    {
        Debug.Log(nameof(OverrideSampleBase) + "." + nameof(SampleCommandOverride) + " is called.");
    }
}