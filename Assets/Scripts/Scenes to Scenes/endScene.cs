using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class endScene : MonoBehaviour
{
   [SerializeField] private bool stringIndex;
   
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Player")
      {
         stringIndex = true;
         SceneManager.LoadScene(1);
      }
   }

   
}
