using UnityEngine;
using System.Collections;

public class Finder : MonoBehaviour {

    public SphereCollider playerCollider;

    public Transform destinationPoint;

    public Light lightGlow;

    Vector3 standingPos;

    float weaknessTime;

    bool gameOver;
    bool isActive;
    bool busy;

	// Use this for initialization
	void Start () {
        lightGlow.enabled = false;
        gameOver = false;
        weaknessTime = 5.0f;
        busy = false;
        isActive = false;
        transform.gameObject.SetActive(isActive);
	}
	
	// Update is called once per frame
	void Update () {

        if (isActive && !busy)
        {
            standingPos = transform.position;
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = destinationPoint.position;
        }
        if(busy)
        {
            weaknessTime -= Time.deltaTime;
            transform.position = standingPos;
        }
	}

    public void activeToggle()
    {
        isActive = true;
        transform.gameObject.SetActive(isActive);
        lightGlow.enabled = true;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "door" && !gameOver)
        {
            print("monster collides with door");
            if (!collision.gameObject.GetComponent<Door_V2>().openStatus())
            {
                print("attempting");

                busy = true;

                standingPos = transform.position;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }   
        }
        if (collision.gameObject.tag == "Player")
        {
            gameOver = true;
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0.0f;
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = 0.0f;
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().angularSpeed = 0.0f;
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = standingPos;
            collision.gameObject.GetComponent<Player>().loseAnimation(transform);
            //collision.gameObject.GetComponent<Player>().restartGame();
            GameObject.Find("Game over object").GetComponent<game_over>().gameState(true);
            print("You lose");
        }
    }

    void OnTriggerStay(Collider collision)
    {

        if (collision.gameObject.tag == "door")
        {
            print(weaknessTime);

            if (collision.gameObject.GetComponent<Door_V2>().openStatus() || weaknessTime <= 0.0f)
            {
                if (!collision.gameObject.GetComponent<Door_V2>().openStatus())
                {
                    if (collision.gameObject.GetComponent<Door_V2>().lock_mech == "non_key_door_latch")
                        collision.gameObject.GetComponent<Door_V2>().interact();
                    else
                        collision.gameObject.GetComponent<Door_V2>().toggle();
                }

                if (collision.gameObject.GetComponent<Door_V2>().openStatus())
                {
                    GetComponent<Rigidbody>().angularVelocity = new Vector3();
                    GetComponent<Rigidbody>().velocity = new Vector3();
                    //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    //GetComponent<Rigidbody>().isKinematic = true;

                    busy = false;
                }

                weaknessTime = 5.0f;


            }
        }
    }

}
