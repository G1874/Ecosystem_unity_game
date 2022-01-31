using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI{
    public class CursorController : MonoBehaviour
    {
        public Texture2D cursor;

        void changeCursor(Texture2D cursorType){
            Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
        }

        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            changeCursor(cursor);
            Cursor.lockState = CursorLockMode.Confined;
        }

        
        // void Update()
        // {
        //     if(Input.GetMouseButtonDown(0)){
        //         RaycastHit hit;
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         if(Physics.Raycast(ray, out hit, 1000f))
        //         {
        //             if(hit.transform.gameObject.name == "DeerPrefab(Clone)"){
        //                 Debug.Log("tak");
        //             }
        //             else if(hit.transform.gameObject != null)
        //             {
        //                 Debug.Log("nie");
        //             }
        //             else
        //             {
        //                 Debug.Log("problem");
        //             }
        //         }
        //     }
        // }
    }
}