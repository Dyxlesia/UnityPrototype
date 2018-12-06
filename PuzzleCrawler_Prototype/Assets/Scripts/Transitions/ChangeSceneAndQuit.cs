using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAndQuit : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public void ChangeScene(string s)
    {
        SceneManager.LoadScene(s);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "RoomNodes")
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Title");
            }
        }

        

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
