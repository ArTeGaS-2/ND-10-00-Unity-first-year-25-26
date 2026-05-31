using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Переміщення")]
    public float speed = 10f; // Швидкість руху гравця
    public float maxSpeed = 8f;
    public float jumpForce = 7f;

    [Header("GroundCheck")]
    public Transform groundCheck; // Точка для перевірки, чи знаходиться гравець на землі
    public float groundCheckRadius = 0.25f; // Радіус для перевірки, чи знаходиться гравець на землі
    public LayerMask groundLayer; // Шар, який визначає, що є землею

    private Rigidbody rb; // Фізичний компонент
    private bool isGrounded; // Чи на землі ми

    private void Start() // Викликається при запуску гри
    {
        // Отримуємо компонент Rigidbody
        rb = GetComponent<Rigidbody>();
    }
    // Викликається з фіксованою частотою, використовується для фізичних обчислень
    private void FixedUpdate() 
    {
        Move();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
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

        Vector3 move = new Vector3(horizontal, 0, vertical);

        rb.velocity = move * speed;
    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer);
    }
}
