using UnityEngine;

public class LeftRightGround : MonoBehaviour
{
    [SerializeField] private float rightOffset = 3f;  // Насколько вправо поедет платформа
    [SerializeField] private float leftOffset = -3f;  // Насколько влево поедет
    [SerializeField] private float speed = 2f;        // Скорость движения

    private Vector3 rightPoint;
    private Vector3 leftPoint;
    private bool movingRight = true;

    void Start()
    {
        rightPoint = transform.position + Vector3.right * rightOffset;
        leftPoint = transform.position + Vector3.right * leftOffset;
    }


    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, rightPoint) < 0.01f)
                movingRight = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, leftPoint) < 0.01f)
                movingRight = true;
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
