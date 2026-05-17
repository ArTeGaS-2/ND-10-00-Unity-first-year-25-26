using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f; // Швидкість руху гравця

    private Rigidbody rb; // Фізичний компонент

    private void Start() // Викликається при запуску гри
    {
        // Отримуємо компонент Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Викликається з фіксованою частотою, використовується для фізичних обчислень
    private void FixedUpdate() 
    {
        float horizontal = Input.GetAxis("Horizontal"); // Вісь руху вправо/вліво
        float vertical = Input.GetAxis("Vertical"); // Вісь руху вперед/назад

        Vector3 move = new Vector3(horizontal, 0, vertical);

        rb.velocity = move * speed;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(0, 5, 0, ForceMode.Impulse);
        }
    }
}
