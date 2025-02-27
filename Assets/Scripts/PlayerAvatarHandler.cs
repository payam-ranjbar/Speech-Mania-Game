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
        var created = Instantiate(avatar, avatarRoot);
        created.transform.localPosition = Vector3.zero;
        
        created.gameObject.name = $"new avatar {avatar.StateName}";
        
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

    private PlayerAvatar GetAvatarFromList(PlayerAvatar avatar)
    {
        return _avatarTable[avatar.StateName];
    }

    private void SetCurrent(PlayerAvatar avatar)
    {

        var avatarGO = GetAvatarFromList(avatar);
        if (_currentAvatar is null)
        {
            _currentAvatar = avatarGO;
            _currentAvatar.gameObject.SetActive(true);
            return;
        }
        
        _currentAvatar.gameObject.SetActive(false);
        _currentAvatar = avatarGO;
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
        Debug.LogWarning($"game object {_currentAvatar.gameObject.name} atcive is {_currentAvatar.gameObject.activeInHierarchy}");

    }
}