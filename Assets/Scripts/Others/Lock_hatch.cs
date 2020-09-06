using UnityEngine;
using System.Collections;

public class Lock_hatch : MonoBehaviour {

    public bool hasLock;
    public GameObject _lock;

    void Start()
    {
        _lock.GetComponent<Lock>().initMethod();

        //print("trigger");
        
        if (hasLock)
        {
            _lock.SetActive(true);
        }
        else
        {

            _lock.SetActive(false);
        }
    }

    public void setLock()
    {
        //print("set lock");
        if (!hasLock)
        {
            hasLock = true;
            _lock.SetActive(true);
            if (transform.gameObject.tag == "plank_hatch")
            {
                _lock.GetComponent<Lock>().changeState();
            }
            else
            {
                _lock.transform.Find("Padlock").GetComponent<Lock>().changeState();
            }
        }
    }

    public void unSetLock()
    {
        //print("unset lock");
        hasLock = false;
        _lock.SetActive(false);
    }

    public bool checkLock()
    {
        return hasLock;
    }


}
