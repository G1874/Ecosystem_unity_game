using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    
    public float movementSpeed = 20f;
    public float fastMovementSpeed = 100f;
    public float freeLookSensitivity = 10f;
    public float zoomSensitivity = 200f;
    public float panBorderThickness = 30f;
    public float rotationBorderThicness = 100f;

    void Start()
    {
        transform.position.Set(0f, 100f, 0f);
    }

    void Update()
    {   
        
        if(Input.GetKey(KeyCode.LeftShift))
            transitionalMotion(fastMovementSpeed);
         else    
            transitionalMotion(movementSpeed);

        if(Input.GetMouseButton(1))
        rotation();
        
        zoom();
    }

    private void transitionalMotion(float speed){

    Vector3 transitionalVector = transform.position; 

        if(Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= Screen.height - panBorderThickness && Input.mousePosition.y <= Screen.height))
            transitionalVector = new Vector3(transitionalVector.x + (transform.forward.x * speed * Time.deltaTime), transitionalVector.y, transitionalVector.z + (transform.forward.z * speed * Time.deltaTime));
        
        if(Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= panBorderThickness && Input.mousePosition.y >= 0f))
            transitionalVector = new Vector3(transitionalVector.x - (transform.forward.x * speed * Time.deltaTime), transitionalVector.y, transitionalVector.z - (transform.forward.z * speed * Time.deltaTime));

        if(Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= Screen.width - panBorderThickness && Input.mousePosition.x <= Screen.width))
            transitionalVector = transitionalVector + (transform.right * speed * Time.deltaTime);
        
        if(Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= panBorderThickness && Input.mousePosition.x >= 0f))
            transitionalVector = transitionalVector - (transform.right * speed * Time.deltaTime);

        if(Input.GetKey("q"))
            transitionalVector.y += speed * Time.deltaTime;

        if(Input.GetKey("e"))
            transitionalVector.y -= speed * Time.deltaTime;
    
        transform.position = transitionalVector;        
    }

    private void zoom(){

        if(Input.mouseScrollDelta.y > 0)
            transform.position += transform.forward * zoomSensitivity * Time.deltaTime;
        if(Input.mouseScrollDelta.y < 0)
            transform.position += -transform.forward * zoomSensitivity * Time.deltaTime;  
    }

    private void rotation(){

        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
        transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
    }
}
