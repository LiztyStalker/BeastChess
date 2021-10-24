using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioActor : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
    }

    private AudioManager.TYPE_AUDIO _typeAudio;

    public AudioManager.TYPE_AUDIO typeAudio => _typeAudio;

    public bool IsEqualKey(string key) => AudioSource.clip.name == key;

    public bool IsPlaying() => AudioSource.isPlaying;

    public void SetData(AudioClip clip, bool isLoop = false)
    {
        AudioSource.clip = clip;
        AudioSource.loop = isLoop;
    }

    public void SetData(AudioManager.TYPE_AUDIO typeAudio)
    {
        _typeAudio = typeAudio;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.clip = null;
        AudioSource.Stop();
        gameObject.SetActive(false);
        _stoppedEvent?.Invoke(this);
    }

    private void Update()
    {
        if (!AudioSource.isPlaying)
        {
            Stop();
        }
    }

    #region ##### Listener #####
    private System.Action<AudioActor> _stoppedEvent;
    public void SetOnStoppedListener(System.Action<AudioActor> act) => _stoppedEvent = act; 
    #endregion

}
