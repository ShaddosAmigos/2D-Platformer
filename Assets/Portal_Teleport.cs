using UnityEngine;

public class Portal_Teleport : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform destination; // Drag your 'Exit Point' object here
    [SerializeField] private string playerTag = "Player"; // Ensure your Player object has this tag

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the portal is actually the player
        if (collision.CompareTag(playerTag))
        {
            TeleportPlayer(collision.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        if (destination != null)
        {
            // Move the player to the destination's position and rotation
            playerTransform.position = destination.position;

            // Optional: If you want to match the exit's rotation
            // playerTransform.rotation = destination.rotation;
        }
        else
        {
            Debug.LogWarning("Portal reached, but no destination is assigned!");
        }
    }
}