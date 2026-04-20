using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO.Ports;

public enum TractorGear
{
    Drive,
    Neutral,
    Reverse 
}

public class Tractor_Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float deacceleration = 4f;

    // Marchas
    public TractorGear currentGear = TractorGear.Neutral;
    [SerializeField] private float reverseSpeedMult = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Animator animator;

    private float currentGearMult = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Le pedimos al Manager la última letra que recibió la FPGA
        // Si no hay Manager en la escena, esto dará un error, asegúrate de tenerlo.
        char incomingChar = SerialManager.Instance.LastReceivedChar;

        // 2. Si el caracter no es nulo ('\0'), significa que llegó un comando
        if (incomingChar != '\0')
        {
            switch (incomingChar)
            {
                case 'w': moveInput = new Vector2(0, 1); break;
                case 's': moveInput = new Vector2(0, -1); break;
                case 'a': moveInput = new Vector2(-1, 0); break;
                case 'd': moveInput = new Vector2(1, 0); break;
                case ' ': 
                    moveInput = Vector2.zero; 
                    break; 
                case 'e': 
                    ToggleGear();
                    break;
            }
        }

        if (currentGear == TractorGear.Neutral) currentGearMult = 0f;
        else if (currentGear == TractorGear.Drive) currentGearMult = 1f;
        else if (currentGear == TractorGear.Reverse) currentGearMult = -reverseSpeedMult;

        bool isMoving = currentVelocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    void FixedUpdate()
    {
        Vector2 targetV = moveInput * maxSpeed * currentGearMult;
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
    }

    

    public void ChangeGear(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleGear();
        }
    }

    private void ToggleGear()
    {
        if (currentGear == TractorGear.Neutral) currentGear = TractorGear.Drive;
        else if (currentGear == TractorGear.Drive) currentGear = TractorGear.Reverse;
        else if (currentGear == TractorGear.Reverse) currentGear = TractorGear.Neutral;
        
        Debug.Log("Cambio de marcha a: " + currentGear);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed) InteractAction();
    }

    private void InteractAction()
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