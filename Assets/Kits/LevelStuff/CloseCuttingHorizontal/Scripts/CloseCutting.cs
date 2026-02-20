using UnityEngine;

public class CloseCutting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();

        if(playerCharacter != null)
        {
            playerCharacter.NotifyPunch(0.2f);
        }
    }
}
