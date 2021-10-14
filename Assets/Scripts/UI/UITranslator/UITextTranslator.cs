using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextTranslator : MonoBehaviour
{
    [SerializeField]
    private string _dicKey;

    [SerializeField]
    private string _key;

    [SerializeField]
    private string _verb;

    private Text _text;

    private Text Text => GetComponent<Text>();

    private void OnEnable()
    {
        SetTranslate();
    }
    private void SetTranslate()
    {
        Text.text = TranslatorStorage.Instance.GetTranslator(_dicKey, _key, _verb);
    }

}
