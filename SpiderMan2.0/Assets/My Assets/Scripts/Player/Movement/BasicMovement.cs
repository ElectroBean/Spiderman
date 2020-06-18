using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    public float moveSpeed;
    public float maxSpeed;
    public float currentSpeed;
    public float maxVelocityChange = 10.0f;
    CharacterController cc;

    [Header("Jump variables")]
    //jump variables
    public float jumpForce;
    public float gravForce;
    private bool jump = false;
    private bool inAir = false;

    [Header("Ground checks"), Space(2.0f)]
    //ground check
    public bool grounded;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public float usedGravity = -9.8f;

    // using variables
    float vertical;
    float horizontal;
    Vector3 moveDir;
    Rigidbody rb;

    public enum LocoState
    {
        walking,
        falling,
        swinging
    }
    [Header("States")]
    public LocoState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetermineState();

        grounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        #region old
        //if (vertical != 0 || horizontal != 0)
        //{
        //    Vector3 forward = Camera.main.transform.forward * vertical;
        //    Vector3 right = Camera.main.transform.right * horizontal;
        //    forward.y = 0;
        //    right.y = 0;
        //    Vector3 movementVector = (forward + right);
        //    movementVector *= moveSpeed;
        //    //movementVector.y = rb.velocity.y;
        //    //rb.velocity = movementVector;
        //    //rb.AddForce(movementVector, ForceMode.Acceleration);
        //    //if (rb.velocity.magnitude > maxSpeed)
        //    //{
        //    //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        //    //}
        //    //rb.MovePosition(transform.position + Time.deltaTime * currentSpeed * transform.TransformDirection(movementVector));
        //    LookInDirectionOfMovement();
        //}
        //if (jump)
        //{
        //    //rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        //    inAir = true;
        //    jump = false;
        //}
        #endregion
        switch (currentState)
        {
            case LocoState.walking:
                {
                    Walk();
                }
                break;
            case LocoState.falling:
                {
                    Fall();
                }
                break;
            case LocoState.swinging:
                {

                }
                break;
        }
    }

    bool IsGrounded()
    {
        Collider[] overlaps = Physics.OverlapSphere(groundCheck.position, 0.15f);
        if (overlaps.Length > 0)
        {
            foreach (Collider cl in overlaps)
            {
                if (MyLibrary.IsLayerInMask(groundLayerMask, cl.gameObject.layer))
                {
                    usedGravity = Physics.gravity.y;
                    return true;
                }
            }
        }
        return false;
    }

    void LookInDirectionOfMovement()
    {
        #region old
        Vector3 euler = rb.velocity;
        euler.y = 0;
        Quaternion desiredRot = Quaternion.LookRotation(euler);
        if (euler == Vector3.zero)
            return;
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, 0.2f);
        #endregion
    }

    void Walk()
    {
        #region old
        //if (cc.isGrounded)
        //{
        //    moveDir = new Vector3(Input.GetAxis("HorizontalMovement"), 0, Input.GetAxis("VerticalMovement"));
        //    moveDir = Camera.main.transform.TransformDirection(moveDir);
        //    moveDir.y = 0.0f;
        //    moveDir *= moveSpeed;

        //    if (Input.GetButton("Jump"))
        //    {
        //        moveDir.y = jumpForce;
        //    }
        //}
        //moveDir.y -= 9.81f * Time.deltaTime;
        //cc.Move(moveDir * Time.deltaTime);

        //LookInDirectionOfMovement();
        #endregion
        if (grounded)
        {
            DirectionalMovement();
            if (Input.GetAxis("Jump") > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }

        //rb.AddForce(new Vector3(0, -Physics.gravity.y * rb.mass, 0));
        LookInDirectionOfMovement();
    }

    void DirectionalMovement()
    {
        Vector3 forward = Camera.main.transform.forward * Input.GetAxis("VerticalMovement");
        Vector3 right = Camera.main.transform.right * Input.GetAxis("HorizontalMovement");
        forward.y = 0;
        right.y = 0;
        Vector3 movementVector = (forward + right);
        var targetVelocity = movementVector;
        //targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= moveSpeed;

        var velocity = rb.velocity;
        var velocityChange = (targetVelocity - velocity);
        //velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        //velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Swing()
    {
        
    }

    void Fall()
    {
        #region old
        //moveDir = new Vector3(Input.GetAxis("HorizontalMovement"), 0, Input.GetAxis("VerticalMovement"));
        //moveDir = Camera.main.transform.TransformDirection(moveDir);
        //moveDir *= moveSpeed;
        //moveDir.y -= 9.81f;
        #endregion
        DirectionalMovement();
        usedGravity -= 19.6f;
        rb.AddForce(new Vector3(0, usedGravity, 0));
        LookInDirectionOfMovement();
    }

    void DetermineState()
    {
        if(grounded)
        {
            currentState = LocoState.walking;
        }
        else
        {
            currentState = LocoState.falling;
        }
    }
}
