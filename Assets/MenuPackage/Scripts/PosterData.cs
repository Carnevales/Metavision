using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PosterData", menuName = "Menu/Poster Data")]
public class PosterData : ScriptableObject
{
    public List<PosterInfo> posters = new List<PosterInfo>();
}

[Serializable]
public class PosterInfo
{
    public Sprite image;
    public string posterName;
    public string className;
    public string url;
}
