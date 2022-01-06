using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip MainMusic;
    public AudioClip BattleMusic;

    private DoubleAudioSource doubleAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        this.doubleAudioSource = GetComponent<DoubleAudioSource>();
        this.PlayMainMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBattleMusic()
    {
        if (this.BattleMusic)
        {
            this.doubleAudioSource.CrossFade(this.BattleMusic, 1f, 0.5f);
        }
    }

    public void PlayMainMusic()
    {
        if (this.MainMusic)
        {
            this.doubleAudioSource.CrossFade(this.MainMusic, 1f, 0.5f);
        }
    }
}
