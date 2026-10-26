using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Comandos disponíveis no lobby principal.</summary>
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private PanelManager panelManager;
    [Tooltip("Nome exato da cena de jogo adicionada em Build Settings.")]
    [SerializeField] private string gameSceneName;

    public void GoToGame()
    {
        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogWarning("Defina o nome da cena de jogo no LobbyManager.", this);
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenLeaveConfirmation()
    {
        if (panelManager != null)
            panelManager.ShowLeaveConfirmation();
    }

    public void OpenCredits()
    {
        if (panelManager != null)
            panelManager.ShowCredits();
    }

    public void OpenSettings()
    {
        if (panelManager != null)
            panelManager.ShowSettings();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
