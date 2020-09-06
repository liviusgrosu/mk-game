using UnityEngine;
using System.Collections;

public class Flash_Light : MonoBehaviour {

    private float batteryLife;
    public bool startingState;
    private bool canPress = true;

	// Use this for initialization
	void Start () {
        batteryLife = 400.0f;
        GetComponent<Light>().enabled = startingState;

    }
	
	// Update is called once per frame
	void Update () {

        if(GetComponent<Light>().enabled)
            drain();

        if (!Input.GetButton("Flash Light"))
            canPress = true;

        if (Input.GetButton("Flash Light") && batteryLife > 0.0f && canPress)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            canPress = false;
        }
        else if(batteryLife <= 0.0f)
        {
            if (GetComponent<Light>().enabled)
                GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
        }

        //print("Battery life: " + batteryLife);
	}


    public void addbatteries(float amount)
    {
        batteryLife += amount;
    }

    public float getBatteryLife()
    {
        return batteryLife;
    }

    public void setBattery(float amount)
    {
        batteryLife = amount;
    }

    public void drain()
    {
        if (batteryLife > 0.0f)
            batteryLife -= Time.deltaTime;
        //print(batteryLife);

    }

}
