using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        AI_Creature creature = GetComponent<AI_Creature>();
        waypoints = creature.GetWaypoints();

        // Adjust agent properties for smoother movement
        agent.angularSpeed = 360f; // Fast turning
        agent.stoppingDistance = 1f; // Distance before stopping
    }

    public void Roam()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints are missing or empty!");
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            Transform nextWaypoint = waypoints[currentWaypointIndex];
            agent.SetDestination(nextWaypoint.position);
        }
    }

    public void MoveTo(Vector3 targetPosition, float minimumDistance = 0f)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > minimumDistance)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            agent.ResetPath();
        }
    }

    public bool IsNearTarget(Vector3 targetPosition, float threshold)
    {
        return Vector3.Distance(agent.transform.position, targetPosition) <= threshold;
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
        agent.acceleration = speed * 2; // Adjust acceleration to match speed
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        agent.angularSpeed = movementSpeed * 100f; // Adjust angular speed
    }

    public void DebugAgentState()
    {
        Debug.Log($"Agent Speed: {agent.speed}, Remaining Distance: {agent.remainingDistance}, Path Pending: {agent.pathPending}");
    }
}