using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] private Image[] lifeSegments; //Life fill images

    private void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onDeath.AddListener(OnDeath);

    }

    void OnLifeChanged(float newLife)
    {
        UpdateLifeSegments(Mathf.Clamp01(newLife));
    }

    //This is for a life bar with many fill sprites, like many hearts
    void UpdateLifeSegments(float normalizedLife)
    {
        int segmentCount = lifeSegments.Length;

        if (segmentCount == 0)
            return;

        float lifePerSegment = 1f / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            float segmentMin = lifePerSegment * i;
            float segmentMax = lifePerSegment * (i + 1);

            if (normalizedLife >= segmentMax)
            {
                lifeSegments[i].fillAmount = 1f;
            }
            else if (normalizedLife > segmentMin)
            {
                float partialFill = (normalizedLife - segmentMin) / lifePerSegment;
                lifeSegments[i].fillAmount = partialFill;
            }
            else
            {
                // Segmento vacío
                lifeSegments[i].fillAmount = 0f;
            }
        }
    }


    void OnDeath()
    {
        Destroy(gameObject);
    }
}
