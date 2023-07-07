using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //[SerializeField] private GameInput gameInput;
    private Rigidbody2D rb;
    [SerializeField] private float movSpeed;
    [SerializeField] private float saltoSpeed;
    private Animator animator;
    private SpriteRenderer sprite;
    Vector2 inputVector;
    private Vector2 ultimoInput;
    private PlayerInputActions playerInputActions;
    //private bool canJump;
    private InputAction jump;
    private InputAction attack;
    public LayerMask layerMask;
    //RaycastHit2D hit;
    private bool isColliding;
    private bool girato;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    private float laserLength;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxSize = new Vector3(1, 0.1f, 0);
        Application.targetFrameRate = 60;
        laserLength = 1.6f;
        movSpeed = 7f;
        saltoSpeed = 700f;
    }


    private void OnEnable()
    {
        jump = playerInputActions.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        attack = playerInputActions.Player.Attack;
        attack.Enable();
        attack.performed += Attack;
    }


    private void OnDisable()
    {
        jump.Disable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 vector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return vector;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Salto();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Attack();
    }



    private void Salto()
    {
        Debug.Log("Piedi per terra: "+GroundCheck());
        Debug.Log("isColliding: " + isColliding);
        if (GroundCheck() && jump.enabled && isColliding)
        {
            //rb.velocity = new Vector2(rb.velocity.x, 15);
            rb.AddForce(Vector2.up * saltoSpeed);


        }

    }
    private void Attack()
    {

        animator.SetTrigger("attack");
        RaycastHit2D hit;
        //Get the first object hit by the ray
        if (girato)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.8f, 0), Vector2.left, laserLength, layerMask);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.8f, 0), Vector2.right, laserLength, layerMask);
        }
        if (hit.collider !=null)
        {
            if (hit.collider.CompareTag("Nemico")) Debug.Log("Attacking enemy");
            Debug.Log(hit.collider.tag);
        }
        


    }

    private void Update()
    {
        if (girato)
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), Vector2.left * laserLength, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), Vector2.right * laserLength, Color.red);
        }
        //Debug.Log(inputVector);
        GroundCheck();
        GiraPersonaggio();
        AggiornaAnimazione();

        //Reference si trova nel file GameInput
        inputVector = GetMovementVector();

        //transform.Translate(inputVector * movSpeed * Time.deltaTime);
        //Questa parte da movimento al player
        if (GroundCheck() == false && isColliding == true && girato == false && inputVector.x > 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        else if (GroundCheck() == false && isColliding == true && girato == true && inputVector.x < 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        else rb.velocity = new Vector2(inputVector.x * movSpeed, rb.velocity.y);


        if (inputVector.x != 0)
        {
            ultimoInput = inputVector;
        }

        //Debug.Log(piediHitBox.playerGrounded());


    }

    

    private void GiraPersonaggio()
    {
        if (ultimoInput.x > 0)
        {
            sprite.flipX = false;
            girato = false;
        }
        else
        {
            sprite.flipX = true;
            girato = true;
        }
    }
    private void AggiornaAnimazione()
    {
        if(inputVector.x!=0  && GroundCheck())
        {
            animator.SetBool("jump", false);
            animator.SetBool("idle", false);
            animator.SetBool("running",true);
        }
        else if (GroundCheck()==false)
        {
            animator.SetBool("jump", true);
            animator.SetBool("idle", false);
            animator.SetBool("running", false);
        }
        else
        {
            animator.SetBool("jump", false);
            animator.SetBool("idle", true);
            animator.SetBool("running", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Oggetti"))
        {
            isColliding = true;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")|| collision.gameObject.CompareTag("Oggetti"))
        {
            isColliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + new Vector3(-0.1f, 0.1f, 0) - transform.up * maxDistance, boxSize);
    }
    private bool GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position+new Vector3(-0.1f,0.1f,0), boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            //Debug.Log("Hitting: ground");
            return true;
        }
        else
        {
            //Debug.Log("Not hitting ground" );
            return false;
        }
    }
}
