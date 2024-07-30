using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{

    [SerializeField] private CursorManager.CursorType cursorType;

    private bool isMouseOver = false;

    private void OnMouseOver()
    {
        if (!isMouseOver)
        {
            CursorManager.Instance.SetActiveCursorType(cursorType);
            isMouseOver = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Debug.Log("Locked");

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Arrow);
        isMouseOver = false;
    }
}
