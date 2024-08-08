using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float swingSpeed;
    //private float drag;
    public float drag;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    public float deathYVal;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sliding,
        swinging,
        air
    }

    public bool sliding;
    public bool swinging;
    public bool bounced = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        if (bounced){
          sliding = false;
          rb.velocity = new Vector3(1f, rb.velocity.y, 1f);
          bounced = false;
        }
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        //    drag = 0;
        else
            rb.drag = 0;
        //    drag =


        //reload on death
        if (transform.position.y<deathYVal){
          Die();
        }
        if (Input.GetKeyDown(KeyCode.J)){
          Die();
        }
    }

    private void FixedUpdate()
    {
        SpeedControl();
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }


    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }
        else if (swinging)
        {
          state = MovementState.swinging;
          desiredMoveSpeed = swingSpeed;
        }



        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if (!swinging){
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
      } else{
        moveSpeed = desiredMoveSpeed;
      }
        lastDesiredMoveSpeed = desiredMoveSpeed;

    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (swinging) return;
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        if (horizontalInput > 0 && xMag > moveSpeed) horizontalInput = 0;
        if (horizontalInput < 0 && xMag < -moveSpeed) horizontalInput = 0;
        if (verticalInput > 0 && yMag > moveSpeed) verticalInput = 0;
        if (verticalInput < 0 && yMag < -moveSpeed) verticalInput = 0;
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            //if (flatVel.magnitude > moveSpeed)
            //{
            //    Vector3 vel = rb.velocity;
            //    vel.x *= (1.0f - (drag * Time.deltaTime));
            //    vel.z *= (1.0f - (drag * Time.deltaTime));
            //    //Vector3 limitedVel = flatVel.normalized * moveSpeed;
                //rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            //    rb.velocity = vel;
            //}
        }
    }
    public Vector2 FindVelRelativeToLook() {
       float lookAngle = orientation.transform.eulerAngles.y;
       float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

       float u = Mathf.DeltaAngle(lookAngle, moveAngle);
       float v = 90 - u;

       float magnitue = rb.velocity.magnitude;
       float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
       float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

       return new Vector2(xMag, yMag);
   }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    private void Die(){
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnCollisionEnter(Collision col){
      if (col.gameObject.tag == "Bullet" || col.gameObject.tag == "Lava"){
        Die();
      }
    }


}
