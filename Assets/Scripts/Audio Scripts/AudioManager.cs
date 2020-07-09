using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] bgm;
    public AudioSource[] sfx;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySFX(int soundToPlay)
    {
        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        // If it's the same track, don't reset the bgm
        if (!bgm[musicToPlay].isPlaying)
        {
            this.StopMusic();

            if (musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        foreach (var music in bgm)
        {
            music.Stop();
        }
    }
}
