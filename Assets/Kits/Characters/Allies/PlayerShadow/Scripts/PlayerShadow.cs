using UnityEngine;

public class PlayerShadow : BaseCharacter
{
    [Header("Lifetime")]
    [SerializeField] float lifetime = 60f;

    [Header("Follow")]
    [SerializeField] Transform playerTarget;
    [Header("Follow Distance")]
    [SerializeField] float followDistance = 1.5f;      // distancia ideal al jugador
    [SerializeField] float followTolerance = 0.2f;     // margen para evitar jitter


    Sight2D sight;
    float spawnTime;
    bool isDead;

    protected override void Awake()
    {
        base.Awake();

        sight = GetComponent<Sight2D>();
        spawnTime = Time.time;

        // Suscribirse a muerte por daño
        if (life != null)
        {
            life.onDeath.AddListener(HandleDeath);
        }

    }

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        // --- muerte por tiempo ---
        if (Time.time - spawnTime >= lifetime)
        {
            HandleDeath();
            return;
        }

        // --- comportamiento IA ---
        Transform closestEnemy = sight != null ? sight.GetClosesTarget() : null;

        if (closestEnemy != null)
        {
            AttackTarget(closestEnemy);
        }
        else if (playerTarget != null)
        {
            FollowPlayer();
        }
        else
        {
            RequestStopMoving();

        }

    }

    void AttackTarget(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;

        attackDirection = direction;
        Move(direction);

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            PerformDirectionalAttack();
        }else
{
            RequestStopMoving();

        }

    }

    void FollowPlayer()
    {
        Vector2 toPlayer = playerTarget.position - transform.position;
        float distance = toPlayer.magnitude;

        // Si está demasiado lejos acercarse
        if (distance > followDistance + followTolerance)
        {
            Vector2 direction = toPlayer.normalized;
            attackDirection = direction;
            Move(direction);
        }
        else
        {
            RequestStopMoving();

        }

        // Si está dentro del rango cómodo  quedarse quieto
    }


    // --- MUERTE UNIFICADA ---
    void HandleDeath()
    {
        if (isDead)
            return;

        isDead = true;

        // aquí puedes poner animación, VFX, sonido, etc.
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // limpieza del listener (buena práctica)
        if (life != null)
        {
            life.onDeath.RemoveListener(HandleDeath);
        }
    }

    public void SetPlayerTarget(Transform target)
    {
        playerTarget = target;
    }
}
