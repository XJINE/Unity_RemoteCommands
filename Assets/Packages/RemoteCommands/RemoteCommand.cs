using System;
using System.Reflection;
using UnityEngine;

namespace RemoteCommands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
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

        public bool IsInitialized { get; protected set; }

        protected MonoBehaviour Instance { get; set; }

        protected MethodInfo MethodInfo { get; set; }

        #endregion Property

        #region Method

        public bool Initialize(MonoBehaviour instance, MethodInfo methodInfo)
        {
            if (IsInitialized)
            {
                return false;
            }

            ID ??= methodInfo.Name;

            Instance      = instance;
            MethodInfo    = methodInfo;
            IsInitialized = true;

            return true;
        }

        public object Invoke(params object[] parameters)
        {
            return MethodInfo.Invoke(Instance, parameters);
        }

        public override string ToString()
        {
            return MethodInfo.ReflectedType.Name + "." + MethodInfo.Name;;
        }

        #endregion Method
    }
}