using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public string scene;

    // Start is called before the first frame update
    void Start()
    {
      pauseMenuUI.SetActive(true);

    }

    // Update is called once per frame

    public void Play(){
      SceneManager.LoadScene(scene);
    }
    public void QuitGame(){
      Application.Quit();
    }

}
