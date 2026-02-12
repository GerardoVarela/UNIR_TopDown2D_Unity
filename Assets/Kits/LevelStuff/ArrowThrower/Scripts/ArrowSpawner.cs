using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways] // Permite que el script ejecute lógica básica en modo edición
public class ArrowSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Direction direction = Direction.Down;
    [SerializeField] private float distanceSpawnPoint = 0.23f;

    [Header("Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float spawnSpeed = 1f;
    
    private SpriteRenderer _spriteRenderer;

    private void OnDrawGizmos()
    {
        if(spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);
        }
    }

    // Se ejecuta cada vez que cambias un valor en el Inspector
    private void OnValidate()
    {
        UpdateVisuals();
    }

    private void Awake()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        if (spawnPoint == null) return;

        transform.rotation = Quaternion.identity;
        spawnPoint.localPosition = Vector3.zero;

        switch (direction)
        {
            case Direction.Up:
                _spriteRenderer.flipY = true;
                spawnPoint.localPosition = Vector3.up * distanceSpawnPoint;
                break;

            case Direction.Down:
                _spriteRenderer.flipY = false;
                spawnPoint.localPosition = Vector3.down * distanceSpawnPoint;
                break;

            case Direction.Left:
                _spriteRenderer.flipY = false;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                spawnPoint.localPosition = Vector3.down * distanceSpawnPoint;
                break;
            
            case Direction.Right:
                _spriteRenderer.flipY = false;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                spawnPoint.localPosition = Vector3.down * distanceSpawnPoint;
                break;
        }
    }

    public void ShootArrow()
    {
        if (arrowPrefab == null || spawnPoint == null) return;

        GameObject arrowInstance = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
        Arrow arrowScript = arrowInstance.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(direction);
            arrowScript.SetSpeed(spawnSpeed);
        }
    }
}