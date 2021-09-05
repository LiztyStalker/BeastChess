using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusElement
{
    public SkillData skillData;
    public ICaster caster;
    public int turnCount { get; private set; }
    public int overlapCount { get; private set; }


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

    public void SubtractOverlapCount()
    {
        if (skillData.overlapCount == 0 || overlapCount - 1 > 0)
            overlapCount--;
    }

    public void InitializeTurnCount() => turnCount = skillData.turnCount;

    public bool IsEmptyTurn() => turnCount <= 0;

    public bool IsLifeSpan(TYPE_STATUS_LIFE_SPAN typeSkillLifeSpan) => skillData.typeSkillLifeSpan == typeSkillLifeSpan;

    internal StatusElement(ICaster caster, SkillData skillData)
    {
        this.skillData = skillData;
        this.caster = caster;
        overlapCount = 1;
        InitializeTurnCount();
    }
}

public class StatusActor
{
    //��ü ��ų
    private List<StatusElement> _skillList = new List<StatusElement>();

    // ��ų ����
    private Dictionary<SkillData, StatusElement> _skillDic = new Dictionary<SkillData, StatusElement>();

    //�����ں� ���� ��ų
    private Dictionary<ICaster, StatusElement> _casterToSkillDic = new Dictionary<ICaster, StatusElement>();

    //��ų�� ����� ������ ����Ʈ
    private Dictionary<StatusElement, List<ICaster>> _skillToCasterDic = new Dictionary<StatusElement, List<ICaster>>();




    public void ShowSkill(UIBar uiBar)
    {
        uiBar.ShowSkill(_skillList.ToArray());
    }


    public int GetValue<T>(int defaultValue) where T : IStatus
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

    public void Add(ICaster caster, SkillData skillData)
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
            var element = new StatusElement(caster, skillData);
            _skillList.Add(element);
            _skillDic.Add(skillData, element);
            AddCaster(caster, element);
        }
    }

    private void AddCaster(ICaster caster, StatusElement element)
    {
        if (!_casterToSkillDic.ContainsKey(caster))
            _casterToSkillDic.Add(caster, element);

        if (!_skillToCasterDic.ContainsKey(element))
        {
            _skillToCasterDic.Add(element, new List<ICaster>());
        }

        _skillToCasterDic[element].Add(caster);
    }
    
    public void Turn(UIBar uiBar)
    {

        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i].IsLifeSpan(TYPE_STATUS_LIFE_SPAN.Turn))
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


    /// <summary>
    /// ��ų �� ����� ����
    /// </summary>
    /// <param name="element"></param>
    private void Remove(StatusElement element)
    {
        var skillData = element.skillData;
        var caster = element.caster;
        _skillList.Remove(element);
        _skillDic.Remove(skillData);
        RemoveCaster(element);
    }

    private void RemoveCaster(StatusElement element)
    {
        if (_skillToCasterDic.ContainsKey(element))
        {
            if (_casterToSkillDic.ContainsKey(element.caster))
            {
                _casterToSkillDic.Remove(element.caster);
                _skillToCasterDic.Remove(element);
            }
#if UNITY_EDITOR || UNITY_DEBUG
            else
            {
                Debug.Log($"SkillToCasterDic�� ��ϵ��� �ʾҽ��ϴ� {element.caster}");
            }
#endif
        }
#if UNITY_EDITOR || UNITY_DEBUG
        else
        {
            Debug.Log($"SkillToCasterDic�� ��ϵ��� �ʾҽ��ϴ� {element.skillData.key}");
        }
#endif
    }



    public void RemovePreActiveSkill(UIBar uiBar)
    {
        for(int i = 0; i < _skillList.Count; i++)
        {
            var element = _skillList[i];
            if (element.skillData.typeSkillActivate == TYPE_SKILL_ACTIVATE.PreActive)
            {
                Remove(element);
            }
        }
        uiBar.ShowSkill(_skillList.ToArray());
    }

    /// <summary>
    /// ������ ����� ����
    /// </summary>
    /// <param name="element"></param>
    /// <param name="caster"></param> 
    public void Remove(ICaster caster, UIBar uiBar)
    {
        if (_casterToSkillDic.ContainsKey(caster))
        {

            //�����ڰ� ����� ��ų ����
            var element = _casterToSkillDic[caster];
            var skillData = element.skillData;

            //������ �����ֱ��̸� ����
            if (skillData.typeSkillLifeSpan == TYPE_STATUS_LIFE_SPAN.Caster)
            {


                _casterToSkillDic.Remove(caster);

                //��ø ī��Ʈ ������
                if (skillData.isOverlapped)
                {
                    element.SubtractOverlapCount();
                }

                //����� ������
                if (_skillToCasterDic.ContainsKey(element))
                {
                    _skillToCasterDic[element].Remove(caster);

                    //�����ڰ� ������ ���� ����
                    if (_skillToCasterDic[element].Count == 0)
                    {
                        _skillDic.Remove(skillData);
                        _skillToCasterDic.Remove(element);
                        _skillList.Remove(element);
                    }
                }
            }
        }

        uiBar.ShowSkill(_skillList.ToArray());

    }

    public void Clear()
    {
        _skillList.Clear();
        _skillToCasterDic.Clear();
        _casterToSkillDic.Clear();
        _skillDic.Clear();
    }
}


