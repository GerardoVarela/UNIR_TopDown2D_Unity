using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour
{
    [SerializeField] private UnityEvent onEventTrap;
    
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Feet"))
        {
            onEventTrap.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Feet"))
        {
            float r = _originalColor.r * 0.7f;
            float g = _originalColor.g * 0.7f;
            float b = _originalColor.b * 0.7f;
            _spriteRenderer.color = new Color(r, g, b, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Feet"))
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}
