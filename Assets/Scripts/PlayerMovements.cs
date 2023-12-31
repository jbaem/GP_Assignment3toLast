using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    
    public float speed;
    public float rotationSpeed;
    public float dashDistance;

    private Vector2 movementValue;
    private float lookValue;
    private float speedUpValue = 1.0f;
    private bool powerUpValue = false;

    private Rigidbody rb;
    public ParticleSystem speedUpEffect;
    public AudioSource speedUpSound;

    public Animator animator;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
    }
    
    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
        animator.Play("player_movement");
    }

    public void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>().x * rotationSpeed;
    }
    
    public void OnSpeedUp(InputValue value)
    {
        if (value.isPressed)
        {
            speedUpValue = 2;
            if (!speedUpEffect.isPlaying)
            {
                speedUpEffect.Play();
                speedUpSound.Play();
            }
        }
        else
        {
            speedUpValue = 1;
            if (speedUpEffect.isPlaying)
            {
                speedUpEffect.Stop();
                speedUpSound.Stop();
            }
        }
    }
    public void OnPowerUp(InputValue value)
    {
        if(value.isPressed) { powerUpValue = true; }
        else { powerUpValue = false; }
    }

    void Update()
    {
        float teleport = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

        transform.Translate(
            movementValue.x * (Time.deltaTime * (powerUpValue ? 0.5f : speedUpValue) + dashDistance * teleport),
            0,
            movementValue.y * (Time.deltaTime * (powerUpValue ? 0.5f : speedUpValue) + dashDistance * teleport));
        transform.Rotate(0, lookValue * Time.deltaTime, 0);

        rb.AddRelativeForce(
            movementValue.x * Time.deltaTime,
            0,
            movementValue.y * Time.deltaTime);
        rb.AddRelativeTorque(0, lookValue * Time.deltaTime, 0);

        if(movementValue.x == 0 && movementValue.y == 0)
        {
            animator.Play("player_idle");
        }

    }
}
