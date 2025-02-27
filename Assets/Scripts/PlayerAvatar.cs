using UnityEngine;

public class PlayerAvatar: MonoBehaviour
{
        [SerializeField] private string stateName;
        
        public string StateName => stateName;

        public void SetState(PlayerState playerState)
        {
                stateName = playerState.stateName;
        }
}