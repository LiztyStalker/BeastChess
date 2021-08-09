using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommanderSkill : MonoBehaviour
{
    [SerializeField]
    UICommanderSkillIcon _block;

    [SerializeField]
    Transform tr;

    List<UICommanderSkillIcon> list = new List<UICommanderSkillIcon>();

    public void Initialize()
    {
        _block.Hide();
    }

    public void SetSkill(SkillData[] skills)
    {
        for(int i = 0; i < skills.Length; i++)
        {

            UICommanderSkillIcon block = null;
            if(i < list.Count)
            {
                block = list[i];
            }
            else
            {
                block = Instantiate(_block);
                block.transform.SetParent(tr);
                list.Add(block);
            }
            block.SetData(skills[i]);
        }

        for(int i = skills.Length; i < list.Count; i++)
        {
            list[i].Hide();
        }
    }
}
