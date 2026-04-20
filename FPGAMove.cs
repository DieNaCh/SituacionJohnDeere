using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPGAController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float interactionRadius = 1.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        char incomingChar = SerialManager.Instance.LastReceivedChar;

        if (incomingChar != '\0')
        {
            if (incomingChar == 'e')
            {
                PerformInteraction();
            }
            else 
            {
                UpdateMovementDirection(incomingChar);
            }
        }

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * maxSpeed;
    }

    void UpdateMovementDirection(char direction)
    {
        switch (direction)
        {
            case 'w': moveInput = Vector2.up; break;
            case 's': moveInput = Vector2.down; break;
            case 'a': moveInput = Vector2.left; break;
            case 'd': moveInput = Vector2.right; break;
            case ' ': moveInput = Vector2.zero; break;
        }
    }

    void UpdateAnimator()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        if (isMoving)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    void PerformInteraction()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        foreach(var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                interactable.Interact();
                break; 
            }
        }
    }
}