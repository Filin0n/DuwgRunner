using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping = 1.5f; //скольжение камеры
    public Vector2 offset = new Vector2(2f, 1f); //смещение камеры относительно персонажа
    public bool isLeft; // проверка взгляда персонажа 
    private Transform player; //определение положения персонажа 
    private int lastX; //проверка в какую сторону смотрел персонаж

    //Лимиты камеры
    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float upperLimit;


    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);    //математическое вычисление смещения
    
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);
        if (playerIsLeft)
        {
            transform.position = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (player)
        {
            int currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX) isLeft = false;
            else if (currentX < lastX) isLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if (isLeft)
            {
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            }
            else
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);

            transform.position = currentPosition;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),  // разрешается двигатся только в этом диопазоне
                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                transform.position.z
                );
        }
    }
    private void OnDrawGizmos()
    { //рисовка линий
        Gizmos.color = Color.red; // цвет линии
        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit)); //Вектор линии
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));

    }



}
