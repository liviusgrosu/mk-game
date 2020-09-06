using UnityEngine;
using System.Collections;

public class water_fall_effect : MonoBehaviour {

    public GameObject water_fall_prefab;
    bool isCreated;

	// Use this for initialization
	void Start () {
        isCreated = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isCreated)
        {
            Instantiate(water_fall_prefab, transform.position, transform.rotation);
            isCreated = true;
        }
	}
}
