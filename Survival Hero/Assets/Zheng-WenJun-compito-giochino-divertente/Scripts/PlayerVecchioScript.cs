using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVecchioScript: MonoBehaviour
{
    bool atterrato = true;
    private Rigidbody2D rb;
    private float movSpeed = 14f;
    public InputAction playerControls;
    Vector2 movDirection = Vector2.zero;



    //Questo mi serve per l'input system
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //prendo la direzione, ho provato a usare input system
        movDirection = playerControls.ReadValue<Vector2>();

        //Questa parte fa fare il salto al player
        if (Input.GetButtonDown("Jump") && atterrato)
        {
            rb.velocity = new Vector2(rb.velocity.x, movSpeed);
        }

        //Questa parte da movimento al player
        rb.velocity = new Vector2(movDirection.x * 7f, rb.velocity.y);


    }


    //Questa parte serve per verificare che c'è o meno collisione con GameObject Ground,
    //questo mi è utile per far saltare il player quando è atterrato
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            atterrato = true;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            atterrato = false;
        }
    }
}
