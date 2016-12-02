using UnityEngine;
using System.Collections;

public class Notes : MonoBehaviour
{
    private SpriteRenderer[] notes;
    private AudioSource audioSource;
    public AudioClip[] musicNotes;
    private int[] positions;
    private int[] correctPositions;
    private int selectedNote;
    private int amountOfNotes;
    private bool allowInput = true;
    private Vector3[] origin;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        amountOfNotes = transform.childCount;
        notes = new SpriteRenderer[amountOfNotes+1];
        origin = new Vector3[amountOfNotes + 1];
        for (int i = 0; i < amountOfNotes; i++)
        {
            notes[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            origin[i] = notes[i].transform.localPosition;
        }
        positions = new int[amountOfNotes];
        correctPositions = new int[amountOfNotes];
        correctPositions[0] = 4; correctPositions[1] = 2; correctPositions[2] = 0; correctPositions[3] = 3; correctPositions[4] = 4;
    }

    void Update()
    {
        if (allowInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedNote > 0)
                {
                    selectedNote--;
                }
                else
                {
                    selectedNote = amountOfNotes - 1;
                }
                AudioSource.PlayClipAtPoint(musicNotes[positions[selectedNote]], new Vector3(0, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedNote < amountOfNotes - 1)
                {
                    selectedNote++;
                }
                else
                {
                    selectedNote = 0;
                }
                AudioSource.PlayClipAtPoint(musicNotes[positions[selectedNote]], new Vector3(0, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (positions[selectedNote] < 4)
                {
                    positions[selectedNote]++;
                    audioSource.clip = musicNotes[positions[selectedNote]];
                    AudioSource.PlayClipAtPoint(musicNotes[positions[selectedNote]], new Vector3(0, 0, 0));
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (positions[selectedNote] > 0)
                {
                    positions[selectedNote]--;
                    audioSource.clip = musicNotes[positions[selectedNote]];
                    AudioSource.PlayClipAtPoint(musicNotes[positions[selectedNote]], new Vector3(0, 0, 0));
                }
            }

            bool isCorrect = true;
            for (int i = 0; i < amountOfNotes; i++)
            {
                if (i == selectedNote) notes[i].color = new Color(1, 1, 1);
                else notes[i].color = new Color(0, 0, 0);
                //notes[i].transform.position = new Vector3(notes[i].transform.position.x, -0.35f + (0.51f * positions[i]), notes[i].transform.position.z);
                notes[i].transform.localPosition = Vector3.Lerp(notes[i].transform.localPosition, new Vector3(notes[i].transform.localPosition.x, origin[i].y + (0.51f * positions[i]), notes[i].transform.localPosition.z), Time.deltaTime*15);
                if (positions[i] != correctPositions[i])
                {
                    isCorrect = false;
                }
            }
            if (isCorrect)
            {
                StartCoroutine(PlayMelody());
                allowInput = false;
            }
        }
    }

    IEnumerator PlayMelody()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < amountOfNotes; i++)
        {
            notes[i].color = new Color(1, 1, 1);
        }
        for (int i = 0; i < amountOfNotes; i++)
        {
            yield return new WaitForSeconds(.3f);
            AudioSource.PlayClipAtPoint(musicNotes[positions[i]], new Vector3(0, 0, 0));
        }
    }
    
}