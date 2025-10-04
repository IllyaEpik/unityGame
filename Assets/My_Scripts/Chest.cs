using Unity.VisualScripting;
using UnityEngine;
public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject batteryPref;
    ManagerUi managerUi;
    private bool isOpened = false;

    void Start()
    {
        managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("hero"))
        {
            if (managerUi.IsKey())
            {
                managerUi.UseKey();
                Instantiate(batteryPref, transform.position, Quaternion.identity);
                isOpened = true;
                Destroy(gameObject); 
            }
        }
    }
}