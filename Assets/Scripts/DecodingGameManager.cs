using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DecodingGameManager : MonoBehaviour
{
    [TextArea]
    public string codeMessage;
    [TextArea]
    public string EditableText;
    [TextArea]
    public string CompletedText;
    public string englishText;
    public bool isSelecting;
    public char[] letters;
    public char[] lettersCompleted;
    public char selectedLetter;
    public Text encodedText;
    public Text editText;
    public Text selectedLetterText;

    void Start()
    {
        letters = new char[codeMessage.Length + 1];
        letters = codeMessage.ToCharArray();
        lettersCompleted = new char[EditableText.Length + 1];
        lettersCompleted = EditableText.ToCharArray();
        editText.text = EditableText;
        var count = 0;
        foreach (char letter in lettersCompleted)
        {
            count++;
            if (letter.ToString() == "$")
            {
                editText.text = editText.text.Remove(count - 1, 1);
                editText.text = editText.text.Insert(count - 1, "\n");
            }
            if (letter.ToString() == "_")
            {
                editText.text = editText.text.Remove(count - 1, 1);
                editText.text = editText.text.Insert(count - 1, " ");
            }
        }
        count = 0;
        foreach (char letter in letters)
        {
            count++;
            if (letter.ToString() == "$")
            {
                encodedText.text = encodedText.text.Remove(count - 1, 1);
                encodedText.text = encodedText.text.Insert(count - 1, "\n");
            }
        }
        selectedLetterText.text = "";
    }

    void Update()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                string keyHolder = vKey.ToString();
                keyHolder = Regex.Replace(keyHolder, @"[^a-zA-Z]", "");
                if (keyHolder.Length == 1)
                {
                    char newSelectedLetter = keyHolder.ToCharArray()[0];
                    if (newSelectedLetter != selectedLetter)
                    {
                        if (!isSelecting)
                        {                            
                            selectedLetter = keyHolder.ToCharArray()[0];
                            selectedLetterText.text = selectedLetter.ToString();
                            int count = 0;
                            foreach (char letter in letters)
                            {
                                count++;
                                if (letter == char.ToLower(selectedLetter) || letter == char.ToUpper(selectedLetter))
                                {
                                    lettersCompleted[count - 1] = char.ToLower(keyHolder.ToCharArray()[0]);
                                    editText.text = editText.text.Remove(count - 1, 1);
                                    editText.text = editText.text.Insert(count - 1, "_");
                                }
                            }
                        }
                        else
                        {
                            selectedLetterText.text = "";
                            int count = 0;
                            foreach (char letter in letters)
                            {
                                count++;
                                if (letter == char.ToLower(selectedLetter))
                                {
                                    lettersCompleted[count - 1] = char.ToLower(keyHolder.ToCharArray()[0]);
                                    editText.text = editText.text.Remove(count - 1, 1);
                                    editText.text = editText.text.Insert(count - 1, char.ToLower(keyHolder.ToCharArray()[0]).ToString());
                                }
                                if (letter == char.ToUpper(selectedLetter))
                                {
                                    lettersCompleted[count - 1] = char.ToUpper(keyHolder.ToCharArray()[0]);
                                    editText.text = editText.text.Remove(count-1, 1);
                                    editText.text = editText.text.Insert(count-1, char.ToUpper(keyHolder.ToCharArray()[0]).ToString());
                                }
                                if (letter.ToString() == "$")
                                {
                                    editText.text = editText.text.Remove(count - 1, 1);
                                    editText.text = editText.text.Insert(count - 1, "\n");
                                }
                            }
                            foreach (char letter in letters)
                            {
                                if (letter == char.ToLower(selectedLetter) || letter == char.ToUpper(selectedLetter))
                                {
                                    string emptyString = " ";
                                    selectedLetter = emptyString.ToCharArray()[0];
                                }
                            }
                        }
                        isSelecting = !isSelecting;
                    }
                }
            }
        }
        if (editText.text.Contains(CompletedText) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Finished");
            editText.color = new Color(87f / 255f, 166f / 255f, 74f / 255f);
            encodedText.text = "";
        }
    }

    /*
    if (input = r)
    {
        char[]
        isselecting = !isselecting;
    }

    for each string r in char[]
    if (r = r)
    {
        r = input.keydown(newkew);
    }*/

}