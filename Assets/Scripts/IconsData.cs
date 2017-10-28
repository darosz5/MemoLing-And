using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IconItem {

    public string Key;
    public Sprite Icon;

    public IconItem (string key, Sprite icon)
    {
        Key = key;
        Icon = icon;
    }
}


public class IconsData  {
   
    public List<IconItem> CreataData(List<string> keys)
    {
        List<IconItem> IconData = new List<IconItem>();
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            Sprite icon = Resources.Load<Sprite>(LocalizationManager.Instance.category + "/" + keys[i]);
            IconData.Add(new IconItem(key, icon));
        }
        return IconData;
    } 

    public Sprite GetIcon(string key, List<IconItem> items)
    {       
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == key)
                return items[i].Icon;
        }
        return null;
    }
}
