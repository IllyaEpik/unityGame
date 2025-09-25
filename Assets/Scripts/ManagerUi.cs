using UnityEngine;
using TMPro;
public class ManagerUi : MonoBehaviour
{
    [SerializeField] private GameObject heart1, heart2, heart3, heart4, heart5;

    private int battery = 0;

    [SerializeField] private TMP_Text textBattery;

    void Start()
    {
        
    }

    void Update()
    {
        textBattery.text = "" + battery;
    }

    public void AddBatteries()
    {
        battery++;
    }

}
