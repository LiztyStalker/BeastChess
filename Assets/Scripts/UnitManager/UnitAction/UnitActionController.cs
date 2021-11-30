using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitAction 및 행동에 대한 Coroutine이나 메소드는 따로 분리해서 진행할 필요 있음
/// </summary>

public class UnitActionController : CustomYieldInstruction
{

    bool _isRunning = false;

    public bool isRunning { get { return _isRunning; } set { _isRunning = value; /*Debug.Log("Set IsRunning" + _isRunning);*/ } }

    public override bool keepWaiting
    {
        get
        {
            Debug.Log("IsRunning " + isRunning);
            return isRunning;
        }
    }

    IEnumerator enumerator1;
    IEnumerator enumerator2;
    Coroutine coroutine;
    MonoBehaviour mono;

    [System.Obsolete("SetUnitAction<T>로 변경 예정")]
    public void SetUnitAction(MonoBehaviour mono, IEnumerator e1, IEnumerator e2)
    {
        this.enumerator1 = e1;
        this.enumerator2 = e2;
        this.mono = mono;
        coroutine = mono.StartCoroutine(ActionCoroutine());
    }

    public void SetUnitAction<T>(MonoBehaviour mono, UnitActor unitActor, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback) where T : IUnitActionState
    {
        this.enumerator1 = GetUnitAction<T>().ActionCoroutine(unitActor, this, unitActionData, castSkillsCallback);
        this.enumerator2 = WaitUntilAction();
        this.mono = mono;
        coroutine = mono.StartCoroutine(ActionCoroutine());
    }

    private IEnumerator ActionCoroutine()
    {
        isRunning = true;
        yield return mono.StartCoroutine(enumerator1);
        if (enumerator2 != null)
            yield return mono.StartCoroutine(enumerator2);
        isRunning = false;
    }

    private Dictionary<string, IUnitActionState> _unitActionDic = new Dictionary<string, IUnitActionState>();

    private IUnitActionState GetUnitAction<T>() where T : IUnitActionState
    {
        if (!_unitActionDic.ContainsKey(typeof(T).Name))
        {
            _unitActionDic.Add(typeof(T).Name, (T)System.Activator.CreateInstance<T>());
        }
        return _unitActionDic[typeof(T).Name];
    }


    private IEnumerator WaitUntilAction()
    {
        //특정 조건이 성공할 때까지 대기 true가 나오면 yield 종료
        yield return new WaitUntil(() => !_isRunning);
    }
}
