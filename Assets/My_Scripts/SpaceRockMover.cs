using UnityEngine;

public class SpaceRockMover : MonoBehaviour
{
    
   // Скорость движения (в мировых единицах в секунду). Абсолютное значение, направление задается DirectionMultiplier.
    public float moveSpeed = 1.0f; 
    // Если установлено в 1, объект будет случайно поворачиваться при каждом сбросе позиции.
    public int rotation_reset = 1; 

    // Скорость вращения объекта (в градусах в секунду).
    public float rotationSpeed = 50.0f;

    // Расстояние от центра экрана по оси X, при достижении которого объект сбрасывается. 
    // Должно быть немного больше, чем половина видимой ширины экрана.
    public float resetDistance = 15f; 

    // Минимальное значение Y-координаты для случайного появления объекта при сбросе.
    public float minY = -5f; 

    // Максимальное значение Y-координаты для случайного появления объекта при сбросе.
    public float maxY = 5f;

    // Множитель направления движения и сброса: 
    // -1 (по умолчанию): объект летит влево, сбрасывается справа.
    // +1: объект летит вправо, сбрасывается слева.
    public int DirectionMultiplier = -1; 

    // Минимальная насыщенность цвета (Saturation) для рандомизации (ближе к 0 - более серый).
    public float minSaturation = 0.0f; 

    // Максимальная насыщенность цвета (Saturation) для рандомизации.
    public float maxSaturation = 0.3f; 

    // Минимальная яркость цвета (Value) для рандомизации (ближе к 0 - более темный).
    public float minValue = 0.1f; 

    // Максимальная яркость цвета (Value) для рандомизации.
    public float maxValue = 0.5f;

    private SpriteRenderer spriteRenderer; 

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на объекте " + gameObject.name);
        }
    }

    void Update()
    {
        
        transform.Translate(Vector3.right * moveSpeed * DirectionMultiplier * Time.deltaTime, Space.World);
        
        
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        
        

        
        if (DirectionMultiplier < 0 && transform.position.x < -resetDistance)
        {
            ResetPosition();
        }

        else if (DirectionMultiplier > 0 && transform.position.x > resetDistance)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
       
        float newY = Random.Range(minY, maxY);
        
        
        float newX = -resetDistance * DirectionMultiplier;

        transform.position = new Vector3(newX, newY, transform.position.z);
        if (rotation_reset == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
        
        
        if (spriteRenderer != null)
        {
            float randomHue = Random.Range(0f, 1f);
            float randomSaturation = Random.Range(minSaturation, maxSaturation);
            float randomValue = Random.Range(minValue, maxValue);

            Color newColor = Color.HSVToRGB(randomHue, randomSaturation, randomValue);
            spriteRenderer.color = newColor;
        }
    }
}