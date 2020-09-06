using System;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_script : MonoBehaviour {

    public Sprite[] spriteArr;
    public Camera mainCamera;
    private GameObject holding_item;
    float holding_item_size;
    public GameObject player;
    private bool attemptHolding;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteArr[0];

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(getMouseHoverObject(5));
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6f) && holding_item == null)
        {
            if (hit.collider.tag == "Item")
            {
                Event eve = Event.current;

                //gameObject.GetComponent<SpriteRenderer>().sprite = spriteArr[2];


                //If we are left clicking
                //if (Input.GetMouseButtonDown(0))
                //{
                //    holding_item = hit.collider.gameObject;
                //    TryGrabObject(holding_item);
                //}
            }
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteArr[0];
        }
        /*if (holding_item != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * holding_item_size;
            holding_item.transform.position = newPosition;

            if (Input.GetMouseButtonUp(0))
                attemptHolding = false;

            if (Input.GetMouseButtonDown(0) && !attemptHolding)
                DropObject(holding_item);
        }*/
    }

    /*GameObject getMouseHoverObject(float range)
    {
        Vector3 position = player.transform.position;
        RaycastHit raycastHit;
        Vector3 target = position + Camera.main.transform.forward * range;

        if(Physics.Linecast(position, target, out raycastHit))
        {
            return raycastHit.collider.gameObject;
        }
        return null;
    }

    void TryGrabObject(GameObject grabObject)
    {
        if(grabObject == null || grabObject.tag != "Item")
        {
            return;
        }
        holding_item = grabObject;
        holding_item_size = grabObject.GetComponent<Renderer>().bounds.size.magnitude;

        if(grabObject.GetComponent<Rigidbody>() != null)
        {
            grabObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            grabObject.transform.parent.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }

        attemptHolding = true;
        
    }

    void DropObject(GameObject grabObject)
    {
        if (holding_item == null || grabObject.tag != "Item")
        {
            return;
        }

        if (grabObject.GetComponent<Rigidbody>() != null)
        {
            grabObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            grabObject.transform.parent.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        holding_item = null;
    }*/
}
