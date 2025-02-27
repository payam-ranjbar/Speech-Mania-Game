using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent onStateChange;
    public static event Action<int> OnDash;
    public static event Action<PlayerState> OnObstacleHit;
    public static event Action<PlayerState> OnObstaclePassed;
    public static event Action<string> OnWordReceived;
    public static event Action<PlayerState> OnStateChange;

    public void InvokeDash(int direction) => OnDash?.Invoke(direction);
    public void InvokeObstacleHit(PlayerState state) => OnObstacleHit?.Invoke(state);
    public void InvokeObstaclePassed(PlayerState state) => OnObstaclePassed?.Invoke(state);
    public void InvokeWordReceived(string word) => OnWordReceived?.Invoke(word);
    public void InvokeOnStateChange(PlayerState state)
    {
        OnStateChange?.Invoke(state);
        onStateChange?.Invoke();
        
    }
    
    
}