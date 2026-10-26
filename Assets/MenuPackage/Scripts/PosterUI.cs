using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Preenche o Content de um ScrollView com os pôsteres cadastrados.</summary>
public class PosterUI : MonoBehaviour
{
    [SerializeField] private PosterData posterData;
    [SerializeField] private PosterItem posterItemPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private PanelManager panelManager;
    [Header("Scroll navigation")]
    [SerializeField] private ScrollRect scrollRect;
    [Tooltip("Quanto o ScrollView percorre a cada clique, de 0 a 1.")]
    [Range(0.01f, 1f)] [SerializeField] private float navigationStep = 0.25f;

    private readonly List<PosterItem> spawnedItems = new List<PosterItem>();

    private void OnEnable() => Refresh();

    public void Refresh()
    {
        Clear();
        if (posterData == null || posterItemPrefab == null || content == null) return;

        foreach (PosterInfo poster in posterData.posters)
        {
            PosterItem item = Instantiate(posterItemPrefab, content);
            item.Setup(poster);
            spawnedItems.Add(item);
        }
    }

    public void Close()
    {
        if (panelManager != null) panelManager.ShowLobby();
    }

    /// <summary>Ligue este método ao botão de avançar.</summary>
    public void Next()
    {
        Navigate(1f);
    }

    /// <summary>Ligue este método ao botão de retroceder.</summary>
    public void Previous()
    {
        Navigate(-1f);
    }

    private void Navigate(float direction)
    {
        if (scrollRect == null) return;

        if (scrollRect.horizontal)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(
                scrollRect.horizontalNormalizedPosition + (navigationStep * direction));
        }
        else if (scrollRect.vertical)
        {
            // Na vertical, 1 representa o topo; por isso a direção é invertida.
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(
                scrollRect.verticalNormalizedPosition - (navigationStep * direction));
        }
    }

    private void Clear()
    {
        foreach (PosterItem item in spawnedItems)
        {
            if (item != null) Destroy(item.gameObject);
        }
        spawnedItems.Clear();
    }
}
