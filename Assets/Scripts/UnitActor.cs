using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TYPE_TEAM { Left, Right}

public class UnitActor : MonoBehaviour
{

    TYPE_TEAM _typeTeam;

    [Range(1, 1000)]
    [SerializeField]
    int _healthValue = 100;

    int _nowHealthValue;

    [SerializeField]
    int _damageValue = 35;

    [SerializeField]
    int _movementValue = 1;

    [SerializeField]
    int _costValue = 1;

    [SerializeField]
    int _rangeValue = 1;

    [SerializeField]
    SpriteRenderer _renderer;
         
    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        _typeTeam = typeTeam;
        _renderer.color = GetTeamColor(typeTeam);

        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                transform.localScale = Vector3.one;
                break;
            case TYPE_TEAM.Right:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;
        }
    }


    private Color GetTeamColor(TYPE_TEAM typeTeam)
    {
        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                return Color.blue;
            case TYPE_TEAM.Right:
                return Color.red;
        }
        return Color.black;
    }

    private void Start()
    {
        _nowHealthValue = _healthValue;
    }

    public void IncreaseHealth(int value)
    {
        if (_nowHealthValue - value < 0)
            _nowHealthValue = 0;
        else
            _nowHealthValue -= value;

        _renderer.color = Color.Lerp(Color.white, GetTeamColor(_typeTeam), HealthRate());

    }

    public float HealthRate() => (float)_nowHealthValue / (float)_healthValue;

    public bool IsDead() => _nowHealthValue == 0;

    public int damageValue => _damageValue;

    public int movementValue => _movementValue;

    public int rangeValue => _rangeValue;

    public TYPE_TEAM typeTeam => _typeTeam;



}
