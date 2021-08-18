using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActor
{

    private class SkillElement
    {
        public SkillData skillData;
        public ICaster caster;
        public int turnCount;
        public int overlapCount;


        public void Turn()
        {
            if(turnCount > 0) turnCount--;
        }

        public bool IsOverlaped() => skillData.isOverlapped;

        public bool IsEmptyTurn() => turnCount == 0;

        internal SkillElement(ICaster caster, SkillData skillData)
        {
            this.skillData = skillData;
            this.caster = caster;
            this.turnCount = skillData.turnCount;
        }
    }




    private List<SkillElement> _skillList = new List<SkillElement>();


    public int GetValue<T>(int defaultValue) where T : IState
    {
        var rate = 1f;
        var value = defaultValue;

        for(int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].skillData.Calculate<T>(ref rate, ref value);
        }
        Debug.Log(value + " " + rate);
        return (int)(((float)value) * rate);
    }

    public void AddSkill(ICaster caster, SkillData skillData)
    {
        for(int i = 0; i < _skillList.Count; i++)
        {
            var tmpSkillData = _skillList[i].skillData;
            var tmpCaster = _skillList[i].caster;
            //ÁßÃ¸È®ÀÎ
            //
        }

        _skillList.Add(new SkillElement(caster, skillData));
    }

    //public void AddSkills(ICaster caster, SkillData[] skills)
    //{
    //    if (!_casterDic.ContainsKey(caster))
    //    {
    //        _casterDic.Add(caster, new List<SkillElement>());

    //        for (int i = 0; i < skills.Length; i++)
    //        {
    //            _casterDic[caster].Add(new SkillElement(caster, skills[i]));
    //        }
    //    }
    //}

    //public void RemoveSkill(ICaster caster)
    //{

    //    if (_casterDic.ContainsKey(caster))
    //    {
    //        _casterDic.Remove(caster);
    //    }
    //}

    public void Turn()
    {

        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i].IsEmptyTurn())
                _skillList.RemoveAt(i);
            else
                _skillList[i].Turn();
        }
    }

    public void Clear()
    {
        _skillList.Clear();
    }
}


