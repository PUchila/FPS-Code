using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class StopWatch : MonoBehaviour
{
    public bool timerActive;
    private float currentTime;
    public TMP_Text text;
    public static float score;
    public string nextScene;

    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
          currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        text.text = time.Minutes.ToString()+":" + time.Seconds.ToString()+":"+time.Milliseconds.ToString();
    }
    public void StartTimer(){
      timerActive = true;
    }
    public void StopTimer(){
      if (timerActive){
      timerActive = false;
      TimeSpan time = TimeSpan.FromSeconds(currentTime);
      score = currentTime;
      if (!PlayerPrefs.HasKey("highScoreGameSave")){
        PlayerPrefs.SetFloat("highScoreGameSave", score);
      }else if (!PlayerPrefs.HasKey("highScoreGameSave2")){
        PlayerPrefs.SetFloat("highScoreGameSave2", score);
      }else if (!PlayerPrefs.HasKey("highScoreGameSave3")){
        PlayerPrefs.SetFloat("highScoreGameSave3", score);

      }
      else if (score < PlayerPrefs.GetFloat("highScoreGameSave")){
        PlayerPrefs.SetFloat("highScoreGameSave", score);
      }
      else if (score < PlayerPrefs.GetFloat("highScoreGameSave2")){
        PlayerPrefs.SetFloat("highScoreGameSave2", score);
      }
      else if (score < PlayerPrefs.GetFloat("highScoreGameSave3")){
        PlayerPrefs.SetFloat("highScoreGameSave3", score);
      }
      SceneManager.LoadScene(nextScene);
    }else{
      SceneManager.LoadScene(nextScene);
    }
  }
}
