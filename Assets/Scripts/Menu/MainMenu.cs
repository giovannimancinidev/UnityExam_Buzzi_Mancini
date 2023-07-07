using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleRef;
    public GameObject CommandsRef;

    private AsyncOperation asyncOperation;

    public void OnCommandsActive()
    {
        TitleRef.SetActive(false);
        CommandsRef.SetActive(true);

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;
    }

    public void OnStartGame()
    {
        asyncOperation.allowSceneActivation = true;
    }

    public void OnQuitGame()
    {
        Application.Quit(); 
    }
}
