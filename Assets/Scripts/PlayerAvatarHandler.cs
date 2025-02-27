using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarHandler : MonoBehaviour
{
    [SerializeField] private Transform avatarRoot;
    private Dictionary<string, PlayerAvatar> _avatarTable;
    private PlayerAvatar _currentAvatar;
    [SerializeField]  private PlayerEventHandler eventHandler;

    private void Awake()
    {
        _avatarTable = new Dictionary<string, PlayerAvatar>();
    }

    private void InstantiateAvatar(PlayerAvatar avatar)
    {
        var created = Instantiate(avatar, Vector3.zero, Quaternion.identity);
        created.transform.parent = avatarRoot;
        created.transform.position = Vector3.zero;
        created.transform.rotation = Quaternion.identity;
        
        AddAvatar(created);
        SetCurrent(created);
    }

    private bool AvatarExists(PlayerAvatar avatar)
    {
        var avatarStateName = avatar.StateName;

        var found = _avatarTable.ContainsKey(avatarStateName);

        return found;
    }

    private bool AddAvatar(PlayerAvatar avatar)
    {
        if (AvatarExists(avatar)) return false;
        
        _avatarTable.Add(avatar.StateName, avatar);

        return true;
    }

    private void SetCurrent(PlayerAvatar avatar)
    {
        if (_currentAvatar is null)
        {
            _currentAvatar = avatar;
        }

        avatar.gameObject.SetActive(true);
        _currentAvatar.gameObject.SetActive(false);
        _currentAvatar = avatar;
        _currentAvatar.gameObject.SetActive(true);

    }
    private bool CurrentlyActive(PlayerAvatar avatar) => _currentAvatar.StateName == avatar.StateName;
    
    private void ActivateAvatar(PlayerAvatar avatar)
    {
        if (!AvatarExists(avatar))
        {
            InstantiateAvatar(avatar);
            return;
        }
        if (CurrentlyActive(avatar))
        {
            Debug.Log($"Setting avatar {avatar.StateName} failed, already active");
            return;
        }

        SetCurrent(avatar);
    }

    public void ActivateAvatar(PlayerState newState)
    {
        eventHandler.InvokeOnStateChange(newState);
        ActivateAvatar(newState.playerAvatar);
    }
}