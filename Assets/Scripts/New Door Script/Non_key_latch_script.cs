using UnityEngine;
using System.Collections;

public class Non_key_latch_script : MonoBehaviour {


    public GameObject _lock;

    public bool hasLock()
    {
        if (_lock.GetComponent<Non_key_lock>().isLocked())
            return true;
        else
            return false;
    }

    public void assignLock()
    {
        print("assigning lock");
        _lock.SetActive(true);
    }

    public void unAssignLock()
    {
        print("unassigning lock");
        _lock.SetActive(false);
    }
}
