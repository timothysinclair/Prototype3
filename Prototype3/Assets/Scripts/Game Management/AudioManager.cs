using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

    [SerializeField] private AudioClip[] audioClips;

    private Dictionary<string, AudioClip> audioClipsDict;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        audioClipsDict = new Dictionary<string, AudioClip>();

        for (int i = 0; i < audioClips.Length; i++)
        {
            audioClipsDict.Add(audioClips[i].name, audioClips[i]);
        }
    }

    public AudioClip GetAudioClip(string name)
    {
        return audioClipsDict[name];
    }


}
