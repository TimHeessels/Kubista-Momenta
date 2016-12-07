using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalGameManger : MonoBehaviour
{
    private AudioSource audiosource;
    public AudioClip[] clips;
    public Camera[] cameras;
    public Animator cartAnimator;
    public Animator[] DoorAnimators;
    public int CurrentPuzzle;
    public int NumberOfPuzzles;
    public GameObject[] puzzles;
    private Transform lift;
    private bool[,] puzzleIsDisplayedOnScreen;
    public Text[] betaTexts;

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

    private static GlobalGameManger instance = null;

    // Game Instance Singleton
    public static GlobalGameManger Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        puzzleIsDisplayedOnScreen = new bool[12,3];
        audiosource = GetComponent<AudioSource>();
        NumberOfPuzzles = 12;
        cameras = new Camera[3];
        for (int i = 0; i < 3; i++)
        {
            cameras[i] = GameObject.Find("CamerasPerspective").transform.GetChild(i).GetComponent<Camera>();
        }
        puzzles = new GameObject[13];
        puzzles[0] = GameObject.Find("00_Intro");
        puzzles[1] = GameObject.Find("01_Constellation");
        puzzles[2] = GameObject.Find("02_Keypad");
        puzzles[3] = GameObject.Find("03_Alchemy");
        puzzles[4] = GameObject.Find("04_Rotation");
        puzzles[5] = GameObject.Find("05_Steam");
        puzzles[6] = GameObject.Find("06_Magnets");
        puzzles[7] = GameObject.Find("07_Cryptography");
        puzzles[8] = GameObject.Find("08_Xylophone");
        puzzles[9] = GameObject.Find("09_Lamp");
        puzzles[10] = GameObject.Find("10_Cryptex");
        puzzles[11] = GameObject.Find("11_Lazer");
        puzzles[12] = GameObject.Find("12_Outro");

        lift = GameObject.Find("Environment").transform.FindChild("CartBox");
        cartAnimator = GameObject.Find("Environment").GetComponent<Animator>();
        DoorAnimators = new Animator[3];
        DoorAnimators[0] = GameObject.Find("DoorScreen1").GetComponent<Animator>();
        DoorAnimators[1] = GameObject.Find("DoorScreen2").GetComponent<Animator>();
        DoorAnimators[2] = GameObject.Find("DoorScreen3").GetComponent<Animator>();
        DoorAnimators[0].SetFloat("Speed", 1);
        DoorAnimators[1].SetFloat("Speed", 1);
        DoorAnimators[2].SetFloat("Speed", 1);
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].SetActive(false);
        }
        puzzles[0].SetActive(true);
        audiosource.clip = clips[0];
        audiosource.Play();

        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                puzzleIsDisplayedOnScreen[i, j] = false;
                // i = puzzle number
                // j = has screen?
            }
        }
        puzzleIsDisplayedOnScreen[5, 0] = true;
        puzzleIsDisplayedOnScreen[7, 1] = true;
        puzzleIsDisplayedOnScreen[8, 1] = true;
        puzzleIsDisplayedOnScreen[9, 0] = true;
        puzzleIsDisplayedOnScreen[9, 1] = true;
        puzzleIsDisplayedOnScreen[9, 2] = true;
        puzzleIsDisplayedOnScreen[10, 1] = true;
        puzzleIsDisplayedOnScreen[11, 0] = true;
        UpdateBetaTexts();

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CurrentPuzzle < NumberOfPuzzles)
        {
            StartCoroutine(ChangePuzzle());
        }

        //Stop the animation after completing, otherwise the animation would continue without displaying causing bugs
        if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && cartAnimator.GetFloat("Speed") == 1) cartAnimator.SetFloat("Speed", 0);
        if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && cartAnimator.GetFloat("Speed") == -1) cartAnimator.SetFloat("Speed", 0);
        for (int i = 0; i < 3; i++)
        {
            if (DoorAnimators[i].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && DoorAnimators[i].GetFloat("Speed") == 1) DoorAnimators[i].SetFloat("Speed", 0);
            if (DoorAnimators[i].GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && DoorAnimators[i].GetFloat("Speed") == -1) DoorAnimators[i].SetFloat("Speed", 0);
        }
    }

    IEnumerator ChangePuzzle()
    {
        for (int i = 0; i < 3; i++)
        {
            betaTexts[i].text = "Arcanum BETA V0.1 - Loading new puzzle. . .";
        }
        //CurrentPuzzle++;
        CurrentPuzzle = 11;
        if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //if cart is in front
        {
            audiosource.clip = clips[3];
            audiosource.Play();
            cartAnimator.SetFloat("Speed", -1); //move the cart to the back
            yield return new WaitForSeconds(5f); //wait for the cart to move to the back
        }
        bool waitForDoorsToOpen = false; //a bool to determine if we need to wait for the doors to open or not (if all doors are allready open)
        for (int i = 0; i < 3; i++)
        {
            if (DoorAnimators[i].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && puzzleIsDisplayedOnScreen[CurrentPuzzle, i]) //check if doors are closed and need to be open for next puzzle
            {
                //the 1st door is closed but is needed to be open for next puzzle
                DoorAnimators[i].SetFloat("Speed", -1); //open the doors that need to be open
                waitForDoorsToOpen = true; //we must wait for the door to open
            }
        }
        for (int i = 0; i < puzzles.Length; i++) //loop through all puzzles
        {
            if (i == CurrentPuzzle) //if the  loop finds the correct puzzle, enable it and set is as a child of the lift
            {
                puzzles[i].SetActive(true);
                puzzles[i].transform.SetParent(lift);
            }
            else//unparent the rest and disable them
            {
                puzzles[i].SetActive(false); 
                puzzles[i].transform.parent = null;
            }
        }
        if (waitForDoorsToOpen)
        {
            audiosource.clip = clips[0];
            audiosource.Play();
            yield return new WaitForSeconds(3f); //wait for the doors to open
        }
        waitForDoorsToOpen = false; //reset the bool
        for (int i = 0; i < 3; i++) //loop through the three doors
        {
            if (DoorAnimators[i].GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && !puzzleIsDisplayedOnScreen[CurrentPuzzle, i]) //check if doors are open and need to be closed for next puzzle
            {
                //the 1st door is open but is needed to be closed for next puzzle
                DoorAnimators[i].SetFloat("Speed", 1); //close the doors that need to be closed
                waitForDoorsToOpen = true;
            }
        }
        if (waitForDoorsToOpen)
        {
            audiosource.clip = clips[1];
            audiosource.Play();
            yield return new WaitForSeconds(3f); //wait for the doors to open
        }
        if (cartAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && (Mathf.Round(DoorAnimators[0].GetCurrentAnimatorStateInfo(0).normalizedTime) <= 0 || Mathf.Round(DoorAnimators[1].GetCurrentAnimatorStateInfo(0).normalizedTime) <= 0 || Mathf.Round(DoorAnimators[2].GetCurrentAnimatorStateInfo(0).normalizedTime) <= 0)) //if cart is in the back and at least one door is open
        {
            audiosource.clip = clips[2];
            audiosource.Play();
            cartAnimator.SetFloat("Speed", 1); //move the cart back to the front
        }
        if (waitForDoorsToOpen) yield return new WaitForSeconds(5f); //wait for the cart to arrive        
        UpdateBetaTexts();//beta text update
    }

    void UpdateBetaTexts()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CurrentPuzzle == 0) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Intro";
            if (CurrentPuzzle == 1) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Constellation puzzle (trace the stars)";
            if (CurrentPuzzle == 2) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Scale puzzle (calculate the correct weights)";
            if (CurrentPuzzle == 3) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Alchemy puzzle (connect the right elements)";
            if (CurrentPuzzle == 4) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Rotation puzzle (rotate the circle to view the screen)";
            if (CurrentPuzzle == 5) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Steam puzzle (rotate the pipes to connect steam)";
            if (CurrentPuzzle == 6) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Tesla puzzle (transport the ball to the top using magnets)";
            if (CurrentPuzzle == 7) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Typewriter puzzle (decypher the code text)";
            if (CurrentPuzzle == 8) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Phonautograph (figure out the secret music code)";
            if (CurrentPuzzle == 9) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Lightbulb (work together to get to the finish)";
            if (CurrentPuzzle == 10) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Venn diagram (use the diagram to enter the secret code)";
            if (CurrentPuzzle == 11) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Laser puzzle (rotate the blocks to align the lazer)";
            if (CurrentPuzzle == 12) betaTexts[i].text = "Arcanum BETA V0.1 - Screen #" + i + ", Outro";
        }
    }  
}