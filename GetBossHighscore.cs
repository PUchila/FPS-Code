using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class GetBosshighScoreBoss : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    private float currentTime;
    private float currentTime2 = 0;
    private float currentTime3 = 0;
    private TimeSpan time;
    private TimeSpan time2;
    private TimeSpan time3;


    void Start(){
      currentTime = PlayerPrefs.GetFloat("highScoreBossGameSave");
      time = TimeSpan.FromSeconds(currentTime);
      currentTime2 = PlayerPrefs.GetFloat("highScoreBossGameSave2", 9999999);
      time2 = TimeSpan.FromSeconds(currentTime2);
      text2.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
      currentTime3 = PlayerPrefs.GetFloat("highScoreBossGameSave3", 9999999);
      time3 = TimeSpan.FromSeconds(currentTime3);
      text3.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();
      if(currentTime<currentTime2 && currentTime<currentTime3){
        text1.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();
        if (currentTime2<currentTime3){
          text2.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
          text3.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();
        }else{
          text3.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
          text2.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();

        }
      }else if (currentTime2<currentTime && currentTime2<currentTime3){
        text1.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
        if (currentTime<currentTime3){
          text2.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();
          text3.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();
        }else{
          text2.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();
          text3.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();

        }
      }else if (currentTime3<currentTime && currentTime3<currentTime2){
        text1.text = time3.Minutes.ToString()+":" + time3.Seconds.ToString()+":"+time3.Milliseconds.ToString();
        if (currentTime<currentTime2){
          text2.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();
          text3.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
        }else{
          text2.text = time2.Minutes.ToString()+":" + time2.Seconds.ToString()+":"+time2.Milliseconds.ToString();
          text3.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();

        }
      }

      if (currentTime2 == 9999999){
        text2.text = "NaN";
      }else if (currentTime3 == 9999999){
        text3.text = "NaN";
      }



    }
}
