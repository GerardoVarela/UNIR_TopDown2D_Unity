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
    }
    public void OnHitReceived(float damage)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
            onLifeChanged.Invoke(currentLife/startingLife);
            if (currentLife < 0f)
            {
                onDeath.Invoke();
            }
        }
    }

    internal void RecoverHealth(float healthRecovery)
    {
        if (currentLife > 0f)
        {
            currentLife += healthRecovery;
            currentLife = Mathf.Clamp(currentLife/startingLife,0f, startingLife);
            onLifeChanged.Invoke(currentLife);
        }
    }
}
