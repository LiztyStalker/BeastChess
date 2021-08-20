using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillElement
{
    public SkillData skillData;
    public ICaster caster;
    public int turnCount;
    public int overlapCount;


    public void Turn()
    {
        if (turnCount > 0) turnCount--;
    }

    public bool IsOverlaped() => skillData.isOverlapped;

    public void AddOverlapCount()
    {
        if(skillData.overlapCount == 0 || overlapCount + 1 <= skillData.overlapCount)
            overlapCount++;
    }

    public void InitializeTurnCount() => turnCount = skillData.turnCount;

    public bool IsEmptyTurn() => turnCount <= 0;

    public bool IsLifeSpan(TYPE_SKILL_LIFE_SPAN typeSkillLifeSpan) => skillData.typeSkillLifeSpan == typeSkillLifeSpan;

    internal SkillElement(ICaster caster, SkillData skillData)
    {
        this.skillData = skillData;
        this.caster = caster;
        overlapCount = 1;
        InitializeTurnCount();
    }
}

public class SkillActor
{

    private List<SkillElement> _skillList = new List<SkillElement>();
    private Dictionary<SkillData, SkillElement> _skillDic = new Dictionary<SkillData, SkillElement>();
    private Dictionary<ICaster, SkillElement> _casterDic = new Dictionary<ICaster, SkillElement>();

    public void ShowSkill(UIBar uiBar)
    {
        uiBar.ShowSkill(_skillList.ToArray());
    }


    public int GetValue<T>(int defaultValue) where T : IState
    {
        var rate = 1f;
        var value = defaultValue;

        for(int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].skillData.Calculate<T>(ref rate, ref value, _skillList[i].overlapCount);
        }
        //Debug.Log(value + " " + rate);
        return (int)(((float)value) * rate);
    }

    public void AddSkill(ICaster caster, SkillData skillData)
    {

        if (_skillDic.ContainsKey(skillData))
        {
            if (skillData.isOverlapped)
            {
                _skillDic[skillData].AddOverlapCount();
                _skillDic[skillData].InitializeTurnCount();
                return;
            }
        }
        else
        {
            var element = new SkillElement(caster, skillData);
            _skillList.Add(element);
            _skillDic.Add(skillData, element);
            _casterDic.Add(caster, element);
        }
    }
    
    public void Turn(UIBar uiBar)
    {

        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i].IsLifeSpan(TYPE_SKILL_LIFE_SPAN.Turn))
            {
                if (_skillList[i].IsEmptyTurn())
                {
                    Remove(_skillList[i]);
                }
                else
                    _skillList[i].Turn();
            }
        }
        uiBar.ShowSkill(_skillList.ToArray());
    }

    private void Remove(SkillElement element)
    {
        var skillData = element.skillData;
        var caster = element.caster;
        _skillList.Remove(element);
        _skillDic.Remove(skillData);
        _casterDic.Remove(caster);
    }

    public void Remove(ICaster caster)
    {
        if (_casterDic.ContainsKey(caster))
        {
            var element = _casterDic[caster];
            var skillData = element.skillData;
            _skillList.Remove(element);
            _skillDic.Remove(skillData);
            _casterDic.Remove(caster);
        }
    }

    public void Clear()
    {
        _skillList.Clear();
        _casterDic.Clear();
        _skillDic.Clear();
    }
}


