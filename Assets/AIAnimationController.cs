using UnityEngine;

public class AIAnimationController : MonoBehaviour
{
    private Animator animator; // Animator component
    private AIHomeNodeBehavior aiBehavior; // Reference to the AI behavior script

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        aiBehavior = GetComponent<AIHomeNodeBehavior>(); // Get the AI behavior script
    }

    void Update()
    {
        // If AI is chasing, trigger the "Chase" animation
        if (aiBehavior.IsChasing())
        {
            animator.SetBool("IsChasing", true);
        }
        else
        {
            animator.SetBool("IsChasing", false);
        }

        // If AI is close enough to teleport, trigger the "Attack" animation
        if (aiBehavior.IsInTeleportRange())
        {
            animator.SetTrigger("Attack");
        }
    }
}
