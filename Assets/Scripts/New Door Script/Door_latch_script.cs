using UnityEngine;
using System.Collections;

public class Door_latch_script : MonoBehaviour {

    public GameObject _lock;

    public bool hasLock()
    {
        if (_lock.GetComponent<Key_lock>().isActive())
            return true;
        else
            return false;
    }

    public void assignLock()
    {
        _lock.transform.parent.gameObject.SetActive(true);
    }

    public void unAssignLock()
    {
        _lock.transform.parent.gameObject.SetActive(false);
    }

}
