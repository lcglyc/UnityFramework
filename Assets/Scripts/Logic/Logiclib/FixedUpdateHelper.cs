using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class FixedUpdateHelper : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        EditorDrag();
    }
    
    private Vector3 m_screenPos = Vector3.zero;
    private bool IsMoving = false;
    void EditorDrag()
    {
        if( Input.GetMouseButtonDown(0) && !Stage.isTouchOnUI)
        {
            IsMoving = true;
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsMoving = false;
            m_screenPos = Vector3.zero;
        }
        
        if (IsMoving )
        {
            Vector3 offset =Camera.main.ScreenToWorldPoint( Input.mousePosition )- m_screenPos;
            offset.y = -5.5f;
            body.MovePosition(offset + transform.right * Time.deltaTime*0.01f);
        }
    }

}
