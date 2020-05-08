using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    private float rotationSpeed = 85f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ConstantRotation();
    }
    void ConstantRotation()
    {
        this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
