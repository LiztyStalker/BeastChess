using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{



    public enum TYPE_AUDIO { BGM, SFX, Environment, UI}

    private static GameObject _gameObject;

    private static GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject();
                _gameObject.transform.position = Vector3.zero;
                _gameObject.name = "SoundManager";
                Object.DontDestroyOnLoad(_gameObject);
            }
            return _gameObject;
        }
    }

    private static Dictionary<TYPE_AUDIO, List<AudioActor>> _activateDic = new Dictionary<TYPE_AUDIO, List<AudioActor>>();

    private static List<AudioActor> _inactiveList = new List<AudioActor>();

    private static AudioClip GetClip(string clipKey) => DataStorage.Instance.GetDataOrNull<AudioClip>(clipKey, null, null);

    /// <summary>
    /// EffectData를 GameObject Instance화 합니다
    /// EditMode : 실행되지 않습니다
    /// Play : GameObject가 생성됩니다
    /// </summary>
    /// <param name="effectData"></param>
    /// <param name="position"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static AudioActor ActivateAudio(string clipKey, TYPE_AUDIO typeAudio, bool isLoop = false)
    {
        if (Application.isPlaying)
        {
            if (!string.IsNullOrEmpty(clipKey))
            {
                var clip = GetClip(clipKey);
                return ActivateAudio(clip, typeAudio, isLoop);
            }
        }
        return null;
    }

    /// <summary>
    /// EffectData를 GameObject Instance화 합니다
    /// EditMode : 실행되지 않습니다
    /// Play : GameObject가 생성됩니다
    /// </summary>
    /// <param name="effectData"></param>
    /// <param name="position"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static AudioActor ActivateAudio(AudioClip clip, TYPE_AUDIO typeAudio, bool isLoop = false)
    {
        if (Application.isPlaying)
        {
            if (clip != null)
            {
                var actor = GetActor(typeAudio);
                actor.name = $"AudioActor_{typeAudio}_{clip.name}";
                actor.SetData(clip, isLoop);
                actor.SetData(typeAudio);
                actor.Play();
                return actor;
            }
        }
        return null;
    }

    /// <summary>
    /// EffectActor를 종료합니다
    /// </summary>
    /// <param name="effectData"></param>
    public static void InactiveAudio(AudioActor actor)
    {
        actor.Stop();
    }

    private static AudioActor GetActor(TYPE_AUDIO typeAudio)
    {
        AudioActor actor = null;
        for (int i = 0; i < _inactiveList.Count; i++)
        {
            actor = _inactiveList[i];
        }


        if (actor == null)
        {
            actor = CreateActor();
            if (!_activateDic.ContainsKey(typeAudio))
            {
                _activateDic.Add(typeAudio, new List<AudioActor>());
            }
        }
        else
        {
            _inactiveList.Remove(actor);
        }
        _activateDic[typeAudio].Add(actor);
        return actor;
    }

    private static AudioActor CreateActor()
    {
        var gameObejct = new GameObject();
        var actor = gameObejct.AddComponent<AudioActor>();
        actor.SetOnStoppedListener(ReturnActor);
        actor.transform.SetParent(gameObject.transform);
        return actor;
    }

    private static void ReturnActor(AudioActor actor)
    {
        var typeAudio = actor.typeAudio;
        if (_activateDic.ContainsKey(typeAudio))
        {
            var list = _activateDic[typeAudio];
            if (list.Contains(actor))
            {
                list.Remove(actor);
                _inactiveList.Add(actor);
            }
        }
    }

}
