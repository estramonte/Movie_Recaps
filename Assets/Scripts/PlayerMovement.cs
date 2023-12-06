using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    //Essentials
    CharacterController controller;
    public Transform cam;

    //Movement
    public float moveSpeed = 5f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity; 

    //Jumping
    public float jumpHeight;
    public float gravity;
    private bool isGrounded = true;
    Vector3 velocity;

    //animation
    private Animator anim;

    //Navigation to couch
    public Transform couchTransform;
    public Vector3 targetDirection;
    private bool hasSat = false;
    private NavMeshAgent navMeshAgent;

    //TV Camera transition
    public CinemachineFreeLook cinemachineFreeLook;
    public CameraMove cameraMove;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(hasSat)
        {
            navMeshAgent.SetDestination(couchTransform.position);
            //sets the animation to walking
            anim.SetFloat("speed", 1);
            //makes sure the animation doesn't move the character
            anim.transform.localPosition = Vector3.zero;
            anim.transform.localEulerAngles = Vector3.zero;

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Player has reached the couch, rotate it and then make sure it doesn't move anymore
                anim.SetFloat("speed", 0);
                anim.SetTrigger("Sit");
                // targetDirection = (couchTransform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                cinemachineFreeLook.enabled = false;
                StartCoroutine(cameraMove.StartCameraTransition());
                this.enabled = false;
            }
        }
        else
        {
            isGrounded = Physics.CheckSphere(transform.position, 0.1f, 1);
            // //for the jumping animation

            //to make sure we stay on the ground
            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -1;
            }

            //stop the animation from moving the position of the character
            anim.transform.localPosition = Vector3.zero;
            anim.transform.localEulerAngles = Vector3.zero;

            //get the inputs (WASD)
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            //create a vector based on those inputs
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float moveMagnitude = direction.magnitude;

            //if we're moving we move 
            if (moveMagnitude >= 0.1f)
            {
                anim.SetFloat("speed", 1);
                //this makes sure we're moving where our head is facing
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
            }

            if(moveMagnitude < 0.1f)
            {
                anim.SetFloat("speed",0);
            }

            //Jumping
            //We need height so we have to jump, however the jump mechanic takes awhile so it doesn't look
            //good if I press the space bar and the animation is delayed
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                anim.SetTrigger("Jump");
            }
            
            if(velocity.y > -20)
            {
                velocity.y += (gravity*10) * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);

            if(Input.GetKeyDown(KeyCode.Slash))
            {
                hasSat = true;
            }
        }
    }

}