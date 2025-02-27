using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarHandler : MonoBehaviour
{
    [SerializeField] private Transform avatarRoot;
    private Dictionary<string, PlayerAvatar> _avatarTable;
    private PlayerAvatar _currentAvatar;
    private PlayerEventHandler _eventHandler;

    private void InstantiateAvatar(PlayerAvatar avatar)
    {
        var created = Instantiate(avatar, avatarRoot);
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
        _currentAvatar.gameObject.SetActive(false);
        _currentAvatar = avatar;
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
        _eventHandler.InvokeOnStateChange(newState);
        ActivateAvatar(newState.playerAvatar);
    }
}