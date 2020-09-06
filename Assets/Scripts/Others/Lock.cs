using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {

    public bool lockedState;
    public string animName;
    Animation animation;



    // Use this for initialization
    void Start()
    {
        animation = gameObject.GetComponent<Animation>();
    }


    public void initMethod()
    {
        animation = gameObject.GetComponent<Animation>();

        if (!lockedState && animation != null)
        {
            
            animation[animName].speed = 3f;
            animation.Play();
        }
        print("animation played");
    }

    public void changeState()
    {
        lockedState = !lockedState;

        //print(lockedState);

        if (animation != null)
        {
            if (!lockedState)
            {
                //print("unlocking");
                animation[animName].speed = 3f;
                animation.Play();

            }
            else
            {
                //print("locking");
    
                animation[animName].time = animation[animName].length;
                animation.Play();
            }
        }
    }

    public bool getLockedState()
    {
        return lockedState;
    }

}
