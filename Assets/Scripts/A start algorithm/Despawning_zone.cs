using UnityEngine;
using System.Collections;

public class Despawning_zone : MonoBehaviour {

    public GameObject entity;
    public GameObject player;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //Change this later for other conditions 
            if (entity.activeSelf)
            {
                entity.SetActive(false);
                print("Despawned");
            }
        }
    }

}
