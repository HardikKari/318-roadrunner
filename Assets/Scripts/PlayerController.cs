using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 8f;

    [Header("Lane Movement")]
    [SerializeField] private float laneWidth = 3f;
    [SerializeField] private float laneChangeSpeed = 12f;

    [Header("Jumping")]
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private int currentLane = 1;
    private float verticalVelocity;

    private bool isGameOver;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        ReadLaneInput();
        HandleJumpAndGravity();
        MovePlayer();
    }

    private void ReadLaneInput()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentLane -= 1;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentLane += 1;
        }

        currentLane = Mathf.Clamp(currentLane, 0, 2);
    }

    private void HandleJumpAndGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            controller.isGrounded)
        {
            verticalVelocity = jumpSpeed;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void MovePlayer()
    {
        float targetX = (currentLane - 1) * laneWidth;

        float nextX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        Vector3 movement = new Vector3(
            nextX - transform.position.x,
            verticalVelocity * Time.deltaTime,
            forwardSpeed * Time.deltaTime
        );

        controller.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
{
    if (hit.gameObject.CompareTag("Obstacle"))
    {
        isGameOver = true;
        Debug.Log("Game Over");
    }
}
}

