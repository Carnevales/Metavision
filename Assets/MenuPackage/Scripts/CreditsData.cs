using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreditsData", menuName = "Menu/Credits Data")]
public class CreditsData : ScriptableObject
{
    public List<MemberInfo> members = new List<MemberInfo>();
}

[Serializable]
public class MemberInfo
{
    public string memberName;
    public string role;
}
