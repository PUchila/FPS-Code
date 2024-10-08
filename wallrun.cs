using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallrun : MonoBehaviour
{
    public Transform orientation;

    [Header("Wall Running")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov = 60;
    [SerializeField] private float wallRunFov = 110;
    [SerializeField] private float wallRunFovTime = 20;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt {get; private set; }


    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    private void Start()
    {
      rb = GetComponent<Rigidbody>();
    }



    bool CanWallRun()
    {
      return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
      wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
      wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);

    }
    void Update()
    {
        CheckWall();
        if(CanWallRun())
        {
          if(wallLeft){
            StartWallRun();
          }
          else if(wallRight)
          {
          StartWallRun();
          }
          else
          {
            StopWallRun();
          }
        }else{
          StopWallRun();
        }
    }

    void StartWallRun()
    {
      rb.useGravity = false;
      rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
      cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

      if (wallLeft)
        tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
      else if (wallRight)
      tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);



      if (Input.GetKeyDown(KeyCode.Space))
      {
        if (wallLeft)
        {
          Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
          rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
          rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
        } else if (wallRight)
        {
          Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
          rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
          rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
        }
      }
    }

    void StopWallRun()
    {
      rb.useGravity = true;
      cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
      tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

    }
}
