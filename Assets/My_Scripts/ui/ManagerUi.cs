using UnityEngine;
using UnityEngine.UI;
public class ManagerUi : MonoBehaviour
{
    // private int battery = 0;
    private bool isKey = false;
    // [SerializeField] private Image[] heart;

    // public Sprite[] BattaryElems; // managerUi
    private Animator batteryUiAnimator;
    private Animator HeartUiAnimator;
    public UnityEngine.UI.Image batteryUi; // managerUi
    // [SerializeField] private Sprite[] spritesOfHeart; // managerUi
    // [SerializeField] private UnityEngine.UI.Image[] hearts;
    [SerializeField] private UnityEngine.UI.Image heartUi;
    [SerializeField] private Image shieldIcon;
    // private int currentHealth;
    private Hero hero;
    private bool hasShield = false;


    void Start()
    {
        // currentHealth = hearts.Length;
        // UpdateUI();
        hero = GameObject.FindGameObjectWithTag("hero").GetComponent<Hero>();
        batteryUiAnimator = batteryUi.GetComponent<Animator>();
        HeartUiAnimator = heartUi.GetComponent<Animator>();
        batteryUiAnimator.speed = 0;
        updateHp();
        updateBattery();
    }

    void Update()
    {

    }

    // public void AddBattery()
    // {
    //     battery++;
    // }

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

        // if (currentHealth > 0)
        // {
        //     currentHealth--;
        //     UpdateUI();
        // }
    }

    // private void UpdateUI()
    // {
    //     for (int i = 0; i < hearts.Length; i++)
    //     {
    //         hearts[i].enabled = (i < currentHealth);
    //     }
    // }
    public void updateHp()
    {
        float hp = hero.health + 1;
        HeartUiAnimator.Play("heart", 0, Mathf.Clamp01(hp / 6f));
        HeartUiAnimator.Update(0f); 
        HeartUiAnimator.speed = 0;
    }
    public void updateBattery()
    {
        batteryUiAnimator.speed = 1;
        batteryUiAnimator.Play("battery", 0, Mathf.Clamp01(hero.battery / 6f));
        batteryUiAnimator.Update(0f); 
        batteryUiAnimator.speed = 0;
    }

}
