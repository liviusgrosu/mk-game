using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public int stamina;
    private bool exhausted;

    float fog1Setting = 0.016f;
    float fog2Setting = 0.1f;

    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    public GameObject startingPos;

    public GameObject Inventory;

    private float rotLeftRight;

    public float movementSpeed = 10f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 20.0f;

    private float devX;
    private float devY;
    private bool allowDev;

    public bool onLadder;
    public bool justGotOff;
    public float ladderSpeed;

    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    float verticalVelocity = 0;

    int sprint;
    float sprintGapTimer;
    bool enabledSprintOption;
    bool enableSprint;

    public AudioSource grassAudio;
    public AudioSource gravelAudio;
    public AudioSource woodAudio;
    bool isGrassPlaying;
    bool isGravelPlaying;
    bool isWoodPlaying;
    bool isMoving;
    string groundMaterial;

    bool ladderSpaceTrans;

    bool lost;
    bool freeze;
    //float ledge_displacement;

    float airForwardSpeed;
    float airSideSpeed;

    float forwardSpeed;
    float sideSpeed;

    float jumpCoolDown;
    bool hasJumped;

    Vector3 speed;

    float acc;

    CharacterController cc ;

    bool locked;

    public float shake = 10;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public GameObject monster1;
    public GameObject monster2;

    Vector3 orgCameraPos;

    bool walkingGrass;
    bool walkingPath;
    bool walkingWood;

    // Use this for initialization
    void Start()
    {
        orgCameraPos = Camera.main.transform.localPosition;
        isMoving = false;
        print("Camera pos: " + orgCameraPos);

        devX = 0;
        devY = 0;   

        //print("screen width: " + Screen.width + " | screen height: " + Screen.height);
        lost = false;
        onLadder = false; 
        Screen.lockCursor = true;
        cc = GetComponent<CharacterController>();

        jumpCoolDown = 0.0f;

        airForwardSpeed = 0.0f;
        airSideSpeed = 0.0f;

        forwardSpeed = 0.0f;
        sideSpeed = 0.0f;
        acc = 0.0f;

        sprint = 0;
        sprintGapTimer = 0.0f;

        enabledSprintOption = false;
        enableSprint = false;

        hasJumped = false;

        freeze = false;

        speed = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint1 = Camera.main.WorldToViewportPoint(monster1.transform.position);
        Vector3 screenPoint2 = Camera.main.WorldToViewportPoint(monster2.transform.position);
        if ((screenPoint1.z > 0 && screenPoint1.x > 0 && screenPoint1.x < 1 && screenPoint1.y > 0 && screenPoint1.y < 1 && monster1.activeSelf) || (screenPoint2.z > 0 && screenPoint2.x > 0 && screenPoint2.x < 1 && screenPoint2.y > 0 && screenPoint2.y < 1 && monster2.activeSelf))
        {
            RenderSettings.fogDensity = fog2Setting;
            Camera.main.transform.localPosition = Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            RenderSettings.fogDensity = fog1Setting;
            Camera.main.transform.localPosition = orgCameraPos;
        }


        /*if (shake > 0)
        {
            Camera.main.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;

        }*/
        /*if(Time.time > nextActionTime)
        {
            nextActionTime += period;
            stamina += 5;
            print("stamina: " + stamina);
        }

        if (stamina <= 0)
            exhausted = true;

        if (exhausted && stamina >= 20)
            exhausted = false;
        */

        locked = Inventory.GetComponent<Inventory>().isLocked;

        //If player chooses to hit the RUN key
        /*if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && !exhausted)
        {
            movement(movementSpeed * 1.5f, 1f);
            stamina -= 1;
        }

        else if(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && exhausted)
        {
            movement(movementSpeed * 1.5f, 1f);
        }

        //If player chooses to hit the SLOW WALK key
        else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
            movement(movementSpeed * 0.3f, 1f);
            */
        //If player chooses to WALK
        //if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
         movement(movementSpeed, 1f);


        //Debug.Log("Sprint option: " + enabledSprintOption + " | sprint: " + enableSprint);
    }

    //Pass through the parameters of the movement speed and walking side so
    void movement(float movementForwardSpeed, float movementSideSpeed)
    {
        // Rotation

        //Debug.Log(movementForwardSpeed);



        if (!allowDev)
            rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        else
        {
            rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity + devX;
            allowDev = false;
        }



        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (!allowDev)
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        else
        {
            verticalRotation = Mathf.Clamp(verticalRotation + devY, -upDownRange, upDownRange);
            allowDev = false;
        }

        if (locked)
        {
            transform.Rotate(0, rotLeftRight, 0);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        //If the character is GROUNDED
        if (cc.isGrounded)
        {
            if (!lost)
            {
                if (Input.GetKey("s"))
                    forwardSpeed = Input.GetAxis("Vertical") * movementForwardSpeed * acc * movementSideSpeed;
                else
                    forwardSpeed = Input.GetAxis("Vertical") * movementForwardSpeed * acc;

                sideSpeed = Input.GetAxis("Horizontal") * movementForwardSpeed * acc * movementSideSpeed;

                if ((Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f) && acc <= 1.0f)
                    acc += 0.025f;
                else if (Input.GetAxis("Vertical") == 0.0f && Input.GetAxis("Horizontal") == 0.0f)
                    acc = 0.0f;

                if (jumpCoolDown >= 0.0f)
                    jumpCoolDown -= Time.deltaTime;


                verticalVelocity = Physics.gravity.y;
                if (onLadder)
                {
                    //Freeze the player if the jump or walk up to the ladder
                    verticalVelocity = 0;
                    forwardSpeed = 0.0f;
                    sideSpeed = 0.0f;
                    airForwardSpeed = 0;
                    airSideSpeed = 0;

                    if (Input.GetAxis("Vertical") > 0.0f)
                        verticalVelocity = 1.0f;
                    if (Input.GetAxis("Vertical") < 0.0f)
                        verticalVelocity = -1.0f;
                    if (Input.GetButton("Jump") && !ladderSpaceTrans)
                    {
                        print("getting off");
                        onLadder = false;
                    }
                    if (Input.GetButtonUp("Jump"))
                    {
                        print("ready to jump off laddar");
                        ladderSpaceTrans = false;
                    }
                }
            }
            else
            {
                verticalVelocity = 0;
                forwardSpeed = 0.0f;
                sideSpeed = 0.0f;
                airForwardSpeed = 0;
                airSideSpeed = 0;
            }
        }
        //If the character is NOT GROUNDED 
        else
        {
            if (!lost)
            {
                //Code initializes jumping characteristics then after is turned off 
                if (hasJumped)
                {
                    airForwardSpeed = forwardSpeed;
                    airSideSpeed = sideSpeed;
                    forwardSpeed = 0.0f;
                    sideSpeed = 0.0f;
                    acc = 0.2f;
                }
                hasJumped = false;

                //if the player hits a ledge or ...
                if (freeze)
                {
                    verticalVelocity = 0;
                    airForwardSpeed = 0;
                    airSideSpeed = 0;

                    if (Input.GetKey("space"))
                    {
                        freeze = false;
                        verticalVelocity = jumpSpeed * 1.5f;
                        hasJumped = true;
                        jumpCoolDown = 0.2f;
                    }
                }
                else
                {
                    verticalVelocity += (Physics.gravity.y * 2.0f) * Time.deltaTime;

                    if (Input.GetKey("w") && airForwardSpeed == 0)
                        airForwardSpeed = 1.0f;
                    if (Input.GetKey("s") && airForwardSpeed == 0)
                        airForwardSpeed = -1.0f;
                    if (Input.GetKey("a") && airSideSpeed == 0)
                        airSideSpeed = -1.0f;
                    if (Input.GetKey("d") && airSideSpeed == 0)
                        airSideSpeed = 1.0f;

                    if (onLadder)
                    {
                        //Freeze the player if the jump or walk up to the ladder
                        verticalVelocity = 0;
                        forwardSpeed = 0.0f;
                        sideSpeed = 0.0f;
                        airForwardSpeed = 0;
                        airSideSpeed = 0;

                        if (Input.GetAxis("Vertical") > 0.0f)
                            verticalVelocity = 1.0f;
                        if (Input.GetAxis("Vertical") < 0.0f)
                            verticalVelocity = -1.0f;
                        if (Input.GetButton("Jump") && !ladderSpaceTrans)
                        {
                            print("getting off");
                            onLadder = false;
                        }
                        if(Input.GetButtonUp("Jump"))
                        {
                            print("ready to jump off laddar");
                            ladderSpaceTrans = false;
                        }
                    }
                }
            }
            else
            {
                verticalVelocity = 0;
                forwardSpeed = 0.0f;
                sideSpeed = 0.0f;
                airForwardSpeed = 0;
                airSideSpeed = 0;
            }
        }


        if (cc.isGrounded && Input.GetButton("Jump") && jumpCoolDown <= 0.0f)
        {
            ladderSpaceTrans = true;
            verticalVelocity = jumpSpeed;
            hasJumped = true;
            jumpCoolDown = 0.2f;
        }
        

        if (sideSpeed != 0.0f || forwardSpeed != 0.0f)
            isMoving = true;
        else
            isMoving = false;


        if (walkingWood)
        {
            if (isGravelPlaying)
            {
                gravelAudio.Stop();
                isGravelPlaying = false;
            }
            if(isGrassPlaying)
            {
                grassAudio.Stop();
                isGrassPlaying = false;
            }
            if (isMoving && !isWoodPlaying)
            {
                //print("walking");
                woodAudio.Play();
                isWoodPlaying = true;
                print("playing the wood sounds");
            }
            if (!isMoving)
            {
                //print("standing");
                woodAudio.Stop();
                isWoodPlaying = false;
            }
        }

        if (walkingGrass)
        {
            if(isGravelPlaying)
            {
                gravelAudio.Stop();
                isGravelPlaying = false;
            }
            if (isWoodPlaying)
            {
                woodAudio.Stop();
                isWoodPlaying = false;
            }
            if (isMoving && !isGrassPlaying)
            {
                //print("walking");
                grassAudio.Play();
                isGrassPlaying = true;
                print("playing the grass sounds");
            }
            if (!isMoving)
            {
                //print("standing");
                grassAudio.Stop();
                isGrassPlaying = false;
            }
        }

        if (walkingPath)
        {
            if (isGrassPlaying)
            {
                grassAudio.Stop();
                isGrassPlaying = false;
            }
            if (isWoodPlaying)
            {
                woodAudio.Stop();
                isWoodPlaying = false;
            }

            if (isMoving && !isGravelPlaying)
            {
                //print("walking");
                gravelAudio.Play();
                isGravelPlaying = true;
                print("playing the gravel sounds");
            }
            if (!isMoving)
            {
                //print("standing");
                gravelAudio.Stop();
                isGravelPlaying = false;
            }
        }
        print("isGravelPlaying: " + isGravelPlaying + " | isGrassPlaying: " + isGrassPlaying + " | isMoving: " + isMoving);
        //print("grass sound: " + isGravelPlaying + ", gravel sound: " + isGravelPlaying );
        //print("Are we moving: " + isMoving);

        if (cc.isGrounded)
            speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        else
            speed = new Vector3(airSideSpeed, verticalVelocity, airForwardSpeed);

        //Debug.Log("forward: " + forwardSpeed + " ,acc: " + acc + " , forwardSpeed: " + movementForwardSpeed + " ,Input: " + Input.GetAxis("Vertical"));

        speed = transform.rotation * speed;

        cc.Move(speed * Time.deltaTime);
    }

    public bool getJustGotOff()
    {
        return justGotOff;
    }

    public void changeJustGotOff(bool state)
    {
        justGotOff = state;
    }

    public bool isGrounded()
    {
        return cc.isGrounded;
    }

    public void freezeCC()
    {
        freeze = true;
    }

    public void unFreezeCC()
    {
        freeze = false;
    }

    public void ladderState(bool state)
    {
        onLadder = state;
    }

    public void loseAnimation(Transform monster)
    {
        transform.LookAt(monster);
        lost = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void assignDev(float x, float y)
    {
        devX = x;
        devY = y;
        allowDev = true;
    }

    public void OnTriggerStay(Collider collider)
    {
        groundMaterial = collider.gameObject.name;
        if (groundMaterial == "grass")
        {
            walkingGrass = true;
            walkingWood = false;
            walkingPath = false;
        }
        if (groundMaterial == "wood")
        {
            walkingGrass = false;
            walkingWood = true;
            walkingPath = false;
        }
        if (groundMaterial == "path")
        {
            walkingGrass = false;
            walkingWood = false;
            walkingPath = true;
        }

        /*//print("moving: " + isMoving);
        if (collider.gameObject.name == "grass")
        {
            if (isMoving && !isGrassPlaying)
            {
                //print("walking");
                grassAudio.Play();
                isGrassPlaying = true;
            }
            if(!isMoving)
            {
                //print("standing");
                grassAudio.Pause();
                isGrassPlaying = false;
            }
        }*/
    }
}
