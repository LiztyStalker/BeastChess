using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TribeData", menuName = "ScriptableObjects/TribeData")]
public class TribeData : ScriptableObject
{
    [SerializeField]
    private string _name;

    public string TribeName => _name;
}
