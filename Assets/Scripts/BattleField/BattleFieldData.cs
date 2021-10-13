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


    #region ##### Getter Setter #####

    public string Key => _key;
    public string Name => TranslatorStorage.Instance.GetTranslator<BattleFieldData>(Key, "Name");
    public Sprite icon => _icon;
    public Sprite background => _background;
    public string description => TranslatorStorage.Instance.GetTranslator<BattleFieldData>(Key, "Description");

    #endregion
}
