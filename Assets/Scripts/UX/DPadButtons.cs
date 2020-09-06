using UnityEngine;
using System.Collections;

public class DPadButtons : MonoBehaviour
{
    public static bool up;
    public static bool down;
    public static bool left;
    public static bool right;

    float lastDpadX;
    float lastDpadY;

    float lastX;
    float lastY;

    public DPadButtons()
    {
        up = down = left = right = false;
        lastX = Input.GetAxis("DPadX");
        lastY = Input.GetAxis("DpadY");
    }

    void Update()
    {
        lastDpadX = lastX;
        lastDpadY = lastY ;
        if (Input.GetAxis("DPadX") == 1 && lastDpadX != 1) { right = true; } else { right = false; }
        if (Input.GetAxis("DPadX") == -1 && lastDpadX != -1) { left = true; } else { left = false; }
        if (Input.GetAxis("DPadY") == 1 && lastDpadY != 1) { up = true; } else { up = false; }
        if (Input.GetAxis("DPadY") == -1 && lastDpadY != -1) { down = true; } else { down = false; }
    }
}