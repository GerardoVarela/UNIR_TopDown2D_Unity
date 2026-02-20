using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] float startingLife = 1f;

    [Header("Debug")]
    [SerializeField] float debugHitDamage = 0.1f;
    [SerializeField] bool debugReceivedHit;

    [SerializeField] float currentLife;

    [SerializeField] public UnityEvent<float> onLifeChanged;
    [SerializeField] public UnityEvent onDeath;
    [SerializeField] public GameObject dropPrefab;
    private Knockback knockback;
    private Flash flash;

    private void OnValidate()
    {
        if (debugReceivedHit)
        {
            debugReceivedHit = false;
            OnHitReceived(debugHitDamage);
        }
    }

    private void Awake()
    {
        currentLife = startingLife;
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }
    public void OnHitReceived(float damage)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
            
            if (transform.CompareTag("Enemy")) knockback?.GetKnockedBack(PlayerCharacter.Instance.transform);
            StartCoroutine(flash?.FlashRoutine());

            onLifeChanged.Invoke(currentLife / startingLife);
            CheckHealth();
        }
    }

    internal void RecoverHealth(float healthRecovery)
    {
        if (currentLife > 0f)
        {
            currentLife += healthRecovery;
            currentLife = Mathf.Clamp(currentLife / startingLife, 0f, startingLife);
            onLifeChanged.Invoke(currentLife);
        }
    }

    public void CheckHealth()
    {
        if (currentLife <= 0.01f)
        {
            onDeath.Invoke();
            Vector3 offset = new Vector3(0, 0.5f, 0);
            if (dropPrefab != null)
            {
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
