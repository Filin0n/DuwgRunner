using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // компонент Rigidbody
    public float speed; // скорость 
    public float JumpForse = 7; // сила прыжка
    public bool onGround; // косание земли
    public bool onAirJump; //косание области воздушного прыжка
    public Transform GoundCheck; //отслеживание GoundCheck
    public float checkRadius = 0.5f;
    public LayerMask Ground; //Слой с землей
    public LayerMask airGround;//слой с объектом воздушного прыжка

    public Animator anim;
    public Vector2 moveVector;

    public int lungeImpulse = 500; //импульс рывка
    public bool lockLunge = false; // блокировка рывка
    public float lungegPause = 2; // перезарядка рывка

    private bool isRolling = false; //блок анимации переката

    public int dashImpulse = 500; //импульс дэша
    public bool lockDash = false; // блокировка дэша
    public float dashPause = 2; // перезарядка дэша



    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // присваивание компонента Rigidbody
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        Jump();
        ChekingGroung();
        Lunge();
        RollingOver();
        Dash();
        AirJump();
    }

    void Run() {
        moveVector= new Vector2(speed, rb.velocity.y); 
        rb.velocity = new Vector2(speed, rb.velocity.y); // перемещение персонажа по оси Y
        anim.SetFloat("MoveX", Mathf.Abs(moveVector.x));
        anim.SetFloat("MoveY", moveVector.y); // отслеживание передвижения по оси Y
    }
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))&& onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * JumpForse);
        }
    }
    
    void ChekingGroung()
    {
        onGround = Physics2D.OverlapCircle(GoundCheck.position, checkRadius, Ground); //персонаж на земле
        onAirJump = Physics2D.OverlapCircle(GoundCheck.position, checkRadius, airGround); //персонаж в области воздушного прыжка

        anim.SetBool("onGround",onGround);
    }
    void Lunge() //Рывок
    {
        if (onGround) lockLunge = false;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !lockLunge && !onGround)
        {
            lockLunge = true;//блок рывка
            anim.StopPlayback(); //Остановка всех анимаций
            anim.Play("lunge"); //запуск анимации lunge
            //rb.velocity = new Vector2(0,0);
            rb.AddForce(Vector2.right * lungeImpulse);
            
        }
    }
 
    void Dash() //дэш
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !lockDash && !onGround)
        {
            lockDash = true;
            Invoke("LockDash", dashPause);//перезарядка дэша
            anim.StopPlayback(); //Остановка всех анимаций
            anim.Play("Dash"); //запуск анимации lunge
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.down * dashImpulse);
        }
    }
    void LockDash()// блок деша
    {
        lockDash = false;
        //CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
        //coll.size = new Vector2(0.15f,0.15f);   
    }

    void RollingOver() // перекат 
    { 
        if (Input.GetKeyDown(KeyCode.S)) {
            anim.SetBool("IsRolling", isRolling = true);

            CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
            coll.size = new Vector2(0.23f,0f);//изменение размера колайдера
            coll.offset = new Vector2(0f,-0.12f);//изменение положения колайдера

            anim.StopPlayback(); //Остановка всех анимаций
            anim.Play("RollingOver"); //запуск анимации RollingOver
        }
        if ( Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("IsRolling", isRolling = false);
            CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
            coll.size = new Vector2(0.21f, 0.47f);//изменение размера колайдера
            coll.offset = new Vector2(0f, 0f);//изменение положения колайдера
        }
    }

    void AirJump()// прыжок в водухе
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))&& onAirJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * JumpForse);
        }
    }




}
