using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommanderSkill : MonoBehaviour
{
    [SerializeField]
    UICommanderSkillIcon _block;

    [SerializeField]
    Transform tr;

    private List<UICommanderSkillIcon> _list = new List<UICommanderSkillIcon>();

    public void Initialize()
    {
        _block.Hide();
    }

    public void SetSkill(SkillData[] skills)
    {
        for(int i = 0; i < skills.Length; i++)
        {

            UICommanderSkillIcon block = null;
            if(i < _list.Count)
            {
                block = _list[i];
            }
            else
            {
                block = Instantiate(_block);
                block.transform.SetParent(tr);
                block.SetOnSkillInformationListener(ShowSkillInformationEvent);
                _list.Add(block);
            }
            block.SetData(skills[i]);
        }

        for(int i = skills.Length; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    #region ##### Listener #####
    private void ShowSkillInformationEvent(SkillData skillData, Vector2 screenPosition)
    {
        _skillInforEvent?.Invoke(skillData, screenPosition);
    }

    private System.Action<SkillData, Vector2> _skillInforEvent;

    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _skillInforEvent = act;

    #endregion
}
