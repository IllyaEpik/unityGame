using UnityEngine;
public class ManagerUi : MonoBehaviour
{

    private int battery = 0;
    private bool isKey = false;

    void Start()
    {

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
}
