using UnityEngine;

/// <summary>Centraliza a navegação entre os painéis do menu.</summary>
public class PanelManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject leavePanel;

    private GameObject[] panels;

    private void Awake()
    {
        panels = new[] { lobbyPanel, creditsPanel, settingsPanel, leavePanel };
        ShowLobby();
    }

    public void ShowLobby() => ShowOnly(lobbyPanel);
    public void ShowCredits() => ShowOnly(creditsPanel);
    public void ShowSettings() => ShowOnly(settingsPanel);
    public void ShowLeaveConfirmation() => ShowOnly(leavePanel);

    public void CloseCurrentPanel() => ShowLobby();

    private void ShowOnly(GameObject panelToShow)
    {
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(panel == panelToShow);
        }
    }
}
