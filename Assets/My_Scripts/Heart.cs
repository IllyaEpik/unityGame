using UnityEngine;

public class Heart : MonoBehaviour
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
        if (collision.CompareTag("Hero"))
        {
            Invoke("DeleteHeart", 0.3f);
        }
    }

    private void DeleteHeart()
    {
        Destroy(gameObject);
    }
}
