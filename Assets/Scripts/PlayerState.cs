using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState-", order = 0, menuName = "Game Properties/Player State")]
public class PlayerState : ScriptableObject
{
    public string stateName;

    [SerializeField] private string[] aliasNames;
    
    public PlayerAvatar playerAvatar;

    public bool HasName(string stateName)
    {
        var exists = String.Equals(this.stateName, stateName, StringComparison.CurrentCultureIgnoreCase);

        if (exists) return true;

        if (aliasNames.Length <= 0) return false;
        
        foreach (var aliasName in aliasNames)
        {
            if (String.Equals(aliasName, stateName, StringComparison.CurrentCultureIgnoreCase)) return true;
        }
        return false;
    }
    public void ApplyState(PlayerMotor player)
    {
        playerAvatar.SetState(this);
    }
}