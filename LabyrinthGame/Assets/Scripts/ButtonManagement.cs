using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagement : MonoBehaviour
{
   public void QuitButton() {
       // Quitter le jeu
       Debug.Log("Quit");
       Application.Quit();
   }

   public void StartButton() {
       // Lancer le jeu
       Debug.Log("Play");
       SceneManager.LoadScene("GameScene");
   }

   public void RulesButton() {
       // Règles du jeu
       Debug.Log("Game rules");
       SceneManager.LoadScene("GameRules");
       
   }

    public void BackToMainMenu()
    {
        Debug.Log("Back to main menu");
        SceneManager.LoadScene("MenuScene");
    }
}
