using UnityEngine;
using System.Collections;

public class Spawning_zone : MonoBehaviour {

    public GameObject monster;
    public GameObject player;

    Inventory inventory;

    public string condition;

    bool isCreated;
    //Player attributes for the prefab to spawn

    Vector3 playerPos;
    Vector3 playerDirection;
    Quaternion playerRotation;
    float spawnDistance;
    Vector3 spawnPos;

    void Start()
    {
        isCreated = false;
        spawnDistance = 10;
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            //Change this later for other conditions 
            if (!isCreated/* && condition == "bronze_key"*/)
            {
                if (inventory.InventoryContains(5))
                {
                monster.GetComponent<Finder>().activeToggle();
                    playerPos = player.transform.position;
                    playerDirection = player.transform.forward;
                    playerRotation = player.transform.rotation;

                    spawnPos = playerPos - playerDirection * spawnDistance;
                    spawnPos.y -= 0.8f;

                    monster.transform.position = spawnPos;
                    monster.transform.rotation = playerRotation;
                    //Instantiate(prefab, spawnPos, playerRotation);
                    Debug.Log("Created");
                    isCreated = !isCreated;
                }
            }
        }
    }


}   
