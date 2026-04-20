using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField]private float interactionRadius = 1.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Movimiento directo
        rb.linearVelocity = moveInput * maxSpeed;

        // Animación
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
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
}