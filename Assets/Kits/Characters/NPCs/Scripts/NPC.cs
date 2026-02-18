using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class DialogueGroup
{
    public string[] lines;
}

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
    [SerializeField] DialogueGroup[] dialogues;
    [SerializeField] float wordSpeed = 0.04f;
    [SerializeField] Transform playerPosition;
    [Header("NPC Quest")]
    [SerializeField] bool activateQuest;
    [SerializeField] InventoryItemDefinition requiredItem;
    [SerializeField] int requiredAmount = 3;

    private string[] dialogue;
    private int dialogueIndex = 0;
    private int lineIndex;
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
        dialogue = dialogues[dialogueIndex].lines;
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
                if (activateQuest) CheckQuest();
                hintPanel.SetActive(false);
                dialoguePanel.SetActive(true);
                typingCoroutine = StartCoroutine(Typing());
            }
            else if (isTyping)
            {
                // If C Key was pressed while typing, end the line instantly
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogue[lineIndex];
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
        lineIndex = 0;
        dialoguePanel.SetActive(false);
    }

    private IEnumerator Typing()
    {
        isTyping = true;
        dialogueText.text = ""; 

        foreach (char letter in dialogue[lineIndex].ToCharArray())
        {
            if (!isTyping) break; // If C Key was pressed, the variable is updated in Update method
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    private void NextLine()
    {
        if (lineIndex < dialogue.Length - 1)
        {
            lineIndex++;
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


    private void CheckQuest()
    {
        Inventory inventory = playerPosition.GetComponent<Inventory>();
        if (inventory == null)
            return;

        List<InventoryItem> questItem = inventory.GetItems(requiredItem.uniqueItemName);

        Debug.Log("questItem.Count");
        Debug.Log(questItem.Count);

        // if (questItem != null && questItem.quantity >= requiredAmount)
        // {
        //     inventory.RemoveItem(questItem, requiredAmount);
        //     Destroy(gameObject);
        // }   
    }
}
