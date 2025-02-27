using UnityEngine;

public class PlayerState : ScriptableObject
{
    public string stateName;

    [SerializeField] private string[] aliasNames;
    
    public PlayerAvatar playerAvatar;

    public bool HasName(string stateName)
    {
        var exists = this.stateName == stateName;

        if (exists) return true;

        if (aliasNames.Length <= 0) return false;
        
        foreach (var aliasName in aliasNames)
        {
            if (aliasName == stateName) return true;
        }
        return false;
    }
    public void ApplyState(PlayerMotor player)
    {
        playerAvatar.SetState(this);
    }
}