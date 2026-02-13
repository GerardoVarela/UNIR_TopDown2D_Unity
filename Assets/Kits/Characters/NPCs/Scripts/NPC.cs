using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : BaseCharacter
{
    public GameObject dialoguePanel;
    public GameObject hintPanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    [SerializeField] float wordSpeed = 0.04f;
    private bool playerIsClose;


    public Transform playerPosition;

    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Keyboard.current.cKey.isPressed && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                FacePlayer();
                hintPanel.SetActive(false);
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
        }

        base.Update();
    }

    private void FacePlayer()
    {
        Vector2 direction = (playerPosition.transform.position - transform.position).normalized;
        Move(direction);
    }

    private void ResetPosition()
    {
        Vector2 direction = new Vector2(0, 0);
        Move(direction);
    }

    private void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    private IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
            hintPanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            hintPanel.SetActive(false);
            RemoveText();
            ResetPosition();
        }
    }
}
