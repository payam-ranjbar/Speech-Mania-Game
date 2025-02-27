using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private PlayerState requiredState;
    private PlayerStateController _stateController;
    void Start()
    {
        FindStateController();
    }

    void OnDestroy()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!StateControllerAssigned()) return;

        _stateController.ObstacleHit(requiredState);
        
        

    }

    private bool StateControllerAssigned()
    {

        var stateNotNull = _stateController is not null;
        var found = stateNotNull || FindStateController();
        if (!found)
        {
            Debug.Log("Obstacle Hit, but player controller not found"); 
        }

        return found;
    }

    private bool FindStateController()
    {
        if (_stateController is null)
        {
            _stateController = FindAnyObjectByType<PlayerStateController>();
            return true;
        }

        return false;
    }
}