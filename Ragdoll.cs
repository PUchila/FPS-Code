using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody[] ragdollBodies;

    void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    // Update is called once per frame

    private void DisableRagdoll(){
      foreach (var rigidbody in ragdollBodies){
        rigidbody.isKinematic = true;
      }
    }


    public void EnableRagdoll(){
      foreach (var rigidbody in ragdollBodies)
      {
        rigidbody.isKinematic = false;
      }
    }
    public void forceUp(float force){
      foreach (var rigidbody in ragdollBodies)
      {
        rigidbody.AddForce(transform.up * force, ForceMode.Impulse);
      }
    }
}
