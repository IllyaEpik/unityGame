using UnityEngine;
using UnityEngine.UI;
public class ManagerUi : MonoBehaviour
{

    private int battery = 0;
    private bool isKey = false;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image shieldIcon;
    private int currentHealth;
    private bool hasShield = false;

    void Start()
    {
        currentHealth = hearts.Length;
        UpdateUI();
    }

    void Update()
    {

    }

    public void AddBattery()
    {
        battery++;
    }

    public void TakeKey()
    {
        isKey = true;
    }

    public bool IsKey()
    {
        return isKey;
    }

    public void UseKey()
    {
        isKey = false;
    }

    public void AddShield()
    {
    hasShield = true;
    shieldIcon.enabled = true;
    }

    public void TakeShield()
    {
        hasShield = false;
        shieldIcon.enabled = false;
    }

    public void TakeDamage()
    {
        if (hasShield)
        {
            hasShield = false;
            shieldIcon.enabled = false;
            return;
        }

        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < currentHealth);
        }
    }
}
