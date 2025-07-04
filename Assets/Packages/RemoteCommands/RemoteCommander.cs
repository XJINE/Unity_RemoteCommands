﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RemoteCommands {
public class RemoteCommander : SingletonMonoBehaviour<RemoteCommander>, IInitializable
{
    #region Field

    private Dictionary<string, RemoteCommand> _commands;

    #endregion Field

    #region Property

    public bool IsInitialized { get; protected set; }

    public ReadOnlyDictionary<string, RemoteCommand> Commands { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void Start()
    {
        // CAUTION:
        // To scan other objects, Initialize shouldn't be called in Awake.
        Initialize();
    }

    public virtual bool Initialize()
    {
        if (IsInitialized)
        {
            return false;
        }

        IsInitialized = true;

        _commands = new Dictionary        <string, RemoteCommand>();
        Commands  = new ReadOnlyDictionary<string, RemoteCommand>(_commands);

        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>(includeInactive:true))
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

    private void FindCommands(MonoBehaviour instance, IReflect type)
    {
        // NOTE:
        // DeclaredOnly needs to avoid conflict in inherit class.

        var memberInfos = type.GetMembers(BindingFlags.Public
                                        | BindingFlags.NonPublic
                                        | BindingFlags.Instance
                                        | BindingFlags.Static
                                        | BindingFlags.DeclaredOnly);

        foreach (var methodInfo in memberInfos)
        {
            var remoteCommand = Attribute.GetCustomAttribute(methodInfo, typeof(RemoteCommand)) as RemoteCommand;

            if (remoteCommand == null)
            {
                continue;
            }

            remoteCommand.Initialize(instance, methodInfo);

            AddCommand(remoteCommand);
        }
    }

    public virtual object Command(params object[] parameters)
    {
        var id = parameters[0].ToString();

        if (!_commands.TryGetValue(id, out var command))
        {
            Debug.Log($"Command ID '{id}' is not found.");
            return null;
        }
        try
        {
            parameters = parameters.Skip(1).ToArray();
            return command.Invoke(parameters);
        }
        catch (Exception exception)
        {
            Debug.Log($"Invoke '{command}' is failed.\n{exception}");
            return null;
        }
    }

    public bool AddCommand(RemoteCommand command)
    {
        if (!command.IsInitialized)
        {
            Debug.Log("command is not initialized.");
            return false;
        }

        if (!_commands.TryAdd(command.ID, command))
        {
            Debug.Log($"Conflict Command ID: {_commands[command.ID]}, {command}");
            return false;
        }

        return true;
    }

    #endregion Method
}}