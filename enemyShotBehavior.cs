using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShotBehavior : MonoBehaviour
{

    private void OnCollisionStay(Collision col){
      if (col.transform.root.gameObject.tag == "Environment"){
        Destroy(gameObject);
      }
    }
}
