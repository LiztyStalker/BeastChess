using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

public enum TYPE_SKILL_CAST { DeployCast, PreCast, AttackCast, AttackedCast}

public enum TYPE_SKILL_CAST_CONDITION { None, Health}

public enum TYPE_SKILL_CAST_CONDITION_COMPARE {GreaterThan, LessThan, Equal, NotEqual}

public enum TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE { Value, Rate }

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{


    #region ##### SkillDataProcess #####

    [System.Serializable]
    public class SkillDataProcess
    {
        public enum TYPE_USED_DATA { NotUsed, Used, Used_Callback }

        [Tooltip("��������Ʈ �Դϴ�")]
        [SerializeField]
        private EffectData _castEffectData;



        [Tooltip("źȯ ��� ����")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedBulletData;

        [Tooltip("źȯ ������ �Դϴ�")]
        [SerializeField]
        private BulletData _bulletData;

        [Tooltip("źȯ ��ǥ ������")]
        [SerializeField]
        private TargetData _bulletTargetData;



        [Tooltip("ü�°��� ��� ����")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedDecreaseHealth;

        [Tooltip("ü�°��� ��ǥ ������")]
        [SerializeField]
        private TargetData _decreaseNowHealthTargetData;

        [Tooltip("ü�°���")]
        [SerializeField]
        private int _decreaseNowHealthValue;

        [Tooltip("ü�°��� ����Ʈ")]
        [SerializeField]
        private EffectData _decreaseNowHealthEffectData;



        [Tooltip("ü������ ��� ����")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedIncreaseHealth;

        [Tooltip("ü������ ��ǥ ������")]
        [SerializeField]
        private TargetData _increaseNowHealthTargetData;

        [Tooltip("ü������")]
        [SerializeField]
        private int _increaseNowHealthValue;

        [Tooltip("ü������ ����Ʈ")]
        [SerializeField]
        private EffectData _increaseNowHealthEffectData;




        [Tooltip("�����̻� ��� ����")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedStatusData;

        [Tooltip("�����̻� ��ǥ������")]
        [SerializeField]
        private TargetData _statusTargetData;

        [Tooltip("�����̻� ������")]
        [SerializeField]
        private StatusData _statusData;



        [Tooltip("���� ������ ��� ����")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedUnitData;

        [Tooltip("���� ��ǥ������")]
        [SerializeField]
        private TargetData _unitTargetData;

        [Tooltip("���� ������")]
        [SerializeField]
        private UnitData _unitData;



        #region ##### Getter Setter #####

        public EffectData CastEffectData => _castEffectData;

        public TYPE_USED_DATA TypeUsedBulletData => _typeUsedBulletData;
        public BulletData BulletData => _bulletData;
        public TargetData BulletTargetData => _bulletTargetData;


        public TYPE_USED_DATA TypeUsedDecreaseHealth => _typeUsedDecreaseHealth;
        public TargetData DecreaseNowHealthTargetData => _decreaseNowHealthTargetData;
        public int DecreaseNowHealthValue => _decreaseNowHealthValue;


        public TYPE_USED_DATA TypeUsedIncreaseHealth => _typeUsedIncreaseHealth;
        public TargetData IncreaseNowHealthTargetData => _increaseNowHealthTargetData;
        public int IncreaseNowHealthValue => _increaseNowHealthValue;


        public TYPE_USED_DATA TypeUsedStatusData => _typeUsedStatusData;
        public TargetData StatusTargetData => _statusTargetData;
        public StatusData StatusData => _statusData;


        public TYPE_USED_DATA TypeUsedUnitData => _typeUsedUnitData;
        public UnitData UnitData => _unitData;
        public TargetData UnitTargetData => _unitTargetData;


#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
        public void SetCastEffectData(EffectData effectData) => _castEffectData = effectData;

        public void SetTypeUsedBulletData(TYPE_USED_DATA typeBullet) => _typeUsedBulletData = typeBullet;
        public void SetBulletData(BulletData bulletData) => _bulletData = bulletData;
        public void SetBulletTargetData(TargetData targetData) => _bulletTargetData = targetData;

        public void SetTypeUsedDecreaseHealth(TYPE_USED_DATA typeHealth) => _typeUsedDecreaseHealth = typeHealth;
        public void SetDecreaseNowHealthTargetData(TargetData targetData) => _decreaseNowHealthTargetData = targetData;
        public void SetDecreaseNowHealthValue(int value) => _decreaseNowHealthValue = value;

        public void SetTypeUsedIncreaseHealth(TYPE_USED_DATA typeHealth) => _typeUsedIncreaseHealth = typeHealth;
        public void SetIncreaseNowHealthTargetData(TargetData targetData) => _increaseNowHealthTargetData = targetData;
        public void SetIncreaseNowHealthValue(int value) => _increaseNowHealthValue = value;

        public void SetTypeUsedStatusData(TYPE_USED_DATA typeStatus) => _typeUsedStatusData = typeStatus;
        public void SetStatusTargetData(TargetData targetData) => _statusTargetData = targetData;
        public void SetStatusData(StatusData statusData) => _statusData = statusData;


        public void SetTypeUsedUnitData(TYPE_USED_DATA typeStatus) => _typeUsedUnitData = typeStatus;
        public void SetUnitTargetData(TargetData targetData) => _unitTargetData = targetData;
        public void SetUnitData(UnitData uData) => _unitData = uData;

#endif

        #endregion


        public void ProcessUnitData(ICaster caster)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _unitTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                //                if (blocks[i].unitActor == null)
                //                {
                var uCard = UnitCard.Create(_unitData);
                var uKey = uCard.UnitKeys[0];
                //������ ����
                UnitManager.Current.CreateUnit(uCard, uKey, blocks[i], caster.typeTeam);
                //                }
            }
        }

        public void ProcessStatusData(ICaster caster)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _statusTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null && blocks[i].unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    //Debug.Log("Receive");
                    blocks[i].unitActor.ReceiveStatusData(caster, StatusData);
                }
            }
        }

        public void ProcessDecreaseNowHealth(ICaster caster)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _decreaseNowHealthTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null)
                {
                    blocks[i].unitActor.DecreaseHealth(_decreaseNowHealthValue);
                    EffectManager.ActivateEffect(_decreaseNowHealthEffectData, blocks[i].unitActor.position);
                }
            }
        }

        public void ProcessIncreaseNowHealth(ICaster caster)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _increaseNowHealthTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null)
                {
                    blocks[i].unitActor.IncreaseHealth(_increaseNowHealthValue);
                    EffectManager.ActivateEffect(_increaseNowHealthEffectData, blocks[i].unitActor.position);
                }
            }
        }

        public void ProcessEffectData(ICaster caster, System.Action callback)
        {
            EffectManager.ActivateEffect(CastEffectData, caster.position, delegate { callback?.Invoke(); });
        }

        public void ProcessBulletData(ICaster caster, System.Action callback)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _bulletTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null)
                {
                    BulletManager.ActivateBullet(BulletData, caster.position, blocks[i].unitActor.position, delegate { callback?.Invoke(); });
                }
            }
        }
    }


    #endregion

    [SerializeField]
    private string _key;

    [SerializeField]
    private Sprite _icon;


    [Tooltip("[DeployCast] - �׻� ��ų�� �ߵ��˴ϴ�\n" +
        "[PreCast] - ������ ���۵ɶ� �ߵ��մϴ�\n" +
        "[AttackCast] - ���� ���� ���� �����Ҷ� �ߵ��˴ϴ�\n" +
        "[AttackedCast] - ���� ���� ���� ������ ������ �ߵ��˴ϴ�")]
    [SerializeField]
    private TYPE_SKILL_CAST _typeSkillCast;



    [SerializeField]
    [Tooltip("��ų �ߵ� �����Դϴ�")]
    private TYPE_SKILL_CAST_CONDITION _typeSkillCastCondition;

    [SerializeField]
    [Tooltip("��ų�ߵ� ���� �� Ÿ�� �Դϴ�")]
    private TYPE_SKILL_CAST_CONDITION_COMPARE _typeSkillCastConditionCompare;

    [SerializeField]
    [Tooltip("��ų�ߵ� ���� �� Ÿ�� �Դϴ�")]
    private TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE _typeSkillCastConditionCompareValue;

    [SerializeField]
    [Tooltip("��ų ���� ���� ��")]
    private float _conditionValue;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("���ݽ� ��ų ���� Ȯ���Դϴ�")]
    private float _skillCastRate = 0.2f;



    [SerializeField]
    [Tooltip("��ų ������ ���μ��� �Դϴ�")]
    private SkillDataProcess _skillDataProcess = new SkillDataProcess();





    #region ##### Getter Setter #####

    public string Key => _key;
    public Sprite Icon => _icon;

    public string SkillName => null;
    public string Description => null;

    public TYPE_SKILL_CAST typeSkillCast => _typeSkillCast;
    public float skillCastRate => _skillCastRate;



    #endregion


    private System.Action[] _processEvents = null;
    private int nowProcessIndex = 0;

    public bool IsTypeSkillCast(TYPE_SKILL_CAST typeSkillCast) => (_typeSkillCast == typeSkillCast);

    public bool IsSkillCondition(IUnitActor uActor)
    {

        switch (_typeSkillCastCondition)
        {
            case TYPE_SKILL_CAST_CONDITION.Health:
                var value = GetValue(uActor.nowHealthValue, uActor.HealthRate());
                return IsConditionCompare(value);
            default:
                break;
        }
        return true;
    }

    private bool IsConditionCompare(float value)
    {
        switch (_typeSkillCastConditionCompare)
        {
            case TYPE_SKILL_CAST_CONDITION_COMPARE.GreaterThan:
                return (value > _conditionValue);
            case TYPE_SKILL_CAST_CONDITION_COMPARE.LessThan:
                return (value < _conditionValue);
            case TYPE_SKILL_CAST_CONDITION_COMPARE.NotEqual:
                return (value != _conditionValue);
            case TYPE_SKILL_CAST_CONDITION_COMPARE.Equal:
                return (value == _conditionValue);
        }
        return false;
    }

    private float GetValue(int value, float rate)
    {
        if (_typeSkillCastConditionCompareValue == TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE.Rate)
            return rate;
        else
            return value;
    }


    public void CastSkillProcess(ICaster caster, TYPE_SKILL_CAST typeSkillActivate)
    {
        if (_typeSkillCast == typeSkillActivate)
        {
            nowProcessIndex = 0;
            AssemblySkillProcess(caster);
            RunSkillProcess();
        }
    }


    private void AssemblySkillProcess(ICaster caster)
    {
        _processEvents = new System.Action[3];

        var assemblyProcessIndex = 0;
        //����Ʈ ���μ��� ����
        if (_skillDataProcess.CastEffectData != null)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessEffectData(caster, RunSkillProcess); };
        }

        //�ݹ�����̸� ���μ��� �ε��� ����
        if (_skillDataProcess.TypeUsedBulletData == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            assemblyProcessIndex++;
        }

        //źȯ�� ����ϸ� ���μ��� ����
        if (_skillDataProcess.TypeUsedBulletData != SkillDataProcess.TYPE_USED_DATA.NotUsed && _skillDataProcess.BulletTargetData != null)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessBulletData(caster, RunSkillProcess); };
        }


        //�ݹ��̸� ���� ���μ����� ����
        if (_skillDataProcess.TypeUsedDecreaseHealth == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessDecreaseNowHealth(caster); };
        }
        //�ƴϸ� ���� ���μ����� ����
        else if (_skillDataProcess.TypeUsedDecreaseHealth == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessDecreaseNowHealth(caster); };
        }


        //�ݹ��̸� ���� ���μ����� ����
        if (_skillDataProcess.TypeUsedIncreaseHealth == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessIncreaseNowHealth(caster); };
        }
        //�ƴϸ� ���� ���μ����� ����
        else if (_skillDataProcess.TypeUsedIncreaseHealth == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessIncreaseNowHealth(caster); };
        }

        //�ݹ��̸� ���� ���μ����� ����
        if (_skillDataProcess.TypeUsedStatusData == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessStatusData(caster); };
        }
        //�ݹ��̸� ���� ���μ����� ����
        else if (_skillDataProcess.TypeUsedStatusData == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            //Debug.Log("Assemble");
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessStatusData(caster); };
        }

        //�ݹ��̸� ���� ���μ����� ����
        if (_skillDataProcess.TypeUsedUnitData == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessUnitData(caster); };
        }
        //�ݹ��̸� ���� ���μ����� ����
        else if (_skillDataProcess.TypeUsedUnitData == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessUnitData(caster); };
        }



    }

    private void RunSkillProcess()
    {
        if (nowProcessIndex < _processEvents.Length)
        {
            _processEvents[nowProcessIndex++]?.Invoke();
        }
    }


    #region ##### UnitTest #####

#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

    public void SetData(TYPE_SKILL_CAST typeSkillActivate, float skillActivateRate = 1f)
    {
        _typeSkillCast = typeSkillActivate;
        _skillCastRate = skillActivateRate;
    }


    public void SetSkillDataProcess(SkillDataProcess skillDataProcess)
    {
        _skillDataProcess = skillDataProcess;
    }


    public void SetSkillDataCondition(TYPE_SKILL_CAST_CONDITION typeSkillCastCondition)
    {
        _typeSkillCastCondition = typeSkillCastCondition;
    }

    public void SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE typeSkillCastConditionCompare)
    {
        _typeSkillCastConditionCompare = typeSkillCastConditionCompare;
    }
    public void SetSkillDataConditionCompareValue(TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE typeSkillCastConditionCompareValue)
    {
        _typeSkillCastConditionCompareValue = typeSkillCastConditionCompareValue;
    }
    public void SetSkillDataConditionValue(float value)
    {
        _conditionValue = value;
    }

    public StatusData GetStatusData()
    {
        return _skillDataProcess.StatusData;
    }


#endif

    #endregion
}
