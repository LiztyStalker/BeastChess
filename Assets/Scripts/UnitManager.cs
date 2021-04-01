using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    UnitActor[] _units;

    [SerializeField]
    UIBar _uiBar;

    List<UnitActor> list = new List<UnitActor>();

    [HideInInspector]
    public int deadL;

    [HideInInspector]
    public int deadR;

    public void CreateUnit(ActorBlock actorBlock, TYPE_TEAM typeTeam)
    {
        var unit = Instantiate(_units[Random.Range(0, _units.Length)]);
        actorBlock.SetUnitActor(unit);
        unit.AddBar(Instantiate(_uiBar));
        unit.SetTypeTeam(typeTeam);
        unit.gameObject.SetActive(true);
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
                var nowBlock = fieldManager.FindActorBlock(unit);
                               

                //var attackDirectionX = (typeTeam == TYPE_TEAM.Left) ? nowBlock.coordinate.x + unit.rangeValue : nowBlock.coordinate.x - unit.rangeValue;
                //var movementDirectionX = (typeTeam == TYPE_TEAM.Left) ? nowBlock.coordinate.x + unit.movementValue : nowBlock.coordinate.x - unit.movementValue;

                var attackDirectionX = (typeTeam == TYPE_TEAM.Left) ? unit.rangeValue : -unit.rangeValue;
                var movementDirectionX = (typeTeam == TYPE_TEAM.Left) ? unit.movementValue : -unit.movementValue;


                var attactBlock = fieldManager.GetAttackBlock(nowBlock.coordinate, attackDirectionX, typeTeam);
                var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, movementDirectionX);


                //1회 공격
                if (attactBlock != null)
                {
                    attactBlock.unitActor.IncreaseHealth(unit.damageValue);
                    if (attactBlock.unitActor.IsDead())
                    {
                        var deadUnit = attactBlock.unitActor;
                        deadList.Add(deadUnit);
                        attactBlock.ResetUnitActor();
                    }
                }

                yield return new WaitForSeconds(Setting.FREAM_TIME);

                //1회 이동
                if (movementBlock != null) { 
                    nowBlock.ResetUnitActor();
                    movementBlock.SetUnitActor(unit);
                }

                yield return new WaitForSeconds(Setting.FREAM_TIME);

                attactBlock = fieldManager.GetAttackBlock(nowBlock.coordinate, attackDirectionX, typeTeam);

                //1회 추가 공격
                if (attactBlock != null)
                {
                    attactBlock.unitActor.IncreaseHealth(unit.damageValue);
                    if (attactBlock.unitActor.IsDead())
                    {
                        var deadUnit = attactBlock.unitActor;
                        deadList.Add(deadUnit);
                        attactBlock.ResetUnitActor();
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
            //Debug.Log("Destroy " + arr[i].gameObject);
            DestroyImmediate(arr[i].gameObject);

        }

    }
}
