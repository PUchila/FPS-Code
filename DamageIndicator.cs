using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public bool attackIndicator = false;
    public float speed = 10f;
    public Vector3 maxScale;
    public Vector3 minScale;
    private Vector3 desiredScale;


    void Start(){
      foreach (Transform reticle in transform){
        reticle.gameObject.SetActive(false);
      }
      transform.localScale = minScale;
    }


    // Update is called once per frame
    void Update()
    {
        if(attackIndicator){
          Activation();
        }
        if((maxScale.x - transform.localScale.x) <= .001f){
          Deactivation();
        }
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, speed*Time.deltaTime);
    }
    private void Activation(){
      foreach(Transform reticle in transform){
        reticle.gameObject.SetActive(true);
      }
      desiredScale = maxScale;
    }
    private void Deactivation(){
      transform.localScale = minScale;
      desiredScale = minScale;
      foreach(Transform reticle in transform){
        reticle.gameObject.SetActive(false);
      }
      attackIndicator = false;
    }
}
