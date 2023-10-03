using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPetiton_GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanelGameObject, shadowPanelGameObject;

    private void Start()
    {
        winPanelGameObject.SetActive(false);
        shadowPanelGameObject.SetActive(false);

        OpenPetition.onPetitionSignedCompletelyCallback = OnWin;
    }

    private void OnWin()
    {
        AdManager.AdAction();

        QuestCache.questCompleted = true;
        ShowAppropriatePanel(winPanelGameObject);
        SFXMusic.Instance.PlayApplauses();
    }

    private void ShowAppropriatePanel(GameObject panel)
    {
        panel.SetActive(true);
        RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
        panelRectTransform.anchoredPosition = new Vector2(1000, 109);
        panelRectTransform.LeanMoveLocal(new Vector3(-10, 109), 0.5f).setOnComplete(() =>
        {
            panelRectTransform.LeanMoveLocal(new Vector3(0, 109), 0.1f);
        });

        shadowPanelGameObject.SetActive(true);
        CanvasGroup shadowPanelCanvasGroup = shadowPanelGameObject.GetComponent<CanvasGroup>();
        shadowPanelCanvasGroup.alpha = 0;
        LeanTween.alphaCanvas(shadowPanelCanvasGroup, 1f, 0.6f);
    }

    public void Close()
    {
        SceneManager.LoadScene("MainMenu");
        SFXMusic.Instance.StopPlayingAllClips();
    }
}
