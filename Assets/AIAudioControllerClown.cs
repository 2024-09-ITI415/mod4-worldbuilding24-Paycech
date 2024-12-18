using UnityEngine;

public class AIAudioControllerClown : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip idleAudio;
    public AudioClip runAudio;
    public AudioClip circleAudio;
    public AudioClip sprintAudio;

    public void PlayAudio(string state)
    {
        switch (state)
        {
            case "Idle":
                PlayClip(idleAudio);
                break;
            case "Run":
                PlayClip(runAudio);
                break;
            case "Circle":
                PlayClip(circleAudio);
                break;
            case "Sprint":
                PlayClip(sprintAudio);
                break;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource.isPlaying && audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
