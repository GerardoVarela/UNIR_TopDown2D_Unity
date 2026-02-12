using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AxeKeyMechanism : MonoBehaviour
{
    [SerializeField] private InputActionReference interact;
    [SerializeField] private UnityEvent onMechanismEvent;

    private Animator _animator;
    private bool _isActivated = false;
    private bool _isPressed = false;
    
    // Definimos el color Cyan de forma clara
    private Color _hoverColor = new Color(0f, 1f, 1f, 1f); 

    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteractAxe;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isActivated) return;

        if(collision.CompareTag("Player"))
        {
            // Aplicamos el color
            _animator.SetBool("IsHover", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(_isActivated) return;

        if(collision.CompareTag("Player"))
        {
            if (_isPressed)
            {
                _isActivated = true;
                _isPressed = false;
                _animator.SetTrigger("Activate");
                StartCoroutine(WaitToInvokeEvent());
            }
        }
    }

    private IEnumerator WaitToInvokeEvent()
    {
        yield return new WaitForSeconds(0.5f);
        onMechanismEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(_isActivated) return;

        if(collision.CompareTag("Player"))
        {
            _animator.SetBool("IsHover", false);
            _isPressed = false;
        }
    }

    private void OnInteractAxe(InputAction.CallbackContext context)
    {
        if(_isActivated) return;
        _isPressed = true;
    }

    private void OnDisable()
    {
        interact.action.Disable();
        interact.action.performed -= OnInteractAxe;
    }
}