using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 _moveDirection;
    private float _speed;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += _moveDirection * _speed * Time.deltaTime;
    }

    public void SetDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                _moveDirection = Vector3.up;
                break;
            case Direction.Down:
                _moveDirection = Vector3.down;
                break;
            case Direction.Left:
                _moveDirection = Vector3.left;
                break;
            case Direction.Right:
                _moveDirection = Vector3.right;
                break;
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        if (playerCharacter != null)
        {
            playerCharacter.NotifyPunch(0.1f);
            Destroy(gameObject);
        }
    }
}