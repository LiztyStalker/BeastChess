using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitSelector : MonoBehaviour
{
    [SerializeField]
    UIUnitSelectorMenu uiSelectorMenu;

    [SerializeField]
    UIUnitInformation uiUnitInformation;

    private void Start()
    {
        uiSelectorMenu.Hide();
        uiSelectorMenu.showInformationEvent += uiUnitInformation.ShowActor;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !uiSelectorMenu.isActiveAndEnabled)
        {
            var wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.RaycastAll(wp, Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.tag == "Unit")
                {
                    uiSelectorMenu.Show(hits[i].collider.GetComponent<UnitActor>(), Input.mousePosition);
                    break;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            uiSelectorMenu.Cancel();
        }
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void AddReturnUnitListener(System.Action<UnitActor> act) => uiSelectorMenu.onReturnUnitEvent += act;
    public void RemoveReturnUnitListener(System.Action<UnitActor> act) => uiSelectorMenu.onReturnUnitEvent -= act;

}
