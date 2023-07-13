using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuRef;
    
    private InputActions menuAction;
    
    void Awake()
    {
        menuAction = new InputActions();
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        PauseMenuRef.SetActive(false);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        menuAction.Enable();

        menuAction.Menu.Pause.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        menuAction.Disable();

        menuAction.Menu.Pause.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext value)
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Cursor.visible = Time.timeScale == 0 ? true : false;
        PauseMenuRef.SetActive(Time.timeScale == 0);
    }
}