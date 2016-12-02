using UnityEngine;
using System.Collections;

public class GlobalGameManger : MonoBehaviour
{
    private AudioSource audiosource;
    public AudioClip[] clips;
    public Camera[] cameras;
    public Animator cartAnimator;
    public Animator DoorAnimator;
    public int CurrentPuzzle;
    public int NumberOfPuzzles;
    public GameObject[] puzzles;
    private Transform lift;
    private bool cartIsMoving;
    private bool cartIsInFront;
    private bool doorAnimationPlaying = true;
    private bool doorIsClosed = true;
    private bool cartLoadsInNewPuzzle;

    //Screen1 = red, green and blue buttons
    //Screen2 = keyboard(typewriter) + draaiknoppen
    //Screen3 = Joystick
    //-------------puzzle order-------------
    //0- Intro              -- Camera: None
    //1- Sterrenbeeld       -- Camera: None
    //2- Weegschaal         -- Camera: None
    //3- Alchemie           -- Camera: None
    //4- Rotatie            -- Camera: None
    //5- Stoom              -- Camera: 1
    //6- Tesla              -- Camera: none
    //7- Typemachine        -- Camera: 2
    //8- Phonautograph      -- Camera: 2
    //9- Gloeilamp          -- Camera: 1, 2 en 3
    //10- Venn diagram      -- Camera: 2
    //11- Laser rotatie     -- Camera: 1
    //12- Outro             -- Camera: None
    //-------------------------------------


    void Start ()
    {
        audiosource = GetComponent<AudioSource>();
        NumberOfPuzzles = 12;
        cameras = new Camera[3];
        for (int i = 0; i < 3; i++)
        {
            cameras[i] = GameObject.Find("CamerasPerspective").transform.GetChild(i).GetComponent<Camera>();
        }
        puzzles = new GameObject[13];
        puzzles[0] = GameObject.Find("Intro");
        puzzles[1] = GameObject.Find("Constellation");
        puzzles[2] = GameObject.Find("Keypad");
        puzzles[3] = GameObject.Find("Alchemy");
        puzzles[4] = GameObject.Find("Rotation");
        puzzles[5] = GameObject.Find("Steam");
        puzzles[6] = GameObject.Find("Magnets");
        puzzles[7] = GameObject.Find("Cryptography");
        puzzles[8] = GameObject.Find("Xylophone");
        puzzles[9] = GameObject.Find("Lamp");
        puzzles[10] = GameObject.Find("Cryptex");
        puzzles[11] = GameObject.Find("Lazer");
        puzzles[12] = GameObject.Find("Outro");

        lift = GameObject.Find("Environment").transform.FindChild("CartBox");
        cartAnimator = GameObject.Find("Environment").GetComponent<Animator>();
        DoorAnimator = GameObject.Find("Door").GetComponent<Animator>();
        cartAnimator.SetFloat("Speed", 0);
        DoorAnimator.SetFloat("Speed", 0);
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].SetActive(false);
        }
        audiosource.clip = clips[0];
        audiosource.Play();
    }	

	void Update ()
    {
        if (!cartIsMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CurrentPuzzle > 0)
            {
                CurrentPuzzle--;
                LoadNewPuzzle();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && CurrentPuzzle < NumberOfPuzzles)
            {
                CurrentPuzzle++;
                LoadNewPuzzle();
            }
        }

        if (doorAnimationPlaying)
        {
            if (doorIsClosed)
            {
                if (DoorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                {
                    DoorAnimator.SetFloat("Speed", 1);
                }
                else
                {
                    DoorAnimator.SetFloat("Speed", 0);
                }
            }
            else
            {
                if (DoorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0)
                {
                    DoorAnimator.SetFloat("Speed", -1);
                }
                else
                {
                    //move the cart to front
                    DoorAnimator.SetFloat("Speed", 0);
                    doorAnimationPlaying = false;
                    cartIsMoving = true;
                    cartIsInFront = true;
                    audiosource.clip = clips[2];
                    audiosource.Play();
                    ActivateCurrentPuzzle();
                }
            }
        }

        if (cartIsMoving)
        {
            if (cartIsInFront)
            {
                if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                {
                    cartAnimator.SetFloat("Speed", 1);
                }
                else
                {
                    cartAnimator.SetFloat("Speed", 0);
                    cartIsMoving = false;
                }
            }
            else
            {
                if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0)
                {
                    cartAnimator.SetFloat("Speed", -1);
                }
                else
                {
                    cartAnimator.SetFloat("Speed", 0);
                    ActivateCurrentPuzzle();                    
                }
            }
        }       
    }

    void ActivateCurrentPuzzle()
    {
        if (cartLoadsInNewPuzzle)
        {
            cartIsInFront = true;
        }
        else
        {
            audiosource.clip = clips[0];
            audiosource.Play();
            cartIsMoving = false;
            doorAnimationPlaying = true;
            doorIsClosed = true;
        }
        for (int i = 0; i < puzzles.Length; i++)
        {
            if (i == CurrentPuzzle)
            {
                puzzles[i].SetActive(true);
                if (cartLoadsInNewPuzzle) puzzles[i].transform.SetParent(lift);
            }
            else
            {
                puzzles[i].SetActive(false);
                puzzles[i].transform.parent = null;
            }
        }
    }
    
    void LoadNewPuzzle()
    {
        //-------------puzzle order-------------
        //0- Intro              -- Camera: None
        //1- Sterrenbeeld       -- Camera: None
        //2- Weegschaal         -- Camera: None
        //3- Alchemie           -- Camera: None
        //4- Rotatie            -- Camera: None
        //5- Stoom              -- Camera: 1
        //6- Tesla              -- Camera: none
        //7- Typemachine        -- Camera: 2
        //8- Phonautograph      -- Camera: 2
        //9- Gloeilamp          -- Camera: 1, 2 en 3
        //10- Venn diagram      -- Camera: 2
        //11- Laser rotatie     -- Camera: 1
        //12- Outro             -- Camera: None
        //-------------------------------------
        
        if (CurrentPuzzle == 0 || CurrentPuzzle == 1 || CurrentPuzzle == 2 || CurrentPuzzle == 3 || CurrentPuzzle == 4 || CurrentPuzzle == 6 || CurrentPuzzle == 12) //no screens
        {
            Debug.Log("Puzzle selected: " + CurrentPuzzle + ", the door is closed!");
            if (cartIsInFront)
            {
                audiosource.clip = clips[3];
                audiosource.Play();
                cartIsInFront = false;
                cartIsMoving = true;
                cartLoadsInNewPuzzle = false;
            }
        }
        if (CurrentPuzzle == 5 || CurrentPuzzle == 9 || CurrentPuzzle == 11) //screen 1 active
        {
            Debug.Log("Puzzle selected: " + CurrentPuzzle + ", this puzzle is displayed on screen.");
            if (doorIsClosed)
            {
                audiosource.clip = clips[1];
                audiosource.Play();
                doorAnimationPlaying = true;
                doorIsClosed = false;
            }
            else
            {
                audiosource.clip = clips[3];
                audiosource.Play();
                cartIsInFront = false;
                cartIsMoving = true;
            }
            cartLoadsInNewPuzzle = true;
        }
        if (CurrentPuzzle == 7 || CurrentPuzzle == 8 || CurrentPuzzle == 9 || CurrentPuzzle == 10) //screen 2 active
        {
            Debug.Log("Puzzle selected: " + CurrentPuzzle + ", this puzzle is displayed on screen.");
            if (doorIsClosed)
            {
                audiosource.clip = clips[1];
                audiosource.Play();
                doorAnimationPlaying = true;
                doorIsClosed = false;
            }
            else
            {
                audiosource.clip = clips[3];
                audiosource.Play();
                cartIsInFront = false;
                cartIsMoving = true;
            }
            cartLoadsInNewPuzzle = true;            
        }
        if (CurrentPuzzle == 9) //screen 3 active
        {
            Debug.Log("Puzzle selected: " + CurrentPuzzle + ", this puzzle is displayed on screen.");
            if (doorIsClosed)
            {
                audiosource.clip = clips[1];
                audiosource.Play();
                doorAnimationPlaying = true;
                doorIsClosed = false;
            }
            else
            {
                audiosource.clip = clips[3];
                audiosource.Play();
                cartIsInFront = false;
                cartIsMoving = true;
            }
            cartLoadsInNewPuzzle = true;
        }
    }
}