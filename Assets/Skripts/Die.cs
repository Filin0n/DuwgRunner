using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "DieObg")
        {
            SceneManager.LoadScene(0);
        }
    }



}

