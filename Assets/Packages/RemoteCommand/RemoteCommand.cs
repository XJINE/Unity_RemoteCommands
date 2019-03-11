using System;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class RemoteCommand : Attribute
{
    #region Property

    // CAUTION:
    // ID must be unique.

    int id;

    public int ID
    {
        get
        {
            return this.id;
        }
        set
        {
            if (!this.IsInitialized)
            {
                this.id = value;
            }
        }
    }

    public bool IsInitialized { get; protected set; }

    protected MonoBehaviour Instance { get; set; }

    protected MethodInfo MethodInfo { get; set; }

    #endregion Property

    #region Method

    public virtual bool Initialize(MonoBehaviour instance, MethodInfo methodInfo)
    {
        if (this.IsInitialized)
        {
            return false;
        }

        this.Instance = instance;
        this.MethodInfo = methodInfo;
        this.IsInitialized = true;

        return true;
    }

    public object Invoke(params object[] parameters)
    {
        return this.MethodInfo.Invoke(this.Instance, parameters);
    }

    public override string ToString()
    {
        if (this.IsInitialized)
        {
            return this.MethodInfo.ReflectedType.Name + "." + this.MethodInfo.Name;
        }

        return base.ToString();
    }

    #endregion Method
}