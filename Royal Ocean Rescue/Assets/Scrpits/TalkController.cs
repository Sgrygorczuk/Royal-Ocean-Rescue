using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Image character;
    public string[] sentences;
    public int index = 0;
    private bool _breakOut = false;
    public bool isTalking = false;
    public float dialogueSpeed;

    private void Awake()
    {
        NextSentence();
    }

    public void LoadText(string[] newSentences, Sprite sprite)
    {
        index = 0;
        sentences = newSentences;
        character.sprite = sprite;
    }

    public bool Done()
    {
        return index == sentences.Length;
    }

    public void SetCanvas(bool state)
    {
        dialogueCanvas.enabled = state;
    }

    public void NextSentence()
    {
        if (index <= sentences.Length - 1 && !isTalking)
        {
            dialogueText.text = "";
            isTalking = true;
            StartCoroutine(WriteSentence());
        }
        else
        {
            _breakOut = true;
        }
    }

    private IEnumerator WriteSentence()
    {
        foreach (var character in sentences[index].ToCharArray())
        {
            if (_breakOut)
            {
                _breakOut = false;
                index++;
                if (index >= sentences.Length) yield break;
                dialogueText.text = "";
                StartCoroutine(WriteSentence());
                yield break;
            }
            dialogueText.text += character;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        isTalking = false;
        index++;
    }
}
