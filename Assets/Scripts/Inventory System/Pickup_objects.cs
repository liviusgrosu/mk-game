using UnityEngine;
using System.Collections;

public class Pickup_objects : MonoBehaviour {

    Camera mainCamera;
    Inventory inventory;
    public GameObject path;
    AudioSource audio;

    public AudioSource paperPickUpAudio;
    public AudioSource paperPutBackAudio;

    bool puPlayed = false;
    bool pbPlayer = false;

    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    private int pageViewNumber;
    private Rect pageRect;
    public Texture[] pageTextures;

    public GameObject[] flames;

    private bool keyPickedUp;

    bool lookingAtItem = false;

    public Texture interactButton;
    private Rect interactRect;

    bool keyIsDown = false;


    // Use this for initialization
    void Start () {
        pageRect = new Rect(Screen.width / 2 - 512, Screen.height / 2 - 512, 1024, 1024);
        interactRect = new Rect(Screen.width / 2 - interactButton.width * 1.3f / 2, Screen.height / 2, interactButton.width * 1.3f, interactButton.height);
        pageViewNumber = 0;
        mainCamera = Camera.main;
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
	}

    // Update is called once per frame
    void Update() {

        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6f))
        {
            if (hit.collider.tag == "Item")
            {
                lookingAtItem = true;
            }
            else
                lookingAtItem = false;
        }

        if (Input.GetButton("Interact"))
        {
            PickUp();
        }

        if (!Input.GetButton("Interact"))
            keyIsDown = false;

    }

    void OnGUI()
    {

        Event e = Event.current;

        if (lookingAtItem)
        {
            GUI.DrawTexture(interactRect, interactButton);
        }

        if(Input.GetButton("Interact") && keyIsDown == false && !GameObject.Find("Inventory").GetComponent<Inventory>().inventoryOpen())
        {
            pageViewNumber = 0;
            keyIsDown = true;
            lookingAtItem = false;
        }

        if(pageViewNumber != 0 && !puPlayed)
        {
            paperPickUpAudio.Play();
            puPlayed = true;
            pbPlayer = false;
        }
        else if(pageViewNumber == 0 && !pbPlayer)
        {
            paperPutBackAudio.Play();
            puPlayed = false;
            pbPlayer = true;
        }

        if (pageViewNumber == 1)
            GUI.DrawTexture(pageRect, pageTextures[0]);
        else if (pageViewNumber == 2)
            GUI.DrawTexture(pageRect, pageTextures[1]);
        else if (pageViewNumber == 3)
            GUI.DrawTexture(pageRect, pageTextures[2]);
        else if (pageViewNumber == 4)
            GUI.DrawTexture(pageRect, pageTextures[3]);
        else if (pageViewNumber == 5)
            GUI.DrawTexture(pageRect, pageTextures[4]);
        else if (pageViewNumber == 6)
            GUI.DrawTexture(pageRect, pageTextures[5]);
        else if (pageViewNumber == 7)
            GUI.DrawTexture(pageRect, pageTextures[6]);
        else if (pageViewNumber == 8)
            GUI.DrawTexture(pageRect, pageTextures[7]);
        else if (pageViewNumber == 9)
            GUI.DrawTexture(pageRect, pageTextures[8]);
        else if (pageViewNumber == 10)
            GUI.DrawTexture(pageRect, pageTextures[9]);


    }

    void PickUp()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 6f))
        {


            //print("object: " + hit.collider.name);
            switch(hit.collider.name)
            {
                case "chs":
                    //Debug.Log("CHS item added");
                    inventory.AddItem(2);
                    Destroy(hit.collider.gameObject);
                    break;

                case "fhs":
                    //Debug.Log("FHS item added");
                    inventory.AddItem(1);
                    Destroy(hit.collider.gameObject);
                    break;

                //case "lock":
                //    if (!hit.collider.gameObject.GetComponent<Lock>().getLockedState())
                //    {
                //        //Debug.Log("lock item added");
                //        inventory.AddItem(3);
                //        //hit.collider.transform.parent.parent.GetComponent<Lock_hatch>().unSetLock();
                //        hit.collider.transform.parent.parent.transform.Find("door_lock").GetComponent<Lock_hatch>().unSetLock();
                //    }
                //    break;

                //case "plank_lock":
                //    print("detect lock");
                //    if (!hit.collider.gameObject.GetComponent<Lock>().getLockedState())
                //    {
                //        hit.collider.transform.parent.transform.Find("shack_door_latch").GetComponent<Lock_hatch>().setLock();
                //    }
                //    else
                //    {
                //        GameObject _lock = hit.collider.gameObject;
                //        if (_lock.GetComponent<Lock>().getLockedState())
                //            _lock.GetComponent<Lock>().changeState();

                //        hit.collider.transform.parent.transform.Find("shack_door_latch").GetComponent<Lock_hatch>().unSetLock();
                //        inventory.AddItem(4);
                //    }
                //    break;

                case "plank":
                    inventory.AddItem(4);
                    Destroy(hit.collider.gameObject);
                    break;

                case "door":
                    hit.collider.gameObject.GetComponent<Door_V2>().interact();
                    if(!hit.collider.gameObject.GetComponent<Door_V2>().hasAudioPlayed())
                    {
                        audio = hit.collider.gameObject.GetComponent<AudioSource>();
                        audio.Play();
                        hit.collider.gameObject.GetComponent<Door_V2>().audioPlayedState(true);
                    }
                    break;

                case "other_door":
                    hit.collider.gameObject.GetComponent<Other_door>().interact();
                    break;

                case "bronze_key":
                    inventory.AddItem(5);
                    Destroy(hit.collider.gameObject);
                    keyPickedUp = true;
                    path.SetActive(false);
                    lightsOut();
                    break;

                case "padlock_body":
                    if (!hit.collider.GetComponent<Key_lock>().isLocked())
                    {
                        inventory.AddItem(3);
                        hit.collider.transform.parent.parent.Find("door_latch").GetComponent<Door_latch_script>().unAssignLock();
                    }
                    break;

                case "non_key_plank":
                    hit.collider.GetComponent<Non_key_lock>().unlock();
                    inventory.AddItem(4);
                    hit.collider.transform.parent.Find("non_key_door_latch").GetComponent<Non_key_latch_script>().unAssignLock();
                    break;

                case "batteries":
                    inventory.AddItem(6);
                    Destroy(hit.collider.gameObject);
                    break;

                case "map":
                    inventory.AddItem(8);
                    audio = hit.collider.gameObject.GetComponent<AudioSource>();
                    audio.Play();
                    Destroy(hit.collider.gameObject.GetComponent<MeshRenderer>());
                    Destroy(hit.collider.gameObject.GetComponent<BoxCollider>());
                    break;

                case "knife":
                    inventory.AddItem(7);
                    Destroy(hit.collider.gameObject);
                    break;

                case "page_1":
                    pageViewNumber = 1;
                    if(!keyPickedUp)
                        flames[0].SetActive(true);
                    break;
                case "page_2":
                    pageViewNumber = 2;
                    if (!keyPickedUp)
                    {
                        flames[0].SetActive(false);
                        flames[1].SetActive(true);
                    }
                    break;
                case "page_3":
                    pageViewNumber = 3;
                    if (!keyPickedUp)
                    {
                        flames[1].SetActive(false);
                        flames[2].SetActive(true);
                    }
                    break;
                case "page_4":
                    pageViewNumber = 4;
                    if (!keyPickedUp)
                    {
                        flames[2].SetActive(false);
                        flames[3].SetActive(true);
                    }

                    break;
                case "page_5":
                    pageViewNumber = 5;
                    if (!keyPickedUp)
                    {
                        flames[3].SetActive(false);
                        flames[4].SetActive(true);
                    }
                    break;
                case "page_6":
                    pageViewNumber = 6;
                    if (!keyPickedUp)
                    {
                        flames[4].SetActive(false);
                        flames[5].SetActive(true);
                        flames[6].SetActive(true);
                        flames[7].SetActive(true);
                        flames[11].SetActive(true);
                        flames[12].SetActive(true);
                    }
                    break;
                case "page_dont":
                    pageViewNumber = 7;
                    if (!keyPickedUp)
                    {
                        flames[11].SetActive(false);
                        flames[12].SetActive(false);
                        flames[5].SetActive(false);
                        flames[6].SetActive(false);
                        flames[8].SetActive(true);
                    }
                    break;
                case "page_cut":
                    pageViewNumber = 8;
                    if (!keyPickedUp)
                        flames[9].SetActive(true);
                    break;
                case "page_the":
                    pageViewNumber = 9;
                    if (!keyPickedUp)
                        flames[10].SetActive(true);
                    break;
                case "page_rope":
                    pageViewNumber = 10;
                    break;

                default:
                    break;
            }
        }
    }

    public void lightsOut()
    {
        for (int i = 0; i < flames.Length; i++)
            flames[i].SetActive(false);
    }
}
