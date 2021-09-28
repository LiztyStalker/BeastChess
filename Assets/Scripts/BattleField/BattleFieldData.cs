using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "BattleFieldData", menuName = "ScriptableObjects/BattleFieldData")]

public class BattleFieldData : ScriptableObject
{
    [SerializeField]
    private string _key;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private Sprite _background;

    [SerializeField]
    private string _description;

    public string Key => _key;
    public Sprite icon => _icon;
    public Sprite background => _background;
    public string description => _description;
}
