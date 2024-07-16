using UnityEngine;

public class UntoggleMouse : MonoBehaviour
{
    private bool isCursorVisible = true;

    void Awake()
    {
        SetCursorVisibility();
    }

    void SetCursorVisibility()
    {
        isCursorVisible = true;
        Cursor.visible = isCursorVisible;
        Cursor.lockState = CursorLockMode.None;
    }
}

