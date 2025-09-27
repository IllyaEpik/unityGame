using UnityEngine;
using TMPro;
public class ManagerUi : MonoBehaviour
{
    [SerializeField] private GameObject heart1, heart2, heart3;

    private int battery = 0;

    [SerializeField] private TMP_Text textBattery;

    void Start()
    {
        
    }

    void Update()
    {
        textBattery.text = "" + battery;
    }

    public void AddBattery()
    {
        battery++;
    }

}
