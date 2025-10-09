using UnityEngine;
using UnityEngine.UI;
public class ManagerUi : MonoBehaviour
{
    private int battery = 0;
    private bool isKey = false;
    [SerializeField] private Image[] heart;

    public Sprite[] BattaryElems; // managerUi
    public UnityEngine.UI.Image batteryUi; // managerUi
    [SerializeField] private Sprite[] spritesOfHeart; // managerUi
    [SerializeField] private UnityEngine.UI.Image[] hearts;
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
    public void updateHp()
    {
        float hp = Hero.health - 0;
        foreach (UnityEngine.UI.Image heart in hearts)
        {

            Debug.Log($"hp: {hp}");
            if (hp >= 2)
            {
                heart.sprite = spritesOfHeart[4];
                hp -= 2;
            }
            else if (hp >= 1)
            {
                heart.sprite = spritesOfHeart[2];
                hp -= 1;
            }
            else
            {

                heart.sprite = spritesOfHeart[0];
            }
        }
    }
    public void updateBattery()
    {
        // battery -= 1;
        batteryUi.sprite = BattaryElems[battery];
    }

}
