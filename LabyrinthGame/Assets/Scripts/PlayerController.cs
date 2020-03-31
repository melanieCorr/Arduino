using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator animator;
    public ArduinoReader arduinoReader;

    const float maxSpeedTranslation = 8.5125f;//85.125f;
    const float minSpeedTranslation = 5.0125f;
    const float translationStep     = 0.5f;

    const float maxSpeedRotation    = 65f;
    const float minSpeedRotation    = 50f;
    const float rotationStep        = 2f;

    static float speedX             = minSpeedRotation;
    static float speedY             = minSpeedTranslation;


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
        this.UpdatePosition();
        arduinoReader.Update(); 
    }

    void UpdatePosition()
    {

        /*        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
                float rotation = Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime;
        */
        int[] dataXYZ = arduinoReader.coefXYZ;

        this.UpdateSpeed(dataXYZ[1], dataXYZ[0]);

        float translation   = -dataXYZ[1] * speedY * Time.deltaTime;
        float rotation      = dataXYZ[0] * speedX * Time.deltaTime;
        
        this.AnimatorManager(translation, rotation);
        
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MovePosition(rb.position + this.transform.forward * translation);
        rb.MoveRotation(rb.rotation * turn);

        print("Translation: " + translation + "\tspeedY: " + speedY);
        print("Rotation: " + rotation + "\tspeedX: " + speedX);
    }

    public void UpdateSpeed(int coefX, int coefY)
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
        if(translation > 0 || rotation > 0)
        {
            if(speedX >= (minSpeedTranslation + translationStep * 3f) || speedY >= (minSpeedRotation + rotationStep * 3f))
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
}
