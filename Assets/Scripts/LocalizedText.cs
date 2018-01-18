using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour {

    [SerializeField]
    string key;

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {             
         text.text = LocalizationManager.Instance.GetUIValue(key);             
    }
}
