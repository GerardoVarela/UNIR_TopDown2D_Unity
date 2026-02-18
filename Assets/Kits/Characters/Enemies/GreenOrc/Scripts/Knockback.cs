using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool gettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb2D.mass;
        rb2D.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb2D.linearVelocity = Vector2.zero;
        gettingKnockedBack = false;
    }

}
