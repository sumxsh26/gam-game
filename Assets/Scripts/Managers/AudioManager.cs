using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source-----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip-----------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip wallTouch;
    public AudioClip jump;
    public AudioClip keyPickup;
    public AudioClip exitDoor;
    public AudioClip micePickup;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
