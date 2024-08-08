using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject controlMenuUI;
    public PlayerCam playercam;

    // Start is called before the first frame update
    void Start()
    {
      GameIsPaused = false;
      Time.timeScale = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
          if(GameIsPaused){
            Resume();
          }else{
            Pause();
          }
        }
    }
    void Pause(){
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
      GameIsPaused = true;
    }
    public void Resume(){
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false;
    }
    public void LoadMenu(){
      SceneManager.LoadScene("Menu");
      Time.timeScale = 1f;
    }
    public void QuitGame(){
      Application.Quit();
    }
    public void Controls(){
      pauseMenuUI.SetActive(false);
      controlMenuUI.SetActive(true);
    }
    public void Back(){
      pauseMenuUI.SetActive(true);
      controlMenuUI.SetActive(false);
    }

    public void SetSensitivity(float sens){
      playercam.sensX = sens;
      playercam.sensY = sens;
    }
}
