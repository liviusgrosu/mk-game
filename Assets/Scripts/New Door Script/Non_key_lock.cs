using UnityEngine;
using System.Collections;

public class Non_key_lock : MonoBehaviour {

    public bool active;
    public bool locked;
    public string aniName;
    Animation ani;

    void Start()
    {
        ani = gameObject.GetComponent<Animation>();

        if (active)
        {
            locked = true;
            gameObject.SetActive(true);
        }
        else
        {
            locked = false;
            gameObject.SetActive(false);
        }
    }

    public bool isActive()
    {
        return gameObject.activeSelf;
    }

    public void unlock()
    {
        //put animations for the key here
        locked = false;
        print("unlocked");
    }

    public void _lock()
    {
        locked = true;
        print("locked");
    }

    public bool isLocked()
    {
        return locked;
    }

}
