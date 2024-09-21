using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class SOData : ScriptableObject
{
    public List<LevelData> levels;
    public LevelData GetLevelData(int index){
        return levels[index];
    }
}
[System.Serializable]
public class LevelData
{
    public List<ItemCard> cards;
    
}
[System.Serializable]
public class ItemCard
{
    public int id;
    public Type type;
    public AudioClip audioClip;
    public string name;
    public Sprite sprite;
}
