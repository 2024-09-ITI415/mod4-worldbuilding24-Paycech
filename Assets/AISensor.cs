using UnityEngine;

public class AISensor : MonoBehaviour
{
    public float sightRange = 10f;    // Detection range for the player
    public LayerMask playerLayer;    // Layer mask for detecting the player
    private Transform player;        // Reference to the detected player

    // Detect the player within sight range
    public Transform GetPlayer()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
        if (playersInRange.Length > 0)
        {
            player = playersInRange[0].transform;
        }
        else
        {
            player = null;
        }
        return player;
    }

    // Generate a random position around the player within a given radius
    public Vector3 GetRandomPositionAroundPlayer(Transform player, float radius)
    {
        if (player == null)
        {
            Debug.LogError("Player is null. Cannot generate random position.");
            return transform.position;
        }

        Vector3 randomOffset = Random.insideUnitSphere.normalized * radius;
        randomOffset.y = 0; // Ensure movement stays on the same plane
        return player.position + randomOffset;
    }
}
