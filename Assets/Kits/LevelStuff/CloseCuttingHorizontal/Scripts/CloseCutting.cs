using UnityEngine;

public class CloseCutting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();

        if(playerCharacter != null)
        {
            Debug.Log("Player hit by close cutting");
            // TODO: Call the damage method of PlayerCharacter
        }
    }
}
