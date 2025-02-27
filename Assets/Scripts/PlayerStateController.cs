using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] PlayerState [] availableStates;
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private PlayerAvatarHandler avatarHandler;
    [SerializeField] private PlayerEventHandler eventHandler;

    private PlayerState _currentState;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void OnEnable()
    {
        SubscribeToSocketServer();
    }

    private void OnDisable()
    {
        UnSubscribeToSocketServer();
    }

    private void SubscribeToSocketServer()
    {
        WebsocketClient.Instance.OnWordReceived += OnWordReceived;
    }

    private void UnSubscribeToSocketServer()
    {
        WebsocketClient.Instance.OnWordReceived -= OnWordReceived;
    }

    private bool StateExists(string stateName, out PlayerState foundState)
    {
        foundState = null;
        foreach (var state in availableStates)
        {
            if (!state.HasName(stateName)) continue;
            
            foundState = state;
            return true;
        }

        return false;
    }
    public void OnWordReceived(string word)
    {
        var exists = StateExists(word, out var state);
        if(!exists) return;
        ChangeState(state);
    }

    public void ChangeState(PlayerState newState)
    {
        if(IsStateEqual(newState)) return;
        
        _currentState = newState;
        _currentState.ApplyState(playerMotor);
        avatarHandler.ActivateAvatar(newState);
    }

    private void ChangeState(string stateName)
    {
        if(StateExists(stateName, out var state))
        {
            ChangeState(state);
        }
        
    }

    private bool IsStateEqual(PlayerState state)
    {
        return _currentState.stateName == state.stateName;

    }
    public void ObstacleHit(PlayerState requiredState)
    {
        var pass = IsStateEqual(requiredState);
        if (pass)
        {
            eventHandler.InvokeObstaclePassed(requiredState);
            return;
        } 
        eventHandler.InvokeObstacleHit(requiredState);
        
    }
}