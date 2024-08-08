using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public PlayerMovement pm;
    public Shooting shoot;
    public bool canaim;

    [Header("AirMove")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Settings")]
    public float spring = 4.5f;
    public float damping = 7f;
    public float massScale = 4.5f;



    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
      if (Input.GetMouseButtonDown(1)){
        StartGrapple();
      } else if(Input.GetMouseButtonUp(1)){
        StopGrapple();
      }
      if (joint != null) AirMovement();



    }
    void LateUpdate(){
      DrawRope();
    }

    void StartGrapple(){
      RaycastHit hit;
      if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)){
        pm.swinging = true;
        grapplePoint = hit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        //change
        joint.spring = spring;
        joint.damper = damping;
        joint.massScale = massScale;
        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;

        canaim = false;

      }else{
        canaim = true;
      }
    }

    void StopGrapple(){
      pm.swinging = false;
      lr.positionCount = 0;
      Destroy(joint);
    }
    private Vector3 currentGrapplePosition;

    void DrawRope(){
      if (!joint) return;
      currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
      lr.SetPosition(0,gunTip.position);
      lr.SetPosition(1,currentGrapplePosition);
    }

    private void AirMovement(){
      if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
      if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
      if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
      if (Input.GetKey(KeyCode.S)) rb.AddForce(-orientation.forward * forwardThrustForce * Time.deltaTime);


    }
}
