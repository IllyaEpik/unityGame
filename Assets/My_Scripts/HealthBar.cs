using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target; // Враг
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    private Image fillImage;
    void Start()
    {
        fillImage = GetComponentInChildren<Image>();
    }

    void Update()
    {

    }
    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset; // Следует за врагом
        transform.rotation = Quaternion.identity; // Не вращается с ним
    }
    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
