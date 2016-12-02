using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerLamp : MonoBehaviour
{
    private TriggerChecker playerTriggerChecker;
    private TriggerChecker frontTriggerChecker1;
    private TriggerChecker backTriggerChecker1;
    private TriggerChecker leftTriggerChecker1;
    private TriggerChecker rightTriggerChecker1;
    private TriggerChecker frontTriggerChecker2;
    private TriggerChecker backTriggerChecker2;
    private TriggerChecker leftTriggerChecker2;
    private TriggerChecker rightTriggerChecker2;
    private Animator playerAnimator;
    private Animator startPlaceAnimator;

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public Camera camera4;

    private Vector3 startPosition;
    public bool godMode;
    public bool freezeMovement;
    private bool isMoving;
    private Vector3 movePosition;
    public GameObject[] bridges;
    public Animator[] bridgesAnimators;
    private GameObject lightActivate;

    void Start()
    {
        playerTriggerChecker = transform.GetChild(0).FindChild("PlayerTrigger").GetComponent<TriggerChecker>();
        frontTriggerChecker1 = transform.GetChild(0).FindChild("FrontTrigger1").GetComponent<TriggerChecker>();
        backTriggerChecker1 = transform.GetChild(0).FindChild("BackTrigger1").GetComponent<TriggerChecker>();
        leftTriggerChecker1 = transform.GetChild(0).FindChild("LeftTrigger1").GetComponent<TriggerChecker>();
        rightTriggerChecker1 = transform.GetChild(0).FindChild("RightTrigger1").GetComponent<TriggerChecker>();

        frontTriggerChecker2 = transform.GetChild(0).FindChild("FrontTrigger2").GetComponent<TriggerChecker>();
        backTriggerChecker2 = transform.GetChild(0).FindChild("BackTrigger2").GetComponent<TriggerChecker>();
        leftTriggerChecker2 = transform.GetChild(0).FindChild("LeftTrigger2").GetComponent<TriggerChecker>();
        rightTriggerChecker2 = transform.GetChild(0).FindChild("RightTrigger2").GetComponent<TriggerChecker>();
        startPosition = transform.position;
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        bridges = GameObject.FindGameObjectsWithTag("BridgeParents").OrderBy(go => go.name).ToArray();        
        bridgesAnimators = new Animator[bridges.Length];
        for (int i = 0; i < bridges.Length;i++)
        {
            bridgesAnimators[i] = bridges[i].transform.GetChild(0).GetComponent<Animator>();
        }
        startPlaceAnimator = GameObject.Find("StartPanelAnimation").GetComponent<Animator>();
        lightActivate = GameObject.Find("LightActivate"); lightActivate.SetActive(false);
        StartCoroutine(PlayRespawnAnimation());
    }

    void Update()
    {
        if (!freezeMovement)
        {
            if (Input.GetButtonDown("Left") && !isMoving)
            {
                if (leftTriggerChecker1.hitCardboardBox)
                {
                    if (!leftTriggerChecker2.movementBlocked)
                    {
                        Transform Box = leftTriggerChecker1.CardboardBox.transform;
                        StartCoroutine(MoveBoxTo(Box, Box.position + Vector3.forward));
                        StartCoroutine(MoveTo(transform.position + Vector3.forward));
                        playerAnimator.Play("Move");
                    }
                    else
                    {
                        playerAnimator.Play("MovementBlocked");
                    }
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position + Vector3.forward));
                    playerAnimator.Play("Move");
                }
            }
            if (Input.GetButtonDown("Right") && !isMoving)
            {
                if (rightTriggerChecker1.hitCardboardBox)
                {
                    if (!rightTriggerChecker2.movementBlocked)
                    {
                        Transform Box = rightTriggerChecker1.CardboardBox.transform;
                        StartCoroutine(MoveBoxTo(Box, Box.position + Vector3.back));
                        StartCoroutine(MoveTo(transform.position + Vector3.back));
                        playerAnimator.Play("Move");
                    }
                    else
                    {
                        playerAnimator.Play("MovementBlocked");
                    }
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position + Vector3.back));
                    playerAnimator.Play("Move");
                }
            }
            if (Input.GetButtonDown("Up") && !isMoving)
            {
                if (frontTriggerChecker1.hitCardboardBox)
                {
                    if (!frontTriggerChecker2.movementBlocked)
                    {
                        Transform Box = frontTriggerChecker1.CardboardBox.transform;
                        StartCoroutine(MoveBoxTo(Box, Box.position + Vector3.right));
                        StartCoroutine(MoveTo(transform.position + Vector3.right));
                        playerAnimator.Play("Move");
                    }
                    else
                    {
                        playerAnimator.Play("MovementBlocked");
                    }
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position + Vector3.right));
                    playerAnimator.Play("Move");
                }
            }
            if (Input.GetButtonDown("Down") && !isMoving)
            {
                if (backTriggerChecker1.hitCardboardBox)
                {
                    if (!backTriggerChecker2.movementBlocked)
                    {
                        Transform Box = backTriggerChecker1.CardboardBox.transform;
                        StartCoroutine(MoveBoxTo(Box, Box.position + Vector3.left));
                        StartCoroutine(MoveTo(transform.position + Vector3.left));
                        playerAnimator.Play("Move");
                    }
                    else
                    {
                        playerAnimator.Play("MovementBlocked");
                    }
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position + Vector3.left));
                    playerAnimator.Play("Move");
                }
            }
        }
    }

    IEnumerator PlayRespawnAnimation()
    {
        startPlaceAnimator.Play("StartPlaceAnimationDown");
        freezeMovement = true;
        playerAnimator.Play("Respawning");
        yield return new WaitForSeconds(.5f);
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        freezeMovement = false;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position,target) > 0.005f)
        {
            var delta = Time.deltaTime * 15; // Assuming existing speed variable.
            transform.position = Vector3.Lerp(transform.position,target,delta);
            yield return new WaitForSeconds(.01f);
        }
        transform.position = target;
        isMoving = false;
        
        if (CheckIfFinished())
        {
            freezeMovement = true;
            yield return new WaitForSeconds(2f);
            lightActivate.SetActive(true);
        }
        if (playerTriggerChecker.hitWater && !playerTriggerChecker.hitFloor && !playerTriggerChecker.hitBridge)
        {
            freezeMovement = true;
            playerAnimator.Play("Death");
            yield return new WaitForSeconds(.5f);
            yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
            //resetScene
        }
        if (playerTriggerChecker.selectedButton != "")
        {
            freezeMovement = true;
            int bridgeNumber = int.Parse(playerTriggerChecker.selectedButton.Substring(6, 1));
            bridgesAnimators[bridgeNumber].Play("BridgeFallDown");
            yield return new WaitForSeconds(1.2f);
            freezeMovement = false;
        }
    }

    IEnumerator MoveBoxTo(Transform box,Vector3 target)
    {
        isMoving = true;
        while (Vector3.Distance(box.position, target) > 0.005f)
        {
            var delta = Time.deltaTime * 15; // Assuming existing speed variable.
            box.position = Vector3.Lerp(box.position, target, delta);
            yield return new WaitForSeconds(.01f);
        }
        box.position = target;
    }

    public void ResetTriggerState()
    {
        frontTriggerChecker1.ResetTriggers();
        frontTriggerChecker2.ResetTriggers();
        backTriggerChecker1.ResetTriggers();
        backTriggerChecker2.ResetTriggers();
        leftTriggerChecker1.ResetTriggers();
        leftTriggerChecker2.ResetTriggers();
        rightTriggerChecker1.ResetTriggers();
        rightTriggerChecker2.ResetTriggers();
    }

    bool CheckIfFinished()
    {
        if (frontTriggerChecker1.hitGoal)
        {
            playerAnimator.Play("PlayerFinishFront");
            return true;
        }
        else if (backTriggerChecker1.hitGoal)
        {
            playerAnimator.Play("PlayerFinishBack");
            return true;
        }
        else if (leftTriggerChecker1.hitGoal)
        {
            playerAnimator.Play("PlayerFinishLeft");
            return true;
        }
        else if (rightTriggerChecker1.hitGoal)
        {
            playerAnimator.Play("PlayerFinishRight");
            return true;
        }
        else return false;
    }
}
