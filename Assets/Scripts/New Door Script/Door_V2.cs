using UnityEngine;
using System.Collections;

public class Door_V2 : MonoBehaviour {

    public bool isOpen;
    public string lock_mech;

    bool audioPlayed;

    //Optional
    //Only used for double door
    public GameObject otherDoor;

    //Duration that a door can take before it gets opened by brute force 
    float weaknessTime = 30.0f;

    public string aniName;
    Animation ani;

    public void Start()
    {
        audioPlayed = false;

        if (lock_mech == "perma_key_lock" || lock_mech == "door_latch")
            ani = transform.parent.gameObject.GetComponent<Animation>();
        else
            ani = gameObject.GetComponent<Animation>();
    }

    //toggle the door to open or close 
    public void interact()
    {
        //reference the parent 
        Transform parent = transform.parent.transform;


        switch(lock_mech)
        {
            //A door with a lock that you can take with you
            case "door_latch":
                if(!parent.Find(lock_mech).GetComponent<Door_latch_script>().hasLock())
                {
                    print("toggle");
                    toggle();
                }
                else
                {
                    print("its locked");
                }
                break;
            //Your standard door
            case "perma_key_lock":
                if(!parent.Find(lock_mech).GetComponent<Perma_lock_script>().isLocked())
                {
                    print("toggle");
                    toggle();

                    if (otherDoor != null)
                        otherDoor.GetComponent<Other_door>().toggle();
                }
                else
                {
                    print("its locked");
                }
                break;
            //EG: Door with a plank to barge it
            case "non_key_door_latch":
                if (!parent.Find(lock_mech).GetComponent<Non_key_latch_script>().hasLock())
                {
                    print("toggle");
                    toggle();
                }
                else
                {
                    print("its locked");
                }
                break;

            default:
                print("toggle");
                toggle();
                break;
            
        }
    }

    public void toggle()
    {
        print("Open");
        if (isOpen)
        {
            ani[aniName].time = ani[aniName].length;
            ani[aniName].speed = -3f;
            ani.Play();
        }
        else
        {
            ani[aniName].speed = 3f;
            ani.Play();
        }
        isOpen = !isOpen;

    }

    public bool openStatus()
    {
        return isOpen;
    }

    public bool hasAudioPlayed()
    {
        return audioPlayed;
    }

    public void audioPlayedState(bool state)
    {
        audioPlayed = state;
    }
}
