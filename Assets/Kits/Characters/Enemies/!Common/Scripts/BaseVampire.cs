using UnityEngine;

public class BaseVampire : BaseCharacter
{

    Sight2D sight;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
    }

    protected override void Update()
    {
        base.Update();
        Transform closesTarget = sight.GetClosesTarget();
        if (closesTarget != null)
        {
            Move((closesTarget.position - transform.position).normalized);
        }
    }
}
