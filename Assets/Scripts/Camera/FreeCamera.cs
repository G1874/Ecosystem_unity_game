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
    int mapSize;
    Vector3[, ] tileCenters;


    void Start()
    {
        GetInfo();
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
        
        if(transitionalVector.x > tileCenters[0, 0].x - 5f && transitionalVector.x < tileCenters[mapSize-1, mapSize-1].x + 5f && transitionalVector.y > 1 && transitionalVector.z > tileCenters[0, 0].z - 5 && transitionalVector.z < tileCenters[mapSize-1, mapSize-1].z + 5)
            transform.position = transitionalVector;        
    }

    private void zoom(){
        Vector3 transitionalVector = transform.position;

        if(Input.mouseScrollDelta.y > 0)
            transitionalVector += transform.forward * zoomSensitivity * Time.deltaTime;
        if(Input.mouseScrollDelta.y < 0)
            transitionalVector -= transform.forward * zoomSensitivity * Time.deltaTime;

        if(transitionalVector.x > tileCenters[0, 0].x - 5f && transitionalVector.x < tileCenters[mapSize-1, mapSize-1].x + 5f && transitionalVector.y > 1 && transitionalVector.z > tileCenters[0, 0].z - 5 && transitionalVector.z < tileCenters[mapSize-1, mapSize-1].z + 5)
        transform.position = transitionalVector;
    }

    private void rotation(){

        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
        transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
    }

    public void GetInfo(){
            GameObject generateMap = GameObject.Find("Generate Map");
            Terrain.MapGenerator mapGenerator = generateMap.GetComponent<Terrain.MapGenerator>();
            mapSize = mapGenerator.mapSize;
            tileCenters = mapGenerator.meshData.tileCenters;
        }
}
