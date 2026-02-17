using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : BaseCharacter
{
    [Header("NPC Info")]
    [SerializeField] string npcName;
    [SerializeField] Color npcColor;
    [SerializeField] Sprite npcProfilePIC;
    [Header("NPC Dialogue")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject hintPanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image dialogueNPCImage;
    [SerializeField] TextMeshProUGUI dialogueNPCName;
    [SerializeField] string[] dialogue;
    [SerializeField] float wordSpeed = 0.04f;
    [SerializeField] Transform playerPosition;

    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;
    private bool playerIsClose;

    void Start()
    {
        dialogueText.text = "";
        dialogueNPCImage.sprite = npcProfilePIC;
        dialogueNPCName.text = npcName;
        npcColor.a = 1f;
        dialogueNPCName.color = npcColor;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                // Hide hint and show dialogue
                FacePlayer();
                hintPanel.SetActive(false);
                dialoguePanel.SetActive(true);
                typingCoroutine = StartCoroutine(Typing());
            }
            else if (isTyping)
            {
                // If C Key was pressed while typing, end the line instantly
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogue[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }

        base.Update();
    }

    private void FacePlayer()
    {
        Vector2 facingDirection = (playerPosition.transform.position - transform.position).normalized;
        Move(facingDirection);
    }

    private Vector2 initialFacingDirection = new Vector2(0,0);
    private void ResetFacingDirection()
    {
        Vector2 facingDirection = initialFacingDirection;
        Move(facingDirection);
    }

    private void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    private IEnumerator Typing()
    {
        isTyping = true;
        dialogueText.text = ""; 

        foreach (char letter in dialogue[index].ToCharArray())
        {
            if (!isTyping) break; // If C Key was pressed, the variable is updated in Update method
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
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

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            RemoveText();
            ResetFacingDirection();
        }
    }
}
