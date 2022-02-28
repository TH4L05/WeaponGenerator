using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum MovementState
    {
        Default,
        HookShot,
    }

    [SerializeField] private CharacterData characterData;
    public CharacterController playerController;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private bool useCharacterController = false;
    [SerializeField] private Transform pivot;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private CapsuleCollider playerBodyCollider;

    private Vector2 axisInput;
    private Vector2 axisInputMouse;
    private float speed = 1f;
    private float lastspeed = 1f;
    private bool isGrounded;
    private bool jump;
    private bool sprint;
    private Vector3 groundVelocity = Vector3.zero;
    private Vector3 verticalVelocity = Vector3.zero;
    private Vector3 characterVelocity = Vector3.zero;
    private Vector3 lastmovement = Vector3.zero;
    private float xRotation = 0f;
    private bool hookshot;
    private MovementState movementState;

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        movementState = MovementState.Default;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void GetComponents()
    {
        playerController = GetComponent<CharacterController>();
        playerBody = GetComponent<Rigidbody>();
        if (playerController != null && !useCharacterController)
        {
            playerController.enabled = false;

        }
        else
        {
            
        }
    }

    private void Update()
    {
        CheckMovementState();
    }

    private void CheckMovementState()
    {
        switch (movementState)
        {
            case MovementState.Default:
            default:
                bool wasGrounded = isGrounded;
                GroundCheck();
                JumpOnInput();
                SprintOnInput();
                UpdateMovement();
                UpdateRotation();
                /*if (!useCharacterController && playerBody.velocity.y <0)
                {
                    playerBody.velocity += Vector3.up * Physics.gravity.y * ((characterData.jump_force + 0.5f) - 1) * Time.deltaTime;
                }*/
                break;

            case MovementState.HookShot:
                HookShootMovement();
                UpdateRotation();
                break;
        }
    }

    public void UpdateValues(Vector2 axisInput, Vector2 axisInputMouse, bool jumpPressed, bool sprintPressed)
    {
        this.axisInput = axisInput;
        this.axisInputMouse = axisInputMouse;
        jump = jumpPressed;
        sprint = sprintPressed;
    }

    private void UpdateMovement()
    {      
        Vector3 horizontalVelocity = transform.right * axisInput.x + transform.forward * axisInput.y;
      
        if (useCharacterController)
        {
            if (playerController.isGrounded)
            {
                
                characterVelocity = horizontalVelocity * speed;               
            }
            else
            {
                characterVelocity = groundVelocity;
                characterVelocity += horizontalVelocity * speed;
            }

            characterVelocity = Vector3.ClampMagnitude(characterVelocity, lastspeed);
            characterVelocity.y = verticalVelocity.y;
            characterVelocity *= Time.deltaTime;
            lastmovement = characterVelocity / Time.deltaTime;

            playerController.Move(characterVelocity);
        }
        else
        {
            if (playerBody.velocity.y == 0)
            {
                playerBody.velocity = horizontalVelocity + new Vector3(0, playerBody.velocity.y, 0);
            }           
        }
        characterData.position = characterVelocity;
    }

    private void UpdateRotation()
    {
        var rotations = axisInputMouse * sensitivity * Time.deltaTime;
        xRotation -= rotations.y;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        transform.Rotate(Vector3.up, rotations.x);
        pivot.eulerAngles = targetRotation;
        characterData.rotation = axisInputMouse;
    }

    private void JumpOnInput()
    {
        groundVelocity = lastmovement;

        if (jump)
        {
            
            if (useCharacterController)
            {
                if (playerController.isGrounded)
                {                   
                    verticalVelocity.y = Mathf.Sqrt(-2 * characterData.jump_force * -characterData.gravity);
                }
            }
            else
            {
                //var velocityXonJump = position.x;
                //var velocityZonJump = position.y;

                //playerBody.AddForce(new Vector3(velocityXonJump, characterData.jump_force, velocityZonJump),ForceMode.VelocityChange);
                playerBody.velocity += characterData.jump_force * Vector3.up;
            }
            
            jump = false;
        }
    }

    private void SprintOnInput()
    {
        if (sprint)
        {
            speed = characterData.sprint_speed;
        }
        else
        {
            speed = characterData.move_speed;
        }
    }

    private void GroundCheck()
    {

        if (useCharacterController)
        {
            if (playerController.isGrounded)
            {
                isGrounded = true;
                verticalVelocity.y = -1;
                lastspeed = characterData.sprint_speed;
            }
            else
            {
                isGrounded = false;
                verticalVelocity.y += -characterData.gravity * Time.deltaTime;
                lastspeed = speed;
            }
        }
        else
        {
            float offset = 0.01f;
            var origin = playerBodyCollider.center;
            var distance = playerBodyCollider.bounds.min.y + offset;
            RaycastHit hit;
            Color rayColor = Color.red;

            if (Physics.Raycast(origin, playerBodyCollider.transform.TransformDirection(Vector3.down), out hit ,distance))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    rayColor = Color.green;
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
            Debug.DrawRay(origin, Vector3.down * distance, rayColor);
        }
    }



    Vector3 direction;
    Transform hookNode;
    float hookshotSpeed;
    float distance;

    public void HookShotMove(Transform hookNode, float forcceMultiplier)
    {   
        this.hookNode = hookNode;
        direction = hookNode.position - transform.position;
        hookshotSpeed = forcceMultiplier;
        movementState = MovementState.HookShot;
    }

    private void HookShootMovement()
    {
        distance = Vector3.Distance(transform.position, hookNode.position);

        if (distance < 1f)
        {
            verticalVelocity.y = -1;
            movementState = MovementState.Default;
            Destroy(hookNode.gameObject);
            return;
        }

        playerController.Move(direction.normalized * (characterData.sprint_speed * 4) * Time.deltaTime);
    }
}
