using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip introMusic;
    [SerializeField] AudioClip transitionMusic;
    [SerializeField] AudioClip gameLoopMusic;
    private AudioSource musicSource;

    private AudioSource[] sfxPool;
    [SerializeField] private int sfxPoolSize = 30;
    private int nextSfxIndex;


    private int musicState = 0;
    /* 0 = intro music
     * 1 = transition music
     * 2 = main loop
    */

    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxPool = new AudioSource[sfxPoolSize];
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxPool[i] = src;
        }
    }

    public void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        switch (musicState)
        {
            case 0:
                musicSource.clip = introMusic;
                musicSource.Play();
                StartCoroutine(PlayNext(introMusic.length));
                break;
            case 1:
                musicSource.clip = transitionMusic;
                musicSource.Play();
                SetState(2);
                StartCoroutine(PlayNext(transitionMusic.length));
                break;
            case 2:
                musicSource.clip = gameLoopMusic;
                musicSource.Play();
                StartCoroutine(PlayNext(gameLoopMusic.length));
                break;
            default:
                break;
        }

    }

    IEnumerator PlayNext(float time)
    {
        yield return new WaitForSeconds(time);
        PlayMusic();
    }

    //called once when the game starts, called again when the transition is played.
    public void SetState(int newState)
    {
        musicState = newState;
    }


    public void PlaySfx(AudioClip clip)
    {
        AudioSource src = sfxPool[nextSfxIndex];
        nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Length;

        src.PlayOneShot(clip);
    }
}
