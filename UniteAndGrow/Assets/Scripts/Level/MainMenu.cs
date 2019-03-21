using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Update()
    {
        bool test = Input.GetButtonDown(Global.pauseButton) ||
            Input.GetButtonDown(Global.altPauseButton);
        if (test)
        {

            GameObject[] menuItems = GameObject.FindGameObjectsWithTag("Menu");
            foreach (GameObject menuItem in menuItems)
            {
                menuItem.SetActive(false);
            }
            gameObject.transform.Find("Main").gameObject.SetActive(true);
        }
    }

    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
