using System;
using System.Reflection;
using UnityEngine;

namespace RemoteCommands
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class RemoteCommand : Attribute
    {
        #region Property

        // CAUTION:
        // ID must be unique.

        private string _id;
        public string ID
        {
            get => _id;
            set { if (!IsInitialized) { _id = value; } }
        }

        public    bool          IsInitialized { get; protected set; }
        protected MonoBehaviour Instance      { get;           set; }
        protected MemberInfo    MemberInfo    { get;           set; }

        #endregion Property

        #region Method

        public bool Initialize(MonoBehaviour instance, MemberInfo memberInfo)
        {
            if (IsInitialized)
            {
                return false;
            }

            ID ??= memberInfo.Name;

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
                    var fieldInfo = ((FieldInfo)MemberInfo);
                        fieldInfo.SetValue(Instance, parameters[0]);
                 return fieldInfo.GetValue(Instance);

                case MemberTypes.Property:
                    var propertyInfo = ((PropertyInfo)MemberInfo);
                        propertyInfo.SetValue(Instance, parameters[0]);
                 return propertyInfo.GetValue(Instance);
                
                case MemberTypes.Method:
                    return ((MethodInfo) MemberInfo).Invoke(Instance, parameters);
            }

            return Instance;
        }

        public override string ToString()
        {
            return MemberInfo.ReflectedType.Name + "." + MemberInfo.Name;;
        }

        #endregion Method
    }
}