using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitSkillLayout : MonoBehaviour
{
    [SerializeField]
    private UISkillIcon _skillIcon;

    [SerializeField]
    private Transform _tr;

    List<UISkillIcon> _list = new List<UISkillIcon>();

    public void Initialize()
    {
        Hide();
    }

    public void CleanUp()
    {
        _list.Clear();
    }

    public void Show(UnitCard uCard)
    {
        Clear();
        if (uCard.Skills != null)
        {
            for (int i = 0; i < uCard.Skills.Length; i++)
            {
                var block = GetBlock(_tr);
                block.SetData(uCard.Skills[i]);
                block.Show();
            }
        }
    }

    public void Hide()
    {
        Clear();
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
        block.SetOnSkillInformationEvent(_skillInformationEvent);
        _list.Add(block);
        return block;
    }

    #region ##### Listener #####
    private System.Action<SkillData, Vector2> _skillInformationEvent;

    public void SetOnSkillInformationEvent(System.Action<SkillData, Vector2> act) => _skillInformationEvent = act;
    #endregion

}
