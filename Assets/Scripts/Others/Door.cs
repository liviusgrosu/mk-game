using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    //public string doorLatchName;
    public string doorLockName;
    public GameObject doorLatch;

    public void openDoor()
    {
        //Transform door = parent.transform.Find("door_lock");

        //Transform door = transform.parent.transform.Find(doorLatchName);

        //print(doorLatchName);

        if(doorLatch != null)
        {
           // door.GetComponent<Lock_hatch>().checkLock();
            if (!doorLatch.GetComponent<Lock_hatch>().checkLock())
            {
                transform.GetComponent<Lock>().changeState();
                print("change door");
            }
            else
            {
                print("door is locked");
            }
        }
        //If the door does not have a door latch
        else
        {
            //Look for the lock itself within the parent object
            Transform _lock = transform.parent.transform.Find(doorLockName);

            //If there is a lock...
            if(_lock != null)
            {
                //and its not locked then unlock the door
                if(!_lock.GetComponent<Lock>().getLockedState())
                {
                    transform.GetComponent<Lock>().changeState();
                }
            }
            //If there isnt a lock then unlock the door
            else
                transform.GetComponent<Lock>().changeState();
            //print("change door");
        }
        //lockedState = !lockedState;

        //if (!lockedState)
        //{
        //    animation[animName].speed = 3f;
        //    animation.Play();

        //}
        //else
        //{
        //    animation[animName].speed = -3f;
        //    animation[animName].time = animation[animName].length;
        //    animation.Play();
        //}
    }
}
