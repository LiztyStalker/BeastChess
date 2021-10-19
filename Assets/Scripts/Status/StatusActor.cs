using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusElement
{
    public StatusData StatusData { get; private set; }
    public ICaster Caster { get; private set; }
    public int TurnCount { get; private set; }
    public int OverlapCount { get; private set; }


    public void Turn(IUnitActor uActor)
    {
        StatusData.Turn(uActor);
        if (TurnCount > 0) TurnCount--;
    }

    public bool IsOverlaped() => false;// skillData.isOverlapped;

    public void AddOverlapCount()
    {
        if(StatusData.OverlapCount == 0 || OverlapCount + 1 <= StatusData.OverlapCount)
            OverlapCount++;
    }

    public void SubtractOverlapCount()
    {
        if (StatusData.OverlapCount == 0 || OverlapCount - 1 > 0)
            OverlapCount--;
    }

    public void InitializeTurnCount() => TurnCount = StatusData.TurnCount;
    public bool IsEmptyTurn() => TurnCount <= 0;

    public bool IsTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN typeStatusLifeSpan) => StatusData.typeStatusLifeSpan == typeStatusLifeSpan;

    private StatusElement(ICaster caster, StatusData statusData)
    {
        //Debug.Log("NewStatusElement");
        StatusData = statusData;
        Caster = caster;
        OverlapCount = 1;
        InitializeTurnCount();
    }

    public static StatusElement Create(ICaster caster, StatusData statusData)
    {
        return new StatusElement(caster, statusData);
    }

}

public class StatusActor
{
    //전체 스킬
    private List<StatusElement> _statusElementList = new List<StatusElement>();

    // 스킬 적용
    private Dictionary<StatusData, StatusElement> _statusDataDic = new Dictionary<StatusData, StatusElement>();

    //시전자별 적용 스킬
    private Dictionary<ICaster, StatusElement> _casterToStatusDic = new Dictionary<ICaster, StatusElement>();

    //스킬별 적용된 시전자 리스트
    private Dictionary<StatusElement, List<ICaster>> _statusToCasterDic = new Dictionary<StatusElement, List<ICaster>>();


    public StatusElement[] GetStatusElementArray() => _statusElementList.ToArray();

    public void ShowStatusDataArray(UIBar uiBar)
    {
        uiBar.ShowStatusDataArray(_statusElementList.ToArray());
    }

    public bool IsHasStatus<T>() where T : IStatus
    {
        for (int i = 0; i < _statusElementList.Count; i++)
        {
            if (_statusElementList[i].StatusData.IsHasStatus<T>())
            {
                return true;
            }
        }
        return false;
    }

    public int GetValue<T>(int defaultValue) where T : IStatusValue
    {
        return (int)GetValue<T>((float)defaultValue);
    }

    public float GetValue<T>(float defaultValue) where T : IStatusValue
    {
        var rate = 1f;
        var value = defaultValue;

        for (int i = 0; i < _statusElementList.Count; i++)
        {
            _statusElementList[i].StatusData.Calculate<T>(ref rate, ref value, _statusElementList[i].OverlapCount);
        }
        //Debug.Log(value + " " + rate);
        return value * rate;
    }

    public void AddStatusData(ICaster caster, StatusData statusData)
    {
//        Debug.Log("AddStatusData " + statusData.GetInstanceID());
        if (_statusDataDic.ContainsKey(statusData))
        {
            if (statusData.IsOverlapped)
            {
                _statusDataDic[statusData].AddOverlapCount();
                _statusDataDic[statusData].InitializeTurnCount();
                return;
            }
        }
        else
        {
            var element = StatusElement.Create(caster, statusData);
            _statusElementList.Add(element);
            _statusDataDic.Add(statusData, element);
            AddCaster(caster, element);
        }
    }

    private void AddCaster(ICaster caster, StatusElement element)
    {
        if (!_casterToStatusDic.ContainsKey(caster))
            _casterToStatusDic.Add(caster, element);

        if (!_statusToCasterDic.ContainsKey(element))
        {
            _statusToCasterDic.Add(element, new List<ICaster>());
        }

        _statusToCasterDic[element].Add(caster);
    }
    
    public void Turn(IUnitActor uActor, UIBar uiBar)
    {
        for (int i = 0; i < _statusElementList.Count; i++)
        {
            if (_statusElementList[i].IsTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn))
            {
                _statusElementList[i].Turn(uActor);

                if (_statusElementList[i].IsEmptyTurn())
                {
                    RemoveStatusData(_statusElementList[i]);
                }
            }
        }
        
        uiBar?.ShowStatusDataArray(_statusElementList.ToArray());
    }


    /// <summary>
    /// 스킬 턴 종료시 제거
    /// </summary>
    /// <param name="element"></param>
    private void RemoveStatusData(StatusElement element)
    {
        var statusData = element.StatusData;
        var caster = element.Caster;
        _statusElementList.Remove(element);
        _statusDataDic.Remove(statusData);
        RemoveCaster(element);
    }

    private void RemoveCaster(StatusElement element)
    {
        if (_statusToCasterDic.ContainsKey(element))
        {
            if (_casterToStatusDic.ContainsKey(element.Caster))
            {
                _casterToStatusDic.Remove(element.Caster);
                _statusToCasterDic.Remove(element);
            }
#if UNITY_EDITOR || UNITY_DEBUG
            else
            {
                Debug.Log($"SkillToCasterDic에 등록되지 않았습니다 {element.Caster}");
            }
#endif
        }
#if UNITY_EDITOR || UNITY_DEBUG
        else
        {
            Debug.Log($"SkillToCasterDic에 등록되지 않았습니다 {element.StatusData}");
        }
#endif
    }



    //public void RemovePreActiveSkill(UIBar uiBar)
    //{
    //    //for(int i = 0; i < _skillList.Count; i++)
    //    //{
    //    //    var element = _skillList[i];
    //    //    if (element.statusData.typeSkillActivate == TYPE_SKILL_ACTIVATE.PreActive)
    //    //    {
    //    //        Remove(element);
    //    //    }
    //    //}
    //    uiBar.ShowStatusDataArray(_statusElementList.ToArray());
    //}

    public void RemoveStatusData(StatusData statusData)
    {
        if (_statusDataDic.ContainsKey(statusData))
        {
            var element = _statusDataDic[statusData];
            _statusElementList.Remove(element);
            _statusToCasterDic.Remove(element);
            RemoveCaster(element);
        }
    }

    public void RemoveStatusData(ICaster caster)
    {
        if (_casterToStatusDic.ContainsKey(caster))
        {
            var element = _casterToStatusDic[caster];
            var statusData = element.StatusData;
            _statusElementList.Remove(element);
            _statusDataDic.Remove(statusData);
            RemoveCaster(element);
        }
    }

    /// <summary>
    /// 시전자 사망시 제거
    /// </summary>
    /// <param name="element"></param>
    /// <param name="caster"></param> 
    public void RemoveStatusData(ICaster caster, UIBar uiBar)
    {
        if (_casterToStatusDic.ContainsKey(caster))
        {

            //시전자가 사용한 스킬 제거
            var element = _casterToStatusDic[caster];
            var statusData = element.StatusData;

            //시전자 생명주기이면 제거
            //if (skillData.typeSkillLifeSpan == TYPE_STATUS_LIFE_SPAN.Caster)
            //{


            //    _casterToSkillDic.Remove(caster);

            //    //중첩 카운트 내리기
            //    if (skillData.isOverlapped)
            //    {
            //        element.SubtractOverlapCount();
            //    }

            //    //연결된 시전자
            //    if (_skillToCasterDic.ContainsKey(element))
            //    {
            //        _skillToCasterDic[element].Remove(caster);

            //        //시전자가 없으면 완전 삭제
            //        if (_skillToCasterDic[element].Count == 0)
            //        {
            //            _skillDic.Remove(skillData);
            //            _skillToCasterDic.Remove(element);
            //            _skillList.Remove(element);
            //        }
            //    }
            //}
        }

        uiBar.ShowStatusDataArray(_statusElementList.ToArray());

    }


    public void Clear()
    {
        _statusElementList.Clear();
        _statusToCasterDic.Clear();
        _casterToStatusDic.Clear();
        _statusDataDic.Clear();
    }

    public StatusElement GetStatusElement(StatusData statusData)
    {
        for(int i = 0; i < _statusElementList.Count; i++)
        {
            if (_statusElementList[i].StatusData == statusData)
                return _statusElementList[i];
        }
        return null;
    }
    public bool IsHasStatusData(StatusData.TYPE_STATUS_LIFE_SPAN typeStatusLifeSpan)
    {
        for(int i = 0; i < _statusElementList.Count; i++)
        {
            if (_statusElementList[i].StatusData.typeStatusLifeSpan == typeStatusLifeSpan) return true;
        }
        return false;
    }

    public bool IsHasStatusData(StatusData statusData)
    {
        for (int i = 0; i < _statusElementList.Count; i++)
        {
            if (_statusElementList[i].StatusData == statusData) return true;
        }
        return false;
    }

}


