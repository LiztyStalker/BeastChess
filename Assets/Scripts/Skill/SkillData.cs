using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

public enum TYPE_SKILL_CAST { DeployCast, PreCast, AttackCast}


[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{


    #region ##### SkillDataProcess #####

    [System.Serializable]
    public class SkillDataProcess
    {
        public enum TYPE_USED_DATA { NotUsed, Used, Used_Callback }

        [Tooltip("시전이펙트 입니다")]
        [SerializeField]
        private EffectData _castEffectData;



        [Tooltip("탄환 사용 여부")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedBulletData;

        [Tooltip("탄환 데이터 입니다")]
        [SerializeField]
        private BulletData _bulletData;

        [Tooltip("탄환 목표 데이터")]
        [SerializeField]
        private TargetData _bulletTargetData;



        [Tooltip("체력증감 사용 여부")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedHealth;

        [Tooltip("체력증감 목표 데이터")]
        [SerializeField]
        private TargetData _increaseNowHealthTargetData;

        [Tooltip("체력증감")]
        [SerializeField]
        private int _increaseNowHealthValue;

        [Tooltip("체력증감 이펙트")]
        [SerializeField]
        private EffectData _increaseNowHealthEffectData;



        [Tooltip("상태이상 사용 여부")]
        [SerializeField]
        private TYPE_USED_DATA _typeUsedStatusData;

        [Tooltip("상태이상 목표데이터")]
        [SerializeField]
        private TargetData _statusTargetData;

        [Tooltip("상태이상 데이터")]
        [SerializeField]
        private StatusData _statusData;



        #region ##### Getter Setter #####

        public EffectData CastEffectData => _castEffectData;
        public TYPE_USED_DATA TypeUsedBulletData => _typeUsedBulletData; 
        public BulletData BulletData => _bulletData; 
        public TargetData BulletTargetData => _bulletTargetData;
        public TYPE_USED_DATA TypeUsedHealth => _typeUsedHealth; 
        public TargetData IncreaseNowHealthTargetData => _increaseNowHealthTargetData;
        public int IncreaseNowHealthValue => _increaseNowHealthValue;
        public TYPE_USED_DATA TypeUsedStatusData => _typeUsedStatusData;
        public TargetData StatusTargetData => _statusTargetData;
        public StatusData StatusData => _statusData;
        public EffectData IncreaseNowHealthEffectData => _increaseNowHealthEffectData;


#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
        public void SetCastEffectData(EffectData effectData) => _castEffectData = effectData;
        public void SetTypeUsedBulletData(TYPE_USED_DATA typeBullet) => _typeUsedBulletData = typeBullet;
        public void SetBulletData(BulletData bulletData) => _bulletData = bulletData;
        public void SetBulletTargetData(TargetData targetData) => _bulletTargetData = targetData;
        public void SetTypeUsedHealth(TYPE_USED_DATA typeHealth) => _typeUsedHealth = typeHealth;
        public void SetIncreaseNowHealthTargetData(TargetData targetData) => _increaseNowHealthTargetData = targetData;
        public void SetIncreaseNowHealthValue(int value) => _increaseNowHealthValue = value;
        public void SetTypeUsedStatusData(TYPE_USED_DATA typeStatus) => _typeUsedStatusData = typeStatus;
        public void SetStatusTargetData(TargetData targetData) => _statusTargetData = targetData;
        public void SetStatusData(StatusData statusData) => _statusData = statusData;
#endif

#endregion


        public void ProcessStatusData(ICaster caster)
        {
            var blocks = FieldManager.GetTargetBlocks(caster, _statusTargetData, caster.typeTeam);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null && blocks[i].unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    blocks[i].unitActor.ReceiveStatusData(caster, StatusData);
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
                    blocks[i].unitActor.IncreaseHealth(-_increaseNowHealthValue);
                    EffectManager.ActivateEffect(IncreaseNowHealthEffectData, blocks[i].unitActor.position);
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
                    BulletManager.Current.ActivateBullet(BulletData, caster.position, blocks[i].unitActor.position, delegate { callback?.Invoke(); });
                }
            }
        }

    }


    #endregion



    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

    [Tooltip("[DeployCast] - 항상 스킬이 발동됩니다\n[PreCast] - 전투가 시작될때 발동합니다\n[AttackCast] - 전투 진행 도중에 공격에 의해서 발동됩니다")]
    [SerializeField]
    private TYPE_SKILL_CAST _typeSkillCast;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("공격시 스킬 시전 확률입니다")]
    private float _skillCastRate = 0.2f;


    [SerializeField]
    [Tooltip("스킬 데이터 프로세스 입니다")]
    private SkillDataProcess _skillDataProcess = new SkillDataProcess();



    #region ##### Getter Setter #####

    public string key => name;        
    public Sprite icon => _icon;

    public TYPE_SKILL_CAST typeSkillCast => _typeSkillCast;
    public float skillCastRate => _skillCastRate;

    #endregion


    private System.Action[] _processEvents = null;
    private int nowProcessIndex = 0;


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
        //이펙트 프로세스 적용
        if(_skillDataProcess.CastEffectData != null)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessEffectData(caster, RunSkillProcess); };
        }

        //콜백실행이면 프로세스 인덱스 증가
        if (_skillDataProcess.TypeUsedBulletData == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            assemblyProcessIndex++;
        }

        //탄환을 사용하면 프로세스 적용
        if (_skillDataProcess.TypeUsedBulletData != SkillDataProcess.TYPE_USED_DATA.NotUsed && _skillDataProcess.BulletTargetData != null)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessBulletData(caster, RunSkillProcess); };
        }


        //콜백이면 다음 프로세스에 적용
        if (_skillDataProcess.TypeUsedHealth == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessIncreaseNowHealth(caster); };
        }
        //아니면 현재 프로세스에 적용
        else if(_skillDataProcess.TypeUsedHealth == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessIncreaseNowHealth(caster); };
        }

        //콜백이면 다음 프로세스에 적용
        if (_skillDataProcess.TypeUsedStatusData == SkillDataProcess.TYPE_USED_DATA.Used_Callback)
        {
            _processEvents[assemblyProcessIndex + 1] += delegate { _skillDataProcess.ProcessStatusData(caster);};
        }
        //콜백이면 다음 프로세스에 적용
        else if (_skillDataProcess.TypeUsedStatusData == SkillDataProcess.TYPE_USED_DATA.Used)
        {
            _processEvents[assemblyProcessIndex] += delegate { _skillDataProcess.ProcessStatusData(caster); };
        }



    }

    private void RunSkillProcess()
    {
        if (nowProcessIndex < _processEvents.Length)
        {
//            Debug.Log(_processEvents[nowProcessIndex].Method + " " + nowProcessIndex);
//#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
//            var thread = new Thread(TestRun);
//            thread.Start();
//#else
            _processEvents[nowProcessIndex++]?.Invoke();
//#endif
        }
    }

#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
    //private void TestRun()
    //{
    //    Debug.Log("Thread Start");
    //    Thread.Sleep(1000);
    //    Debug.Log("Thread End");
    //    _processEvents[nowProcessIndex++]?.Invoke();
    //}

    public void SetData(TYPE_SKILL_CAST typeSkillActivate, float skillActivateRate = 0.5f)
    {
        _typeSkillCast = typeSkillActivate;
        _skillCastRate = skillActivateRate;
    }

    public void SetData(TargetData targetData)
    {
        _skillDataProcess.SetStatusTargetData(targetData);
    }

    public void SetSkillDataProcess(SkillDataProcess skillDataProcess)
    {
        _skillDataProcess = skillDataProcess;
    }


#endif

}
