//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    [Header("----------- Audio Source-----------")]
//    [SerializeField] AudioSource musicSource;
//    [SerializeField] AudioSource SFXSource;

//    [Header("----------- Audio Clip-----------")]
//    public AudioClip background;
//    public AudioClip death;
//    public AudioClip wallTouch;
//    public AudioClip jump;
//    public AudioClip keyPickup;
//    public AudioClip exitDoor;
//    public AudioClip micePickup;
//    public AudioClip checkPoint;
//    public AudioClip waterSplash;
//    public AudioClip walk;
//    public AudioClip enemyHit;
//    public AudioClip spikeHit;
//    public AudioClip fallingSpikeHit;

//    private void Start()
//    {
//        musicSource.clip = background;
//        musicSource.Play();
//    }

//    public void PlaySFX(AudioClip clip)
//    {
//        SFXSource.PlayOneShot(clip);
//    }
//}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source -----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("----------- Audio Clip -----------")]
    public AudioClip death;
    public AudioClip wallTouch;
    public AudioClip jump;
    public AudioClip keyPickup;
    public AudioClip exitDoor;
    public AudioClip micePickup;
    public AudioClip checkPoint;
    public AudioClip waterSplash;
    public AudioClip walk;
    public AudioClip enemyHit;
    public AudioClip spikeHit;
    public AudioClip fallingSpikeHit;

    private void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}

