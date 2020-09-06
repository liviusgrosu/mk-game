using UnityEngine;
using System.Collections;

public class Other_door : MonoBehaviour {

    public bool isOpen;
    public string lock_mech;

    public GameObject parentDoor;

    public string aniName;
    Animation ani;

    // Use this for initialization
    void Start () {
        ani = transform.gameObject.GetComponent<Animation>();
	}

    public void interact()
    {
        switch (lock_mech)
        {
            case "door_latch":
                if (!parentDoor.transform.Find(lock_mech).GetComponent<Door_latch_script>().hasLock())
                {
                    print("toggle");
                    toggle();
                }
                else
                {
                    print("its locked");
                }
                break;
            case "perma_key_lock":
                if (!parentDoor.transform.Find(lock_mech).GetComponent<Perma_lock_script>().isLocked())
                {
                    print("toggle");
                    toggle();

                    if (parentDoor != null)
                        parentDoor.transform.Find("door").GetComponent<Door_V2>().toggle();
                }
                else
                {
                    print("its locked");
                }
                break;
            case "non_key_door_latch":
                if (!parentDoor.transform.Find(lock_mech).GetComponent<Non_key_latch_script>().hasLock())
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
}
