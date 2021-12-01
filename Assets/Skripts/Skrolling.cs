using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skrolling : MonoBehaviour
{
    public bool scroling, paralax;

    public float bacgroundSize; // размер заднего фона
    public float paralaxSpeed;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10; //область видимости камеры
    private int leftIndex; // макс влево
    private int rightIndex; // макс вправо

    private float lastComeraX;

    private void Start()
    {

        cameraTransform = Camera.main.transform; // положение главной камеры
        lastComeraX = cameraTransform.position.x;

        layers = new Transform[transform.childCount]; //<-childCount-> считает количество подчененных 
        for(int i=0;i<transform.childCount;i++) 
        {
            layers[i] = transform.GetChild(i); // положение дочернего объекта под номером <-i->
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1; // длина количества слоев минус один(тоесть камера всегда видит вторую картинку тоесть ту что посередине)
    }

    private void FixedUpdate()
    {
        if (paralax)
        {
            float deltaX = cameraTransform.position.x - lastComeraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
        }

        lastComeraX = cameraTransform.position.x;
        if (scroling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            {
                ScrollLeft();
            }

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x + viewZone))
            {
                ScrollRight();
            }
        }
    }


    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - bacgroundSize);
        leftIndex = rightIndex;
        rightIndex--;

        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + bacgroundSize);
        rightIndex = leftIndex;
        leftIndex++;

        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

}
