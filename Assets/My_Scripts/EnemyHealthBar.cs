using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform healthBarCanvas;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    private enemyBot enemy;

    void Start()
    {
        enemy = GetComponent<enemyBot>();
    }

    void Update()
    {
        healthBarCanvas.position = transform.position + offset;
        // fillImage.fillAmount = enemy.currentHealth / enemy.maxHealth;
    }




    public Transform target; // Враг


    private void LateUpdate()
    {
        // Debug.Log("hello");
        return;
        if (target == null) return;
        transform.position = target.position + offset; // Следует за врагом
        transform.rotation = Quaternion.identity; // Не вращается с ним
    }
    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
