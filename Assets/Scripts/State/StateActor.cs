using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActor
{

    private struct StateElement
    {
        public IState state;
        public ICaster caster;
        public int turnCount;


        public void Turn()
        {
            if(turnCount > 0) turnCount--;
        }

        public bool IsEmptyTurn() => turnCount == 0;

        internal StateElement(ICaster caster, IState state)
        {
            //Debug.Log(state.GetType().UnderlyingSystemType);
            this.state = state;
            this.caster = caster;
            this.turnCount = 0;
        }
    }




    private Dictionary<ICaster, List<StateElement>> _stateDic = new Dictionary<ICaster, List<StateElement>>();


    public int GetValue<T>(int defaultValue) where T : IState
    {
        var rate = 1f;
        var value = defaultValue;

        foreach(var list in _stateDic.Values)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var state = list[i].state;
                Debug.Log(state + " " + typeof(T));
                if (state is T)
                {
                    switch (state.typeValue)
                    {
                        case State.TYPE_VALUE.Value:
                            value += (int)state.value;
                            Debug.Log("Add " + value);
                            break;
                        case State.TYPE_VALUE.Rate:
                            rate += state.value;
                            break;
                    }
                }
            }
        }
        return (int)(((float)value) * rate);
    }

    public void Add(ICaster caster, SkillData[] skills)
    {
        if (!_stateDic.ContainsKey(caster))
        {
            _stateDic.Add(caster, new List<StateElement>());

            for (int i = 0; i < skills.Length; i++)
            {
                var skill = skills[i];                
                var list = skill.GetStateArray();

                for (int j = 0; j < list.Length; j++)
                {
                    var data = list[j] as StateValueAttack;
                    Debug.Log(data.value);
                    _stateDic[caster].Add(new StateElement(caster, list[j]));
                }
            }
        }
    }

    public void Remove(ICaster caster)
    {

        if (_stateDic.ContainsKey(caster))
        {
            _stateDic.Remove(caster);
        }
    }

    public void Turn()
    {

        foreach (var list in _stateDic.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsEmptyTurn())
                    list.RemoveAt(i);
                else
                    list[i].Turn();
            }
        }
    }

    public void Clear()
    {
        _stateDic.Clear();
    }
}


