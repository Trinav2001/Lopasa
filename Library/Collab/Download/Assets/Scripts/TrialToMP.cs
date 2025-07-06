using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndingScript : MonoBehaviour
{
    [SerializeField] private string getName = "Main Menu";
    [SerializeField] private bool isColliding = false;
    
    private Collider2D boxCollider2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding= true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}