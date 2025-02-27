using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float dashDistance = 2f;
    private int lane = 1; // 0 = left, 1 = middle, 2 = right
    private CharacterController controller;
    [SerializeField] private PlayerEventHandler playerEventHandler;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = Vector3.forward * forwardSpeed * Time.deltaTime;
        controller.Move(move);
    }

    public void Dash(int direction)
    {
        int targetLane = Mathf.Clamp(lane + direction, 0, 2);
        if (targetLane != lane)
        {
            lane = targetLane;
            Vector3 newPosition = transform.position + new Vector3(direction * dashDistance, 0, 0);
            transform.position = newPosition;

            playerEventHandler.InvokeDash(direction);
        }
    }
}