using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Referências visuais de um item de pôster.</summary>
public class PosterItem : MonoBehaviour
{
    [SerializeField] private Image posterImage;
    [SerializeField] private TMP_Text posterNameText;
    [SerializeField] private TMP_Text classNameText;
    [SerializeField] private string url;

    public void Setup(PosterInfo info)
    {
        if (info == null) return;

        if (posterImage != null) posterImage.sprite = info.image;
        if (posterNameText != null) posterNameText.text = info.posterName;
        if (classNameText != null) classNameText.text = info.className;
        url = info.url;
    }

    public void OpenUrl()
    {
        Application.OpenURL(url);
    }
}
