using UnityEngine;
using System.Collections;

public class Ledge_Script : MonoBehaviour {

    public GameObject player;

    void OnTriggerEnter(Collider collision)
    {
        //If the player 'ledge' box collider interacts with the ledge
        if (collision.gameObject.tag == "Ledge")
        {
            ledge_action();
            Debug.Log("LELE");
        }
    }

    void ledge_action()
    {
        player.GetComponent<Player>().freezeCC();

    }
}
