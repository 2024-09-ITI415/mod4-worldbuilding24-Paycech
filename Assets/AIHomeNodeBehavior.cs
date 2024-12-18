using UnityEngine;
using UnityEngine.AI;

public class AIHomeNodeBehavior : MonoBehaviour
{
    [Header("References")]
    public Transform homeNode;
    public Transform[] teleportNodes;
    public Transform player;

    [Header("Speed Settings")]
    public float idleSpeed = 3f;
    public float chaseSpeed = 8f;
    public float teleportDistance = 1f;
    public float predictionFactor = 1.5f; // Multiplier for predicting player movement
    public float directChaseRange = 3f;   // Range to switch to direct manual movement

    private NavMeshAgent agent;
    private bool isChasing = false;

    private AIAudioController audioController; // Reference to AIAudioController

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioController = GetComponent<AIAudioController>();

        if (homeNode != null)
        {
            agent.speed = idleSpeed;
            agent.SetDestination(homeNode.position);
        }

        agent.updateRotation = false; // Disable auto-rotation to handle rotation manually
        agent.stoppingDistance = 0.1f; // Minimize stopping distance to avoid orbiting

        // Play idle sound at the start
        if (audioController != null)
        {
            audioController.PlayAudioForState("Idle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            StartChase();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            EndChase();
        }
    }

    private void Update()
    {
        if (isChasing && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= teleportDistance)
            {
                TeleportPlayer();
                EndChase();
            }
            else if (distanceToPlayer <= directChaseRange)
            {
                // Move directly toward the player when close
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * chaseSpeed * Time.deltaTime;

                // Force the AI to face the player with a rotation fix
                transform.forward = direction;
                transform.Rotate(0, 90f, 0); // Manual adjustment to fix sideways orientation
            }
            else
            {
                // Use NavMeshAgent for distant chasing
                Vector3 playerVelocity = player.GetComponent<Rigidbody>()?.velocity ?? Vector3.zero;
                Vector3 predictedPosition = player.position + playerVelocity * predictionFactor;

                agent.SetDestination(predictedPosition);

                // Force the AI to face the player with a rotation fix
                transform.forward = (player.position - transform.position).normalized;
                transform.Rotate(0, 90f, 0); // Manual adjustment to fix sideways orientation
            }
        }
        else if (!isChasing && homeNode != null)
        {
            agent.SetDestination(homeNode.position);
        }
    }

    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;

        // Play chase audio
        if (audioController != null)
        {
            audioController.PlayAudioForState("Chase");
        }
    }

    private void TeleportPlayer()
    {
        if (player == null || teleportNodes == null || teleportNodes.Length == 0)
            return;

        Transform selectedNode = teleportNodes[Random.Range(0, teleportNodes.Length)];
        var characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
            characterController.enabled = false;

        player.position = selectedNode.position + new Vector3(0, 1f, 0);

        if (characterController != null)
            characterController.enabled = true;

        // Play teleport audio
        if (audioController != null)
        {
            audioController.PlayAudioForState("Teleport");
        }
    }

    private void EndChase()
    {
        isChasing = false;
        agent.speed = idleSpeed;
        agent.ResetPath();

        // Play idle audio
        if (audioController != null)
        {
            audioController.PlayAudioForState("Idle");
        }
    }

    public bool IsChasing()
    {
        return isChasing;
    }

    public bool IsInTeleportRange()
    {
        return Vector3.Distance(transform.position, player.position) <= teleportDistance;
    }
}
