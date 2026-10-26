using System.Collections.Generic;
using UnityEngine;

/// <summary>Preenche o Content do painel de créditos.</summary>
public class CreditsUI : MonoBehaviour
{
    [SerializeField] private CreditsData creditsData;
    [SerializeField] private MemberItem memberItemPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private PanelManager panelManager;

    private readonly List<MemberItem> spawnedItems = new List<MemberItem>();

    private void OnEnable() => Refresh();

    public void Refresh()
    {
        Clear();
        if (creditsData == null || memberItemPrefab == null || content == null) return;

        foreach (MemberInfo member in creditsData.members)
        {
            MemberItem item = Instantiate(memberItemPrefab, content);
            item.Setup(member);
            spawnedItems.Add(item);
        }
    }

    public void Close()
    {
        if (panelManager != null) panelManager.ShowLobby();
    }

    private void Clear()
    {
        foreach (MemberItem item in spawnedItems)
        {
            if (item != null) Destroy(item.gameObject);
        }
        spawnedItems.Clear();
    }
}
