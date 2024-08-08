using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class StopWatchBoss : MonoBehaviour
{
    public bool timerActive = false;
    public bool timerActiveTimer = true;
    private float currentTime;
    private float currentTimer;
    public TMP_Text text;
    public TMP_Text timertextval;
    public static float score;
    public string nextScene;
    public string diescene;
    public float minutes;

    void Start()
    {
        currentTime = 0;
        currentTimer = minutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
          currentTime = currentTime + Time.deltaTime;
        }
        if (timerActiveTimer){
          currentTimer = currentTimer - Time.deltaTime;
          if(currentTimer<=0){
            SceneManager.LoadScene(diescene);
          }

        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        TimeSpan timertext = TimeSpan.FromSeconds(currentTimer);
        timertextval.text = timertext.Minutes.ToString()+":" + timertext.Seconds.ToString()+":"+timertext.Milliseconds.ToString();

        text.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();
    }

    public void StartTimer(){
      timerActive = true;
    }
    public void StopTimer(){
      timerActive = false;
      TimeSpan time = TimeSpan.FromSeconds(currentTime);
      score = minutes*60 - currentTimer;
      if (!PlayerPrefs.HasKey("highScoreBossGameSave")){
        PlayerPrefs.SetFloat("highScoreBossGameSave", score);
      }else if (!PlayerPrefs.HasKey("highScoreBossGameSave2")){
        PlayerPrefs.SetFloat("highScoreBossGameSave2", score);
      }else if (!PlayerPrefs.HasKey("highScoreBossGameSave3")){
        PlayerPrefs.SetFloat("highScoreBossGameSave3", score);

      }
      else if (score < PlayerPrefs.GetFloat("highScoreBossGameSave")){
        PlayerPrefs.SetFloat("highScoreBossGameSave", score);
      }
      else if (score < PlayerPrefs.GetFloat("highScoreBossGameSave2")){
        PlayerPrefs.SetFloat("highScoreBossGameSave2", score);
      }
      else if (score < PlayerPrefs.GetFloat("highScoreBossGameSave3")){
        PlayerPrefs.SetFloat("highScoreBossGameSave3", score);
      }
      SceneManager.LoadScene(nextScene);
    }
}
