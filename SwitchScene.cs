using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{

    public string scene;
    public string nextscene;
    void Start(){
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
    public void NextScene(){
      SceneManager.LoadScene(nextscene);
    }
    public void RestartScene(){
      SceneManager.LoadScene(scene);
    }
}
