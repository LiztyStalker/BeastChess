using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/EffectData")]
public class EffectData : ScriptableObject
{
    [SerializeField]
    private GameObject _effectPrefab;
    public GameObject effectPrefab => _effectPrefab;
}
