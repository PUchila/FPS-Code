using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonScript : MonoBehaviour
{

    public StopWatch stopWatch;
    public bool start = true;
    public Animator anim;
    public TMP_Text e_text;
    private bool e_pressed = false;


    void Start(){
      e_text.gameObject.SetActive(false);
    }
    void OnTriggerStay(Collider col){
      if (col.gameObject.tag == "Player" && !e_pressed)
      {
        e_text.gameObject.SetActive(true);
        if (Input.GetKey(KeyCode.E))
        {
          e_pressed = true;
          if (start){
            stopWatch.StartTimer();
          }else{
            stopWatch.StopTimer();
          }
          anim.SetBool("TimerStart", true);

        }
      }
    }
    void OnTriggerExit(){
      e_text.gameObject.SetActive(false);
    }
}
