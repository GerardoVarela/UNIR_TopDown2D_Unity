using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference pause;

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject askQuitGameUI;
    private bool _isPaused = false;

    [Header("Event System")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectedGameSelector;

    private void OnEnable()
    {
        pause.action.Enable();
        pause.action.performed += OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if(askQuitGameUI.activeSelf) return;
        
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        _isPaused = false;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        _isPaused = true;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(firstSelectedGameSelector);
    }

    private void OnDisable()
    {
        pause.action.performed -= OnPause;
        pause.action.Disable();
    }
}