using UnityEngine;
using System.Collections;

public class Key_lock : MonoBehaviour {

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
            transform.parent.gameObject.SetActive(true);
        }
        else
        {
            locked = false;
            transform.parent.gameObject.SetActive(false);
        }
    }

    public bool isActive()
    {
        return transform.parent.gameObject.activeSelf;
    }

    public void unlock()
    {
        //put animations for the key here
        locked = false;

        ani[aniName].speed = 3f;
        ani.Play();

        print("unlocked");
    }

    public void _lock()
    {
        locked = true;

        ani[aniName].time = ani[aniName].length;
        ani[aniName].speed = -3f;
        ani.Play();

        print("locked");
    }

    public bool isLocked()
    {
        return locked;
    }

}
