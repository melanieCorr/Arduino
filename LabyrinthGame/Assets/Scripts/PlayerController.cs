using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator animator;
    public ArduinoReader arduinoReader;

    const float maxSpeedTranslation = 28.5125f;//85.125f;
    const float minSpeedTranslation = 5.0125f;
    const float translationStep     = 0.05f;
    const float runSpeedTranslation = 20f;

    const float maxSpeedRotation    = 65f;
    const float minSpeedRotation    = 10f;
    const float rotationStep        = 3f;
    const float runSpeedRotation    = 50f;

    static float speedX             = minSpeedRotation;
    static float speedY             = minSpeedTranslation;

    const float coefXY              = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        arduinoReader = new ArduinoReader();
        arduinoReader.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //print("CALL the move function\n");
        arduinoReader.Update(); 
        this.UpdatePosition();
        
    }

    void UpdatePosition()
    {
        /*float coefX = Input.GetAxis("Horizontal");
        float coefY = Input.GetAxis("Vertical");

        float translation = coefY * speedY * Time.deltaTime;
        float rotation = coefX * speedX * Time.deltaTime;
        this.UpdateSpeed(coefX, coefY);*/


        int[] dataXYZ = arduinoReader.coefXYZ;
        this.UpdateSpeed(dataXYZ[1], dataXYZ[0]);

        float translation = -dataXYZ[1] * speedY * Time.deltaTime;
        float rotation = dataXYZ[0] * speedX * Time.deltaTime;


        print("coefX: " + (-dataXYZ[1]) + "\tcoefY: " + dataXYZ[0]);
        print("Translation: " + translation + "\tspeedY: " + speedY);
        print("Rotation: " + rotation + "\tspeedX: " + speedX);

        this.AnimatorManager(translation, rotation);
        
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MovePosition(rb.position + this.transform.forward * translation);
        rb.MoveRotation(rb.rotation * turn);

    }

    public void UpdateSpeed(float coefX, float coefY)
    {
        if (coefX == 0)
            speedX = minSpeedRotation;
        else if (speedX <= maxSpeedRotation)
            speedX += rotationStep;

        if (coefY == 0)
            speedY = minSpeedTranslation;
        else if (speedY <= maxSpeedTranslation)
            speedY += translationStep;

    }

    public void AnimatorManager(float translation, float rotation)
    {
        if(translation != 0 || rotation != 0)
        {
            if(speedX >= (minSpeedRotation + rotationStep * coefXY) || speedY >= (minSpeedTranslation + translationStep * coefXY))
            //if (speedX >= runSpeedTranslation || speedY >= runSpeedRotation)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                print("The player is running");
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdling", false);
                print("The player is walking");
            }
        }
        else
        {

            animator.SetBool("isIdling", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            print("The player is idling");
        }

        print("********************\nidle: " + animator.GetBool("isIdling") + "\nwalk : " + animator.GetBool("isWalking") + "\nrun : " + animator.GetBool("isRunning") + "\n********************\n");
    }

    public void ActionManager(int buttonState)
    {
        if (buttonState == 1)
        {
           
        }
    }
}
