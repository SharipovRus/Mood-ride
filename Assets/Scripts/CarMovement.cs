using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private GameObject playerCam;
    [SerializeField] float moveSpeedForward = 50f; 
    [SerializeField] float moveSpeedBackward = 25f; 
    public float rotationSpeed = 70f;
    public Rigidbody rb;
    public Transform forwardIndicator;
    private Vector3 lastMovement;
    private AudioSource audioSource; 
    public AudioClip carSound; 
    public Joystick joystick; 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); 
        audioSource.clip = carSound; 
        audioSource.loop = true; 
        audioSource.playOnAwake = true; 
        audioSource.Play(); 
    }

    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        float moveInput = joystick.Vertical; // Получаем вертикальное значение джойстика
        float rotationInput = joystick.Horizontal; // Получаем горизонтальное значение джойстика

        // Получаем направление переда машины
        Vector3 forwardDirection = forwardIndicator.position - transform.position;
        forwardDirection.Normalize();

        float currentMoveSpeed = moveInput > 0f ? moveSpeedForward : moveSpeedBackward; // Определяем текущую скорость в зависимости от направления движения

        Vector3 movement = forwardDirection * moveInput * currentMoveSpeed * Time.deltaTime;

        if (rotationInput != 0f && moveInput == 0f)
        {
            // Добавляем движение вперед при повороте на месте
            movement += forwardDirection * Mathf.Sign(moveInput) * 0.1f * currentMoveSpeed * Time.deltaTime;
        }

        rb.MovePosition(rb.position + movement);
        lastMovement = movement;

        Quaternion rotation = Quaternion.Euler(0f, rotationInput * rotationSpeed * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
