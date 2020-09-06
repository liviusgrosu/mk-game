using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

    GameObject playerObj;

	// Use this for initialization
	void Start () {

	}
	
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("collision with the player");
            playerObj = col.gameObject;
            playerObj.GetComponent<Player>().ladderState(true);
            print("collision");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Getting off the ladder");
            //playerObj.GetComponent<Rigidbody>().MovePosition(playerObj.transform.position + transform.up * 4.0f);
            playerObj.transform.Translate(Vector3.forward * Time.deltaTime * 50);
            //playerObj.GetComponent<CharacterController>().slopeLimit = 45;
            playerObj.GetComponent<Player>().ladderState(false);
            playerObj = null;
        }
    }
}
