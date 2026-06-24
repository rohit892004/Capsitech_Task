using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;

    public AudioClip moveSound;
    public AudioClip bombSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip buttonSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMove()
    {
        if(moveSound)
            audioSource.PlayOneShot(moveSound);
    }

    public void PlayBomb()
    {
        if(bombSound)
            audioSource.PlayOneShot(bombSound);
    }

    public void PlayWin()
    {
        if(winSound)
            audioSource.PlayOneShot(winSound);
    }

    public void PlayLose()
    {
        if(loseSound)
            audioSource.PlayOneShot(loseSound);
    }

    public void PlayButton()
    {
        if(buttonSound)
            audioSource.PlayOneShot(buttonSound);
    }
}