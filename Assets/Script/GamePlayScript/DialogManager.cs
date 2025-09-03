using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI speakerText; // drag TMP di Inspector
    public GameObject dialogBox;
    public TextMeshProUGUI bodyText;


    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;
    private bool isTyping = false;
    private bool skipTyping = false;
    private string currentSentence;
    private System.Action onFinished; // callback ke GameManager

    void Start()
    {
        if (dialogBox) dialogBox.SetActive(false);
    }

    public void ShowDialog(string speaker, string sentence, System.Action finishedCallback = null)
    {
        if (dialogBox) dialogBox.SetActive(true);
        if (speakerText) speakerText.text = speaker;

        currentSentence = sentence;
        onFinished = finishedCallback;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        skipTyping = false;
        bodyText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (skipTyping)
            {
                bodyText.text = sentence;
                break;
            }
            bodyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (dialogBox.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                // selesai dialog ini → trigger callback ke GameManager
                onFinished?.Invoke();
            }
        }
    }

    public void HideDialog()
    {
        if (dialogBox) dialogBox.SetActive(false);
        bodyText.text = "";
        speakerText.text = "";
    }
}
