using UnityEngine;
using System.Collections;

public class gamePadTesting : MonoBehaviour
{

    bool leftDPadActive = false;
    bool rightDPadActive = false;
    bool upDPadActive = false;
    bool downDPadActive = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i.ToString()))
            {
                print("button: " + i.ToString());
            }
        }
        if (Input.GetAxis("Select X") < 0.0f && !leftDPadActive)
        {
            print("left d-pad");
            leftDPadActive = true;
        }
        if (Input.GetAxis("Select X") > 0.0f && !rightDPadActive)
        {
            print("right d-pad");
            rightDPadActive = true;
        }
        if (Input.GetAxis("Select Y") > 0.0f && !upDPadActive)
        {
            print("up d-pad");
            upDPadActive = true;
        }
        if (Input.GetAxis("Select Y") < 0.0f && !downDPadActive)
        {
            print("down d-pad");
            downDPadActive = true;
        }

        if (Input.GetAxis("Select X") == 0.0f)
        {
            leftDPadActive = false;
            rightDPadActive = false;
        }
        if(Input.GetAxis("Select Y") == 0.0f)
        {
            upDPadActive = false;
            downDPadActive = false;
        }
    }
}
