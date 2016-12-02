using UnityEngine;
using System.Collections;

public class LazerGameManager : MonoBehaviour
{
    public Transform lazer1Transform;
    public Transform lazer2Transform;
    public Transform lazer3Transform;
    public Transform targetTransform;
    private RaycastHit hit;

    private LineRenderer lazer1LineRenderer;
    private LineRenderer lazer2LineRenderer;
    private LineRenderer lazer3LineRenderer;
    private LineRenderer targetlineRenderer;

    
    public ParticleSystem lazer1Particles;
    public ParticleSystem lazer2Particles;
    public ParticleSystem lazer3Particles;

    public Transform[] rings;
    private Animation[] ringAnimations;
    public Animation[] lazerAnimations;

    private float[] rotation;
    private float[] puzzleOffset;

    private bool[] correctLazers;

    private bool lockedInPosition;
    private bool FinishedFirstPuzzle;


    void Start ()
    {
        correctLazers = new bool[3];
        rotation = new float[5];
        puzzleOffset = new float[5];
        lazer1LineRenderer = lazer1Transform.GetComponent<LineRenderer>();
        lazer2LineRenderer = lazer2Transform.GetComponent<LineRenderer>();
        lazer3LineRenderer = lazer3Transform.GetComponent<LineRenderer>();
        targetlineRenderer = targetTransform.GetComponent<LineRenderer>();
        lazer1LineRenderer.SetPosition(0, lazer1LineRenderer.transform.position);
        lazer2LineRenderer.SetPosition(0, lazer2LineRenderer.transform.position);
        lazer3LineRenderer.SetPosition(0, lazer3LineRenderer.transform.position);
        targetlineRenderer.SetPosition(0, targetlineRenderer.transform.position);
        targetlineRenderer.SetPosition(1, targetlineRenderer.transform.position + new Vector3(0,100,0));
        targetlineRenderer.enabled = false;
        ringAnimations = new Animation[5];

        for (int i = 0;i<5;i++)
        {
            ringAnimations[i] = rings[i].GetComponent<Animation>();
        }

        ringAnimations[0]["Ring1Animation"].speed = .7f;
        ringAnimations[1]["Ring2Animation"].speed = .7f;
        ringAnimations[2]["Ring3Animation"].speed = .7f;
        ringAnimations[3]["Ring4Animation"].speed = .7f;
        ringAnimations[4]["Ring5Animation"].speed = .7f;

        SetPuzzleOffset();        
    }

    void SetPuzzleOffset()
    {
        puzzleOffset[0] = 60;
        puzzleOffset[1] = 20;
        puzzleOffset[2] = 120;
        puzzleOffset[3] = 180;
        puzzleOffset[4] = 140;
    }

    void Update ()
    {
        if (Physics.Linecast(lazer1Transform.position, targetTransform.position, out hit))
        {
            lazer1Particles.transform.position = hit.point;
            lazer1LineRenderer.SetPosition(1, hit.point);
            correctLazers[0] = false;
        }
        else
        {
            lazer1Particles.transform.position = targetTransform.transform.position;
            lazer1LineRenderer.SetPosition(1, targetTransform.transform.position);
            correctLazers[0] = true;
        }

        if (Physics.Linecast(lazer2Transform.position, targetTransform.position, out hit))
        {
            lazer2Particles.transform.position = hit.point;
            lazer2LineRenderer.SetPosition(1, hit.point);
            correctLazers[1] = false;
        }
        else
        {
            lazer2Particles.transform.position = targetTransform.transform.position;
            lazer2LineRenderer.SetPosition(1, targetTransform.transform.position);
            correctLazers[1] = true;
        }

        if (Physics.Linecast(lazer3Transform.position, targetTransform.position, out hit))
        {
            lazer3Particles.transform.position = hit.point;
            lazer3LineRenderer.SetPosition(1, hit.point);
            correctLazers[2] = false;
        }
        else
        {
            lazer3Particles.transform.position = targetTransform.transform.position;
            lazer3LineRenderer.SetPosition(1, targetTransform.transform.position);
            correctLazers[2] = true;
        }
        for (int i = 0; i < 5; i++)
        {
            if (!lockedInPosition)
            {
                if (FinishedFirstPuzzle)
                {
                    rings[i].transform.localRotation = Quaternion.Slerp(rings[i].transform.localRotation, Quaternion.Euler(90, rings[i].transform.localRotation.y, -10 + puzzleOffset[i] + rotation[i]), Time.deltaTime * 3);
                }
                else
                {
                    rings[i].transform.localRotation = Quaternion.Slerp(rings[i].transform.localRotation, Quaternion.Euler(-90, rings[i].transform.localRotation.y, -10 + puzzleOffset[i] + rotation[i]), Time.deltaTime * 3);
                }
            }
        }


        if (correctLazers[0] && correctLazers[1] && correctLazers[2] && !lockedInPosition && !FinishedFirstPuzzle)
        {
            StartCoroutine(WinningCheck());
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
        /*
        if (Input.GetButtonDown("Submit"))
        {
            lockedInPosition = true;
            targetlineRenderer.enabled = true;
            StartCoroutine(AnimationPlayer());
        }
        */
    }

    public void Button1()
    {
        if (!lockedInPosition)
        {
            rotation[3] += 20;
            rotation[2] += 20;
        }
    }

    public void Button2()
    {
        if (!lockedInPosition)
        {
            rotation[1] += 20;
            rotation[4] += 20;
        }
    }

    public void Button3()
    {
        if (!lockedInPosition)
        {
            rotation[0] += 20;
            rotation[2] += 20;
            rotation[4] += 20;
        }
    }

    IEnumerator WinningCheck()
    {
        yield return new WaitForSeconds(1f);
        if (correctLazers[0] && correctLazers[1] && correctLazers[2])
        {
            lockedInPosition = true;
            targetlineRenderer.enabled = true;
            StartCoroutine(AnimationPlayer());
        }
    }

    IEnumerator AnimationPlayer()
    {
        lazerAnimations[0].Play();
        lazerAnimations[1].Play();
        lazerAnimations[2].Play();
        targetlineRenderer.enabled = false;
        lazer1LineRenderer.enabled = false;
        lazer2LineRenderer.enabled = false;
        lazer3LineRenderer.enabled = false;
        lazer1Particles.gameObject.SetActive(false);
        lazer2Particles.gameObject.SetActive(false);
        lazer3Particles.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        ringAnimations[0].Play();
        yield return new WaitForSeconds(.3f);
        ringAnimations[1].Play();
        yield return new WaitForSeconds(.3f);
        ringAnimations[2].Play();
        yield return new WaitForSeconds(.3f);
        ringAnimations[3].Play();
        yield return new WaitForSeconds(.3f);
        ringAnimations[4].Play();
        yield return new WaitForSeconds(.3f);
        FinishedFirstPuzzle = true;
        lockedInPosition = false;
    }
}
