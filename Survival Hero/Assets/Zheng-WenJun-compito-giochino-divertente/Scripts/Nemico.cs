using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nemico : MonoBehaviour
{
    public LayerMask layerMask;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    [SerializeField] private float movSpeed;
    //private bool girare;
    //private Vector3 ultimaPosizione;
    private bool isColliding;
    private GameObject punteggio;
    //[SerializeField] private float contatore;
    //private TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movSpeed = 4f;
        
        //girare = false;
        isColliding = false;
        animator.SetBool("run", true);
        punteggio = GameObject.FindWithTag("Punteggio");
        //textMesh = punteggio.GetComponent<TextMeshPro>();


    }

    // Update is called once per frame
    void Update()
    {
        GiraPersonaggio();
        if (animator.GetBool("run"))
        {
            if (isColliding == false)
            {
                rb.velocity = new Vector2(movSpeed, 0);
            }
            else rb.velocity = new Vector2(-movSpeed, 0);
        }




        Debug.Log(isColliding);
    }




    private void GiraPersonaggio()
    {
        if (isColliding == false)
        {
            sprite.flipX = false;

        }
        else
        {
            sprite.flipX = true;
        }
    }


    private void AggiornaAnimazione()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Oggetti"))
        {
            isColliding = !isColliding;
            Debug.Log("hitting oggetti");
        }
        if (animator.GetBool("run"))
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene(2);
            }
        }
        if (animator.GetBool("dead"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("punti tocco");
                animator.SetBool("run", false);
                animator.SetBool("dead", false);
                animator.SetBool("exit", true);

            }
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}
