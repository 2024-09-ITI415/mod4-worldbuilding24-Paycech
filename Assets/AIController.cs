using UnityEngine;

public class AIController : MonoBehaviour
{
    public enum AIState { Roaming, Approaching, Circling, Sprinting }
    public AIState currentState = AIState.Roaming;

    private AISensor sensor;
    private AIMovement movement;
    private Transform player;
    private Animator animator;
    private AIAudioControllerClown audioController;

    public float circleInterval = 2f;
    public float approachDistance = 5f;
    public float circleRadius = 7f;
    public float roamSpeed = 2f;
    public float chaseSpeed = 5f;
    public float circlingSpeed = 3f;
    public float sprintSpeed = 100f;
    public float maximumDistance = 15f;

    private float circleTimer = 0f;

    void Start()
    {
        sensor = GetComponent<AISensor>();
        movement = GetComponent<AIMovement>();
        animator = GetComponent<Animator>();
        audioController = GetComponent<AIAudioControllerClown>();

        // Set initial speed
        movement.SetSpeed(roamSpeed);
    }

    void Update()
    {
        player = sensor.GetPlayer();

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > maximumDistance)
            {
                TransitionToState(AIState.Sprinting);
                movement.SetSpeed(sprintSpeed);
                movement.MoveTo(player.position);
            }
            else if (distanceToPlayer > approachDistance)
            {
                TransitionToState(AIState.Approaching);
                movement.SetSpeed(chaseSpeed);
                movement.MoveTo(player.position, approachDistance);
            }
            else
            {
                TransitionToState(AIState.Circling);
                movement.SetSpeed(circlingSpeed);
                HandleCircling();
            }

            // Rotate to face the player with an offset fix
            RotateTowardsPlayer();
        }
        else
        {
            if (currentState != AIState.Roaming)
            {
                TransitionToState(AIState.Roaming);
                movement.SetSpeed(roamSpeed);
            }
            movement.Roam();
        }
    }

    private void HandleCircling()
    {
        circleTimer += Time.deltaTime;
        if (circleTimer >= circleInterval)
        {
            Vector3 randomPosition = sensor.GetRandomPositionAroundPlayer(player, circleRadius);
            movement.MoveTo(randomPosition, approachDistance);
            circleTimer = 0f;
        }
    }

    private void TransitionToState(AIState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case AIState.Roaming:
                SetAnimationBooleans(isIdle: true, isRunning: false, isCircling: false, isSprinting: false);
                audioController.PlayAudio("Idle");
                break;

            case AIState.Approaching:
                SetAnimationBooleans(isIdle: false, isRunning: true, isCircling: false, isSprinting: false);
                audioController.PlayAudio("Run");
                break;

            case AIState.Circling:
                SetAnimationBooleans(isIdle: false, isRunning: false, isCircling: true, isSprinting: false);
                audioController.PlayAudio("Circle");
                break;

            case AIState.Sprinting:
                SetAnimationBooleans(isIdle: false, isRunning: false, isCircling: false, isSprinting: true);
                audioController.PlayAudio("Sprint");
                break;
        }
    }

    private void SetAnimationBooleans(bool isIdle, bool isRunning, bool isCircling, bool isSprinting)
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isCircling", isCircling);
        animator.SetBool("isSprinting", isSprinting);
    }

    private void RotateTowardsPlayer()
    {
        if (player == null) return;

        // Calculate direction to face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Lock rotation to horizontal plane

        if (direction != Vector3.zero)
        {
            // Set forward direction and apply manual rotation offset
            transform.forward = direction;
            transform.Rotate(0, 90f, 0); // Fix sideways orientation
        }
    }
}