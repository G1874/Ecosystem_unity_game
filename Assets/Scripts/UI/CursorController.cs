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

        
        void Update()
        {
            
        }
    }
}