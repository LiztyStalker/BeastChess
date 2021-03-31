using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    UnitActor _unitActor;

    List<UnitActor> list = new List<UnitActor>();

    [HideInInspector]
    public int deadL;

    [HideInInspector]
    public int deadR;

    public void CreateUnit(ActorBlock actorBlock, TYPE_TEAM typeTeam)
    {
        var unit = Instantiate(_unitActor);
        actorBlock.SetUnitActor(unit);
        unit.gameObject.SetActive(true);
        unit.SetTypeTeam(typeTeam);
        list.Add(unit);
    }

    public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        List<UnitActor> deadList = new List<UnitActor>();

        for(int i = 0; i < list.Count; i++)
        {
            var unit = list[i];
            if (unit.typeTeam == typeTeam)
            {
                var prevblock = fieldManager.FindActorBlock(unit);

                var directionX = (typeTeam == TYPE_TEAM.Left) ? prevblock.coordinate.x + 1 : prevblock.coordinate.x - 1;

                var nextblock = fieldManager.GetBlock(directionX, prevblock.coordinate.y);

                if (nextblock != null) { 
                    if (nextblock.unitActor == null)
                    {
                        prevblock.ResetUnitActor();
                        nextblock.SetUnitActor(unit);
                    }
                    else
                    {
                        if(nextblock.unitActor.typeTeam != typeTeam)
                        {
                            nextblock.unitActor.IncreaseHealth(unit.damageValue);
                            if (nextblock.unitActor.IsDead())
                            {
                                var deadUnit = nextblock.unitActor;
                                deadList.Add(deadUnit);
                                prevblock.ResetUnitActor();
                                nextblock.SetUnitActor(unit);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(Setting.FREAM_TIME);
        }

        var arr = deadList.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            switch (arr[i].typeTeam)
            {
                case TYPE_TEAM.Left:
                    deadL++;
                    break;
                case TYPE_TEAM.Right:
                    deadR++;
                    break;
            }
            list.Remove(arr[i]);
            DestroyImmediate(arr[i].gameObject);

        }

    }
}
