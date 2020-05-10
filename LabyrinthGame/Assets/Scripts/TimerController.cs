using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DentedPixel ;

public class TimerController : MonoBehaviour
{
    public GameObject timeBar;
    static int nbMns = 10;
    public float time = 60f * nbMns;
    // Start is called before the first frame update
    void Start()
    {
        //timeBar = GameObject.FindGameObjectWithTag("Timer2");
        AnimateBar();
    }

    // Update is called once per frame
    void Update()
    {

        //AnimateBar();
    }

    public void AnimateBar()
    {
        LeanTween.scaleX(timeBar, 0, time).setOnComplete(() =>
        {
             SceneManager.LoadScene("GameOverScene");
        }); 
    }
}
