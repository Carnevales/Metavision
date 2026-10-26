using UnityEngine;

/// <summary>Controla os botões da confirmação de saída.</summary>
public class LeaveUI : MonoBehaviour
{
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private LobbyManager lobbyManager;

    public void ConfirmLeave()
    {
        if (lobbyManager != null) lobbyManager.QuitGame();
        else Application.Quit();
    }

    public void Cancel()
    {
        if (panelManager != null) panelManager.ShowLobby();
    }

    public void Close() => Cancel();
}
