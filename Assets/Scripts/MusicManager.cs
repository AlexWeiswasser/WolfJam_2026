using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip introMusic;
    [SerializeField] AudioClip transitionMusic;
    [SerializeField] AudioClip gameLoopMusic;

    private AudioSource[] musicSources = new AudioSource[2];
    private int activeSource;
    private double nextStartTime;
    private const double ScheduleLead = 0.5;

    private AudioSource[] sfxPool;
    [SerializeField] private int sfxPoolSize = 30;
    private int nextSfxIndex;
    [SerializeField] float pitchVariance = 0.15f;

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

        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].playOnAwake = false;
        }

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
        nextStartTime = AudioSettings.dspTime + 0.1;
        StartCoroutine(MusicRoutine());
    }

    private IEnumerator MusicRoutine()
    {
        while (true)
        {
            AudioClip clip = null;
            switch (musicState)
            {
                case 0: clip = introMusic; break;
                case 1: clip = transitionMusic; SetState(2); break;
                case 2: clip = gameLoopMusic; break;
            }

            if (clip == null)
                yield break;

            // Make sure the clip is actually loaded (async on WebGL)
            if (clip.loadState != AudioDataLoadState.Loaded)
                clip.LoadAudioData();
            while (clip.loadState == AudioDataLoadState.Loading)
                yield return null;
            if (clip.loadState != AudioDataLoadState.Loaded || clip.length <= 0f)
            {
                yield return null; // never spin without yielding
                continue;
            }

            AudioSource src = musicSources[activeSource];
            src.clip = clip;

    #if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: simple sequential playback
            src.Play();
            yield return null;
            while (src.isPlaying)
                yield return null;
    #else
            // Other platforms: gapless scheduled playback
            if (nextStartTime < AudioSettings.dspTime)
                nextStartTime = AudioSettings.dspTime + 0.1;

            src.PlayScheduled(nextStartTime);
            nextStartTime += (double)clip.samples / clip.frequency;
            activeSource = 1 - activeSource;

            while (AudioSettings.dspTime < nextStartTime - ScheduleLead)
                yield return null;
    #endif
        }
    }

    //called once when the gameplay starts, called again when the transition is played.
    public void SetState(int newState)
    {
        musicState = newState;
    }

    public void PlaySfx(AudioClip clip)
    {
        AudioSource src = sfxPool[nextSfxIndex];
        nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Length;
        float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        src.pitch = randomPitch;
        src.PlayOneShot(clip);
    }
}