using UnityEngine;

public class AIAudioController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip idleClip;       // Audio for idle state
    public AudioClip chaseClip;      // Audio for chase state
    public AudioClip teleportClip;   // Audio for teleportation
    public AudioClip attackClip;     // Audio for attack state

    private AudioSource audioSource;
    private string currentState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing! Add one to the GameObject.");
        }
    }

    // Call this method to play a specific clip
    public void PlayAudioForState(string state)
    {
        if (currentState == state) return; // Avoid restarting the same audio

        currentState = state;

        switch (state)
        {
            case "Idle":
                PlayClip(idleClip, true);
                break;

            case "Chase":
                PlayClip(chaseClip, true);
                break;

            case "Teleport":
                PlayClip(teleportClip, false);
                break;

            case "Attack":
                PlayClip(attackClip, false);
                break;
        }
    }

    private void PlayClip(AudioClip clip, bool loop)
    {
        if (clip != null)
        {
            Debug.Log("Playing clip: " + clip.name); // Debug statement to confirm playback
            audioSource.Stop(); // Stop any existing sound
            audioSource.clip = clip; // Assign the new clip
            audioSource.loop = loop; // Set looping if needed
            audioSource.Play(); // Play the clip
        }
        else
        {
            Debug.LogWarning("Audio clip is not assigned for state: " + currentState);
        }
    }
}
