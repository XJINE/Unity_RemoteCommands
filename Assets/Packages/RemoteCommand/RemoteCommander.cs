using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;

public class RemoteCommander : SingletonMonoBehaviour<RemoteCommander>, IInitializable
{
    #region Field

    protected Dictionary<int, RemoteCommand> commands;

    #endregion Field

    #region Property

    public bool IsInitialized { get; protected set; }

    public ReadOnlyDictionary<int, RemoteCommand> Commands { get; protected set; }

    #endregion Property

    #region Method

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public virtual string Initialize(int value) { return null; }

    public virtual bool Initialize()
    {
        if (this.IsInitialized)
        {
            return false;
        }

        this.commands = new Dictionary<int, RemoteCommand>();

        var monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();

        foreach (var monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            while (type != null)
            {
                FindCommands(monoBehaviour, type);
                type = type.BaseType;
            }
        }

        this.Commands = new ReadOnlyDictionary<int, RemoteCommand>(this.commands);

        this.IsInitialized = true;

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
            var remoteCommand = Attribute.GetCustomAttribute
                                (methodInfo, typeof(RemoteCommand)) as RemoteCommand;

            if (remoteCommand == null)
            {
                continue;
            }

            remoteCommand.Initialize(instance, methodInfo);

            if (this.commands.ContainsKey(remoteCommand.ID))
            {
                AlertIDConflict(this.commands[remoteCommand.ID], remoteCommand);
                continue;
            }

            this.commands.Add(remoteCommand.ID, remoteCommand);
        }
    }

    public virtual object Command(int id, params object[] parameters)
    {
        if (!commands.ContainsKey(id))
        {
            AlertCommandNotFound(id);
            return null;
        }

        RemoteCommand command = this.commands[id];

        try
        {
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
        Debug.Log("Conflict Command ID: " + existingCommand.ID + ", "
                  + existingCommand.ToString() + ", " + conflictCommand.ToString());
    }

    protected virtual void AlertCommandNotFound(int id)
    {
        Debug.Log("Command ID '" + id + "' is not found.");
    }

    protected virtual void AlertInvokeError(RemoteCommand command, Exception exception)
    {
        Debug.Log("Invoke " + command.ToString() + " is falied. " + exception.ToString());
    }

    #endregion Method
}