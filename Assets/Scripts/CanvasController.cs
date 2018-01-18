using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    [Header("References")]

    public GameObject TextObj;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SHUFFLING, this.OnEvent);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SHUFFLING:
                StartCoroutine(SwitchCO(TextObj, 1.2f));
                break;
        }
    }

    IEnumerator SwitchCO(GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
