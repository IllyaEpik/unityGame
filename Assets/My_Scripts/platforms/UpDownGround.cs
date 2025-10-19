using UnityEngine;

public class UpDownGround : MonoBehaviour
{

    [SerializeField] private float topOffset = 3f;
    [SerializeField] private float bottomOffset = -3f;
    [SerializeField] private float speed = 2f;       // Скорость движения

    private bool movingUp = true; // Направление движения

    private Vector3 topPoint;
    private Vector3 bottomPoint;
    void Start()
    {
        topPoint = transform.position + Vector3.up * topOffset;
        bottomPoint = transform.position + Vector3.up * bottomOffset;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (movingUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, topPoint, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, topPoint) < 0.01f)
                movingUp = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bottomPoint, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, bottomPoint) < 0.01f)
                movingUp = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("hero"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("hero"))
        {
            collision.transform.SetParent(null);
        }
    }
}
