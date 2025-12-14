using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.18f;
    public float mouseSensitivity = 3f;

    float xRotation = 0f;

    public float sprintMultiplier = 1.5f;

    CharacterController controller;
    Transform cam;
    Vector3 velocity;
    bool isGrounded;

    PlayerStats stats;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        stats = GetComponent<PlayerStats>();

        if (cam == null)
            cam = GetComponentInChildren<Camera>()?.transform;
    }

    void Update()
    {
        HandleMove();
        HandleLook();
    }

    void HandleMove()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool canRun = stats.stamina > 0 && v > 0;

        float speed = moveSpeed;
        if (wantsToRun && canRun)
        {
            speed *= sprintMultiplier;
            stats.isRunning = true;
        }
        else
        {
            stats.isRunning = false;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (cam != null)
        {
            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
