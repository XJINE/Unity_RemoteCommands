using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RemoteCommands {
public class RemoteCommander : SingletonMonoBehaviour<RemoteCommander>, IInitializable
{
    // NOTE:
    // An int-type hash may cause conflicts.
    // However, if the number of registered commands is 1000 or less,
    // the risk of conflict is quite low.
    // If a conflict occurs, change the ID to resolve it.

    private Dictionary<string, RemoteCommandAttribute> _commands;
    private Dictionary<   int,                 string> _commandHashes;

    public bool IsInitialized { get; protected set; }

    public ReadOnlyDictionary<string, RemoteCommandAttribute> Commands      { get; private set; }
    public ReadOnlyDictionary<   int,                 string> CommandHashes { get; private set; }

    protected void Start()
    {
        // CAUTION:
        // To scan other objects, Initialize shouldn't be called in Awake.
        Initialize();
    }

    public bool Initialize()
    {
        if (IsInitialized)
        {
            return false;
        }

        IsInitialized = true;

        _commands = new Dictionary        <string, RemoteCommandAttribute>();
        Commands  = new ReadOnlyDictionary<string, RemoteCommandAttribute>(_commands);

        _commandHashes = new Dictionary        <int, string>();
        CommandHashes  = new ReadOnlyDictionary<int, string>(_commandHashes);

        foreach (var monoBehaviour in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        { 
            var type = monoBehaviour.GetType();

            while (type != null && type != typeof(MonoBehaviour))
            {
                RegisterCommands(monoBehaviour, type);
                type = type.BaseType;
            }
        }

        return true;
    }

    private void RegisterCommands(MonoBehaviour instance, IReflect type)
    {
        // NOTE:
        // DeclaredOnly needs to avoid conflict in inherit class.

        var memberInfos = type.GetMembers(BindingFlags.Public
                                        | BindingFlags.NonPublic
                                        | BindingFlags.Instance
                                        | BindingFlags.Static
                                        | BindingFlags.DeclaredOnly);

        foreach (var memberInfo in memberInfos)
        {
            if (Attribute.GetCustomAttribute(memberInfo, typeof(RemoteCommandAttribute)) is not RemoteCommandAttribute remoteCommand)
            {
                continue;
            }

            remoteCommand.Initialize(instance, memberInfo);

            RegisterCommand(remoteCommand);
        }
    }

    public bool RegisterCommand(RemoteCommandAttribute command)
    {
        if (!command.IsInitialized)
        {
            Debug.Log($"{command.ID} is not initialized.");
            return false;
        }

        if (!_commands.TryAdd(command.ID, command))
        {
            Debug.Log($"Conflict Command ID: {command.ID}");
            return false;
        }

        if (!_commandHashes.TryAdd(command.Hash, command.ID))
        {
            Debug.Log($"Conflict Command Hash: {command.ID}:{command.Hash}");
            _commands.Remove(command.ID);
            return false;
        }

        return true;
    }

    public object Command(params object[] parameters)
    {
        return Command(parameters[0].ToString(), parameters.Skip(1).ToArray());
    }

    public object Command(string id, params object[] parameters)
    {
        if (!_commands.TryGetValue(id, out var command))
        {
            Debug.Log($"Command ID '{id}' is not found.");
            return null;
        }
        try
        {
            return command.Invoke(parameters);
        }
        catch (Exception exception)
        {
            Debug.Log($"Invoke '{command}' is failed.\n{exception}");
            return null;
        }
    }

    public object Command(int hash, params object[] parameters)
    {
        if (_commandHashes.TryGetValue(hash, out var id))
        {
            return Command(id, parameters);
        }

        Debug.Log($"Command Hash '{hash}' is not found.");

        return null;
    }
}}