using UnityEngine;

public class GizmosReference : MonoBehaviour
{
    public enum ShapeGizmo
    {
        WireCube,
        WireSphere,
        WireRectangle,
    }
    
    [Header("Gizmo Settings")]
    [SerializeField] private ShapeGizmo shape;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private float gizmoSize = 1f;
    [SerializeField] private Vector2 gizmoOffset = Vector2.zero;

    [Header("Gizmo Settings - Rectangle Mode Exclusive")]
    [SerializeField] private Vector2 rectangleSize = Vector2.one;


    private void OnDrawGizmos()
    {
        if (!drawGizmo || gizmoSize <= 0f) return;

        Gizmos.color = color;

        switch (shape)
        {
            case ShapeGizmo.WireCube:
                Gizmos.DrawWireCube((Vector2)transform.position + gizmoOffset, transform.localScale * gizmoSize);
                break;
            case ShapeGizmo.WireSphere:
                Gizmos.DrawWireSphere((Vector2)transform.position + gizmoOffset, (transform.localScale.x / 2f) * gizmoSize);
                break;
            case ShapeGizmo.WireRectangle:
                Gizmos.DrawWireCube((Vector2)transform.position + gizmoOffset, new Vector3(rectangleSize.x, rectangleSize.y, 0f) * gizmoSize);
                break;
        }
    }
    
}
