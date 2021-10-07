using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitSkillLayout : MonoBehaviour
{
    [SerializeField]
    UISkillInformation _uiSkillInformation;

    [SerializeField]
    UISkillIcon _skillIcon;

    [SerializeField]
    Transform _tr;

    List<UISkillIcon> _list = new List<UISkillIcon>();

    public void Initialize()
    {
        Hide();
    }

    public void Show(UnitCard uCard)
    {
        Clear();
        for (int i = 0; i < uCard.Skills.Length; i++)
        {
            var block = GetBlock(_tr);
            block.SetData(uCard.Skills[i]);
            block.Show();
        }
        _uiSkillInformation.Hide();
    }

    public void Hide()
    {
        Clear();
        _uiSkillInformation.Hide();
    }

    private void Clear()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    private UISkillIcon GetBlock(Transform tr)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].isActiveAndEnabled) return _list[i];
        }

        var block = Instantiate(_skillIcon);
        block.transform.SetParent(tr);
        block.transform.localScale = Vector3.one;
        block._skillInformationEvent += _uiSkillInformation.Show;
        _list.Add(block);
        return block;
    }

}
