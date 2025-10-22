using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    public Vector3 offset = new Vector3(0, 3f, 0);

    public Transform target; // враг, за которым двигается полоска

    void Update()
    {
        if (target != null)
        {
            // Следует за врагом
            transform.position = target.position + offset;
        }
    }

    public void UpdateHealth(float current, float max)
    {
        if (fillImage != null)
            fillImage.fillAmount = current / max;
    }
}