using TMPro;
using UnityEngine;

/// <summary>Referências visuais de um integrante nos créditos.</summary>
public class MemberItem : MonoBehaviour
{
    [SerializeField] private TMP_Text memberNameText;
    [SerializeField] private TMP_Text roleText;

    public void Setup(MemberInfo info)
    {
        if (info == null) return;
        if (memberNameText != null) memberNameText.text = info.memberName;
        if (roleText != null) roleText.text = info.role;
    }
}
