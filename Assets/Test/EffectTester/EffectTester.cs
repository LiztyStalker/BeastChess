#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTester : MonoBehaviour
{

    [SerializeField]
    private EffectData _effectData;

    private void OnGUI()
    {
        if(GUILayout.Button("Test EffectData Activate"))
        {
            var actor = EffectManager.Current.ActivateEffect(_effectData);
            actor.transform.position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }

        if (GUILayout.Button("Test EffectData Inactivate"))
        {
            EffectManager.Current.InactiveEffect(_effectData);
        }
    }
}

#endif