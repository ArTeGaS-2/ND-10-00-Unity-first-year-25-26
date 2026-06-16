using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Переміщення")]
    public float speed = 10f; // Швидкість руху гравця
    public float maxSpeed = 8f; // Максимальна швидкість гравця
    public float jumpForce = 7f; // Сила стрибка гравця

    [Header("Керування камерою")]
    public Transform cameraPivot; // Точка, навколо якої буде обертатися камера
    public Transform cameraTransform; // Трансформ камери, який буде слідувати за гравцем
    public float mouseSensetivity = 3f; // Чутливість миші для керування камерою
    public float minVertivalAngle = -30f; // Мінімальний вертикальний кут обертання камери
    public float maxVerticalAngle = 60f; // Максимальний вертикальний кут обертання камери

    [Header("GroundCheck")]
    public Transform groundCheck; // Точка для перевірки, чи знаходиться гравець на землі
    public float groundCheckRadius = 0.25f; // Радіус для перевірки, чи знаходиться гравець на землі
    public LayerMask groundLayer; // Шар, який визначає, що є землею

    private Rigidbody rb; // Фізичний компонент

    private float mouseX;
    private float mouseY;
    private bool isGrounded; // Чи на землі ми

    private void Awake()
    {
        // Отримуємо компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked; // Блокуємо курсор в центрі екрану
        Cursor.visible = false; // Ховаємо курсор
    }

    private void Start() // Викликається при запуску гри
    {
        
    }
    // Викликається з фіксованою частотою, використовується для фізичних обчислень
    private void FixedUpdate() 
    {
        Move();
    }
    private void Update()
    {
        RotateCamera();
        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    private void RotateCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensetivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensetivity;

        mouseY = Mathf.Clamp(mouseY, minVertivalAngle, maxVerticalAngle);

        cameraPivot.transform.position = transform.position;
        cameraPivot.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }
    private void Jump()
    {
        rb.velocity = new Vector3(
            rb.velocity.x,
            0f,
            rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Вісь руху вправо/вліво
        float vertical = Input.GetAxis("Vertical"); // Вісь руху вперед/назад

        // Створюємо вектор руху на основі введених осей
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        if (move.magnitude < 0.1f) return; // Якщо вектор руху дуже малий, не рухаємо гравця

        Vector3 camForward = cameraTransform.forward; // Вектор, що вказує вперед від камери
        Vector3 camRight = cameraTransform.right; // Вектор, що вказує вправо від камери

        camForward.y = 0f; // Ігноруємо вертикальну складову, щоб рух був тільки по горизонталі
        camRight.y = 0f; // Ігноруємо вертикальну складову, щоб рух був тільки по горизонталі

        camForward.Normalize(); // Нормалізуємо вектори, щоб вони мали довжину 1
        camRight.Normalize(); // Нормалізуємо вектори, щоб вони мали довжину 1

        // Обчислюємо напрямок руху на основі напрямку камери
        Vector3 moveDirection = camForward * move.z + camRight * move.x;

        // Створюємо вектор горизонтальної швидкості на основі поточної швидкості гравця
        Vector3 horizontalVelocity = new Vector3( 
            rb.velocity.x, // Горизонтальна складова швидкості гравця
            0f, // Ігноруємо вертикальну складову, щоб рух був тільки по горизонталі
            rb.velocity.z); // Горизонтальна складова швидкості гравця

        // Якщо поточна горизонтальна швидкість гравця менша за максимальну швидкість
        if (horizontalVelocity.magnitude < maxSpeed)
        {
            // Додаємо силу для руху гравця в напрямку руху, враховуючи максимальну швидкість
            rb.AddForce(moveDirection * speed, ForceMode.Force);
        }
    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer);
    }
}
