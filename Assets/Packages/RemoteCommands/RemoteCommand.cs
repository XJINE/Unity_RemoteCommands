using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace RemoteCommands {
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
public class RemoteCommand : Attribute
{
    #region Property

    private string _id; // ID must be unique.
    public  string ID
    {
        get => _id;
        set { if (!IsInitialized) { _id = value; } }
    }

    public int           Hash          { get; private set; }
    public bool          IsInitialized { get; private set; }
    public MonoBehaviour Instance      { get; private set; }
    public MemberInfo    MemberInfo    { get; private set; }

    #endregion Property

    #region Method

    public bool Initialize(MonoBehaviour instance, MemberInfo memberInfo, string id = null)
    {
        if (IsInitialized)
        {
            return false;
        }

        ID          ??= id ?? memberInfo.Name;
        Hash          = ComputeHash(ID);
        Instance      = instance;
        MemberInfo    = memberInfo;
        IsInitialized = true;

        return true;
    }

    public object Invoke(params object[] parameters)
    {
        switch (MemberInfo.MemberType)
        {
            case MemberTypes.Field:
                var fieldInfo = (FieldInfo)MemberInfo;
                    fieldInfo.SetValue(Instance, parameters[0]);
             return fieldInfo.GetValue(Instance);

            case MemberTypes.Property:
                var propertyInfo = (PropertyInfo)MemberInfo;
                    propertyInfo.SetValue(Instance, parameters[0]);
             return propertyInfo.GetValue(Instance);
            
            case MemberTypes.Method:
                return ((MethodInfo) MemberInfo).Invoke(Instance, parameters);
        }

        return Instance;
    }

    public override string ToString()
    {
        return $"{MemberInfo?.ReflectedType?.Name}.{MemberInfo?.Name}";
    }

    private static int ComputeHash(string id)
    {
        using var sha256  = SHA256.Create();
              var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(id));

        return BitConverter.ToInt32(hash);
    }

    #endregion Method
}}