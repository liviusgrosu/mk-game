using UnityEngine;
using System.Collections;

public class Perma_lock_script : MonoBehaviour {

    public bool locked;
    public GameObject idicator;
    public string idicatorAniName;
    private Animation idicatorAni;

    void Start()
    {
        idicatorAni = idicator.GetComponent<Animation>();
    }

    public void unlock()
    {
        //put animations for the key here
        if(idicator != null)
        {
            print("here m8");
            idicatorAni[idicatorAniName].speed = 2f;
            idicatorAni.Play();
        }
        locked = false;
        print("unlocked");
    }

    public void _lock()
    {
        if(idicator != null)
        {
            idicatorAni[idicatorAniName].time = idicatorAni[idicatorAniName].length;
            idicatorAni[idicatorAniName].speed = -2f;
            idicatorAni.Play();
        }

        locked = true;
        print("locked");
    }

    public bool isLocked()
    {
        return locked;
    }

}
