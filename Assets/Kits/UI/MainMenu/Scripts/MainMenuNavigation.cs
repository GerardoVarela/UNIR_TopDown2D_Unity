using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuNavigation : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference start;

    [Header("UI Animators")]
    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Animator gameSelectorAnimator;

    [Header("Event System")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectedGameSelector;
    
    private bool _pressedStart = false;
    
    private void OnEnable()
    {
        start.action.Enable();
        start.action.performed += OnStart;
    }

    private void Awake()
    {
        if(mainMenuAnimator == null)
            Debug.LogError("MainMenuNavigation.cs: Main Menu Animator is not assigned in the inspector.");
        if(gameSelectorAnimator == null) 
            Debug.LogError("MainMenuNavigation.cs: Game Selector Animator is not assigned in the inspector.");
        if(eventSystem == null) 
            Debug.LogError("MainMenuNavigation.cs: Event System is not assigned in the inspector.");
        if(firstSelectedGameSelector == null) 
            Debug.LogError("MainMenuNavigation.cs: First Selected Game Selector is not assigned in the inspector.");
    }

    private void Start()
    {
        SoundManager.Instance?.PlayMusic(MusicType.TitleTheme);
    }
    private void OnStart(InputAction.CallbackContext context)
    {
        if(_pressedStart) return;

        _pressedStart = true;
        StartCoroutine(ShowGameSelector());
    }

    private IEnumerator ShowGameSelector()
    {
        mainMenuAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(1f);
        gameSelectorAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(0.5f);
        eventSystem.SetSelectedGameObject(firstSelectedGameSelector);
    }

    private void OnDisable()
    {
        start.action.performed -= OnStart;
        start.action.Disable();
    }
}
