using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene;
    
    public void StartGame()
    {
        StartCoroutine(DelayLoadScene(0.5f));
    }
    private IEnumerator DelayLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void OpenOption()
    {

    }
    public void CloseOptions()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
