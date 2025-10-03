using UnityEngine;

public class Key : MonoBehaviour
{

    ManagerUi managerUi;
    void Start()
    {
        managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
    }


    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("132132");
        if (collision.CompareTag("hero"))
        {
            managerUi.TakeKey();
            Invoke("DeleteKey", 0.3f);
        }
    }

    private void DeleteKey()
    {
        Destroy(gameObject);
    }
}
