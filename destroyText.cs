using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyText : MonoBehaviour
{

    public float targetDamage;

    // Update is called once per frame
    void Update()
    {
        if(targetDamage == 0){
          Destroy(gameObject);
        }
    }
}
