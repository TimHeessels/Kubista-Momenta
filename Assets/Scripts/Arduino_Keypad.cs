using UnityEngine;
using System.IO.Ports;
using System.Collections;

public class Arduino_Keypad : MonoBehaviour
{
    public AudioClip[] tones;
    private AudioSource audioSource;
    private int[] passCode;
    private int[] enteredCode;
    private int buttonsPushed;
    private bool allowInput;

    void Start()
    {     
        audioSource = GetComponent<AudioSource>();

        allowInput = true;
        passCode = new int[4];
        passCode[0] = 1; passCode[1] = 3; passCode[2] = 3; passCode[3] = 7;
        enteredCode = new int[4];
        resetKeypad();
    }

    void resetKeypad()
    {
        allowInput = true;
        buttonsPushed = 0;
    }

    void Update()
    {
        if (Arduino.Instance.inputNumber == 0)
        {
            PressedKey(Arduino.Instance.inputNumber);
        } 
    }

    void PressedKey(int key)
    {
        if (allowInput)
        {
            Debug.Log(key);
            audioSource.clip = tones[key];
            audioSource.Play();

            enteredCode[buttonsPushed] = key;
            if (buttonsPushed < passCode.Length - 1)
            {
                buttonsPushed++;
            }
            else
            {
                StartCoroutine(checkPassCode());
            }
        }
    }

    IEnumerator checkPassCode()
    {
        allowInput = false;
        bool isCorrect = true;
        for (int i = 0; i < passCode.Length; i++)
        {
            if (passCode[i] != enteredCode[i])
            {
                isCorrect = false;
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log(isCorrect);
        if (isCorrect)
        {
            audioSource.clip = tones[9];
            for (int i = 0; i < 3; i++)
            {
                audioSource.Play();
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            audioSource.clip = tones[10];
            audioSource.Play();
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.42f);
        resetKeypad();
    }
}