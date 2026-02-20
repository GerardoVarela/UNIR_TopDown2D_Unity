using System.Collections;
using UnityEngine;

public class PossessedSkullEnemy : BaseEnemy
{
    [Space(5)]
    [Header("Possessed Skull Settings")]
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float prepareSpeed = 3f;
    [SerializeField] private float prepareRadius = 0.5f;
    [SerializeField] private float attackSpeed = 10f;
    [SerializeField] private float damage = 0.2f;
    
    private bool _hasAllreadySeen = false;
    private Vector3 _initialPosition;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    protected override void Awake()
    {
        base.Awake();

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                Debug.LogWarning("PossessedSkullEnemy: Player Transform not assigned and not found by tag");
        }
    }

    protected override void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if (_hasAllreadySeen) return;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _initialPosition = transform.position;
                StartCoroutine(AttackAndAutoDestruction());
                _hasAllreadySeen = true;
                return;
            }
        }
    }

    private IEnumerator AttackAndAutoDestruction()
    {
        // 1. Preparación - movimiento circular errático
        float prepareDuration = Random.Range(2f, 3f);
        float elapsedTime = 0f;
        
        while (elapsedTime < prepareDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Movimiento circular con variación
            float angle = elapsedTime * prepareSpeed * Mathf.PI * 2;
            float radiusVariation = prepareRadius * (1 + Mathf.Sin(elapsedTime * 5f) * 0.3f);
            
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radiusVariation,
                Mathf.Sin(angle) * radiusVariation,
                0
            );
            
            transform.position = _initialPosition + offset;
            yield return null;
        }

        // 2. Ataque directo al jugador en línea recta
        Vector3 attackDirection = (playerTransform.position - transform.position).normalized;
        float maxAttackDistance = 20f; // Distancia máxima que recorrerá
        float distanceTraveled = 0f;

        while (distanceTraveled < maxAttackDistance)
        {
            transform.position += attackDirection * attackSpeed * Time.deltaTime;
            distanceTraveled += attackSpeed * Time.deltaTime;
            
            // Verificar colisión con el jugador
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) < 0.3f)
            {
                // Aquí llamarías al método de daño del jugador
                playerTransform.GetComponent<PlayerCharacter>()?.NotifyPunch(damage);
                Debug.Log($"Skull hit player for {damage} damage!");
                break;
            }
            
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}