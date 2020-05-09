using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;

using System.Threading;


public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator animator;
    public ArduinoReader arduinoReader;
    public GameObject[] keys;
    public GameObject[] doors;

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

    const float objectMinDistance = 10f;

    ArrayList playerKeys;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        arduinoReader = new ArduinoReader();
        keys = GameObject.FindGameObjectsWithTag("Keys");
        doors = GameObject.FindGameObjectsWithTag("Doors");
        
        playerKeys = new ArrayList();
        playerKeys.Add("KeyEndDoor");
        arduinoReader.Start();
        animator.SetBool("isIdling", true); animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    // Update is called once per frame
    void Update()
    {
        //print("CALL the move function\n");
       /* print("keys are:");
        foreach(GameObject key in keys)
        {
            print(key.tag);
            print(key.name);
        }*/
        //openDoor(10);
        //print("Close To doors: " + CheckCloseToTag("Doors", 10));
        arduinoReader.Update();
        this.UpdatePosition();

    }

    void OpenDoor()
    {
        foreach (GameObject door in doors)
        {
            if (Vector3.Distance(transform.position, door.transform.position) <= objectMinDistance)
            {
                
                print("Near door " + door.name);
                Animator doorAnimator = door.GetComponent<Animator>();
                if (CanOpenDoor(door.name))
                {
                    doorAnimator.SetBool("isOpened", true);
                    EndGame(door.name);
                }

            }
        }
    }

    bool KeyExists(string doorName)
    {

        foreach(string key in playerKeys)
            if (key.EndsWith(doorName))
                return true;
        
        return false;
    }

    bool CanOpenDoor(string door)
    {
        return (door == "StartDoor") || KeyExists(door);
    }

    void PickUpKey()
    {
        foreach (GameObject key in keys)
        {
            if (Vector3.Distance(transform.position, key.transform.position) <= objectMinDistance)
            {
                print("\n\n\nPick up key " + key.name + "\n\n\n");

                playerKeys.Add(key.name);
                key.SetActive(false);
            }
        }
    }

    void EndGame(string doorName)
    {
        if (doorName.Equals("EndDoor"))
        {
            print("Congratulations\n You have escaped this maze\n\n");

            //SceneManager.LoadScene("EndingGame", LoadSceneMode.Additive);
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
        }
    }

    bool CheckCloseToTag(string tag, float minimumDistance)
    {
        GameObject[] goWithTag = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < goWithTag.Length; ++i)
        {
            if (Vector3.Distance(transform.position, goWithTag[i].transform.position) <= minimumDistance)
            {
                //goWithTag[i]
                return true;
            }
                
        }

        return false;
    }


    void UpdatePosition()
    {
        float coefX = Input.GetAxis("Horizontal");
        float coefY = Input.GetAxis("Vertical");

        float translation = coefY * speedY * Time.deltaTime;
        float rotation = coefX * speedX * Time.deltaTime;
        this.UpdateSpeed(coefX, coefY);
        if (Input.GetKeyDown(KeyCode.Space) == true)
            ActionManager(1);


        /*int[] dataXYZ = arduinoReader.coefXYZ;
        UpdateSpeed(dataXYZ[1], dataXYZ[0]);
        
        ActionManager(dataXYZ[2]);
        float translation = -dataXYZ[1] * speedY * Time.deltaTime;
        float rotation = dataXYZ[0] * speedX * Time.deltaTime;*/


        /*print("coefX: " + (-dataXYZ[1]) + "\tcoefY: " + dataXYZ[0]);
        print("Translation: " + translation + "\tspeedY: " + speedY);
        print("Rotation: " + rotation + "\tspeedX: " + speedX);*/

        AnimatorManager(translation, rotation);
        
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
                //print("The player is running");
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdling", false);
                //print("The player is walking");
            }
        }
        else
        {

            animator.SetBool("isIdling", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            //print("The player is idling");
        }

        //print("********************\nidle: " + animator.GetBool("isIdling") + "\nwalk : " + animator.GetBool("isWalking") + "\nrun : " + animator.GetBool("isRunning") + "\n********************\n");
    }

    public void ActionManager(int buttonState)
    {
        if (buttonState == 1)
        {
            print("\n\nHave clicked on the button\n\n");
            OpenDoor();
            PickUpKey();
        }
    }
}
