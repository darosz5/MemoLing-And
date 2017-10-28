using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LocalizationData
{
    public LoalizationItem[] items;
}

[System.Serializable]
public class LoalizationItem
{
    public string key;
    public string value;
}



