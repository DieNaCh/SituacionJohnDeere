using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TractorGear
{
   Drive,
   Neutral,
   Reverse 
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float maxSpeed = 5f;
    [SerializeField]private float interactionRadius = 1.5f;
    [SerializeField]private float acceleration = 3f;
    [SerializeField]private float deacceleration = 4f;

    //Marchas
    public TractorGear currentGear = TractorGear.Neutral;
    [SerializeField] private float reverseSpeedMult = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log("Tractor encendido. Marcha actual: " + currentGear);
    }

    // Update is called once per frame
    void Update()
    {
        float gearMult = 0f;

        switch (currentGear)
        {
            case TractorGear.Drive:
                gearMult = 1f;
                break;
            case TractorGear.Reverse:
                gearMult = -reverseSpeedMult;
                break;
            case TractorGear.Neutral:
                gearMult = 0f;
                break;
        }
        Vector2 targetV = moveInput * maxSpeed * gearMult;
        float accelRate;
        if(moveInput.magnitude > 0 && currentGear != TractorGear.Neutral)
        {
            accelRate = acceleration;
        }
        else
        {
            accelRate = deacceleration;
        }

        currentVelocity = Vector2.MoveTowards(currentVelocity, targetV, accelRate * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;

        // 4. Actualizamos la animación basándonos en si el tractor realmente se está moviendo
        // Esto evita que las ruedas giren si aceleras estando en Neutral
        bool isMoving = currentVelocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
    }

    public void ChangeGear(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(currentGear == TractorGear.Neutral)
            {
                currentGear = TractorGear.Drive;
            }else if(currentGear == TractorGear.Drive)
            {
                currentGear = TractorGear.Reverse;
            }else if(currentGear == TractorGear.Reverse)
            {
                currentGear = TractorGear.Neutral;
            }
            Debug.Log("Cambio de marcha a: " + currentGear);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        if(moveInput != Vector2.zero)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
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
