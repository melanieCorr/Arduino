using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagement : MonoBehaviour
{
   public void QuitButton() {
       // Quitter le jeu
       Debug.Log("Quitter le jeu.");
       Application.Quit();
   }

   public void StartButton() {
       // Lancer le jeu
       Debug.Log("Lancer le jeu.");
       SceneManager.LoadScene("GameScene");
   }

   public void RulesButton() {
       // Règles du jeu
       Debug.Log("Règles du jeu.");
       
   }
}
