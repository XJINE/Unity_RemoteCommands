using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RemoteCommands
{
    public class RemoteCommander : SingletonMonoBehaviour<RemoteCommander>, IInitializable
    {
        #region Field

        protected Dictionary<string, RemoteCommand> commands;

        #endregion Field

        #region Property

        public bool IsInitialized { get; protected set; }

        public ReadOnlyDictionary<string, RemoteCommand> Commands { get; protected set; }

        #endregion Property

        #region Method

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public virtual bool Initialize()
        {
            if (IsInitialized)
            {
                return false;
            }

            IsInitialized = true;

            commands = new Dictionary<string, RemoteCommand>();
            Commands = new ReadOnlyDictionary<string, RemoteCommand>(commands);

            foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
            { 
                var type = monoBehaviour.GetType();

                while (type != null)
                {
                    FindCommands(monoBehaviour, type);
                    type = type.BaseType;
                }
            }

            return true;
        }

        protected void FindCommands(MonoBehaviour instance, Type type)
        {
            // NOTE:
            // DeclaredOnly needs to avoid conflict in inherit class.

            var methodInfos = type.GetMethods(BindingFlags.Public
                                            | BindingFlags.NonPublic
                                            | BindingFlags.Instance
                                            | BindingFlags.Static
                                            | BindingFlags.DeclaredOnly);

            foreach (var methodInfo in methodInfos)
            {
                var remoteCommand = Attribute.GetCustomAttribute(methodInfo, typeof(RemoteCommand)) as RemoteCommand;

                if (remoteCommand == null)
                {
                    continue;
                }

                remoteCommand.Initialize(instance, methodInfo);

                if (commands.ContainsKey(remoteCommand.ID))
                {
                    AlertIDConflict(commands[remoteCommand.ID], remoteCommand);
                    continue;
                }

                commands.Add(remoteCommand.ID, remoteCommand);
            }
        }

        public virtual object Command(params object[] parameters)
        {
            var id = parameters[0].ToString();

            if (!commands.TryGetValue(id, out var command))
            {
                AlertCommandNotFound(id);
                return null;
            }
            try
            {
                parameters = parameters.Skip(1).ToArray();
                return command.Invoke(parameters);
            }
            catch (Exception exception)
            {
                AlertInvokeError(command, exception);
                return null;
            }
        }

        protected virtual void AlertIDConflict(RemoteCommand existingCommand, RemoteCommand conflictCommand)
        {
            Debug.Log("Conflict Command ID: " + existingCommand.ID + ", " + existingCommand + ", " + conflictCommand);
        }

        protected virtual void AlertCommandNotFound(string id)
        {
            Debug.Log("Command ID '" + id + "' is not found.");
        }

        protected virtual void AlertInvokeError(RemoteCommand command, Exception exception)
        {
            Debug.Log("Invoke " + command + " is failed. " + exception);
        }

        #endregion Method
    }
}