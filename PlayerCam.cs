using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 400;
    public float sensY = 400;
    public Transform orientation;
    public wallrun wallrun;


    public GameObject pauseMenu;
    public GameObject controlMenu;
    float xRotation;
    public float yRotation = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (pauseMenu.activeInHierarchy || controlMenu.activeInHierarchy){
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
          return;
        }else{
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation +=mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, wallrun.tilt);
        orientation.rotation = Quaternion.Euler(0,yRotation, 0);
    }
}
