using RemoteCommands;
using UnityEngine;

public class FieldPropertySample : MonoBehaviour
{
    [RemoteCommand]
    public int sampleIntField = 0;

    private float _sampleFloatProperty = 0f;

    [RemoteCommand]
    public float SampleFloatProperty
    {
        get => _sampleFloatProperty;
        set
        {
            Debug.Log(nameof(SampleFloatProperty) + " set is called.");
            _sampleFloatProperty = value;
        }
    }

    private void Start()
    {
        RemoteCommander.Instance.Initialize();
        Debug.Log(RemoteCommander.Instance.Command(nameof(sampleIntField),      123));
        Debug.Log(RemoteCommander.Instance.Command(nameof(SampleFloatProperty), 3.14f));
    }
}