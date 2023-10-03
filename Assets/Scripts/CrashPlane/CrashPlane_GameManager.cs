using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashPlane_GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanelGameObject, losePanelGameObject, shadowPanelGameObject;

    private void Start()
    {
        winPanelGameObject.SetActive(false);
        losePanelGameObject.SetActive(false);
        shadowPanelGameObject.SetActive(false);

        CrashPlane.onWinCallback = OnWin;
        CrashPlane.onLoseCallback = OnLose;
    }

    private void OnWin()
    {
        AdManager.AdAction();

        QuestCache.questCompleted = true;
        ShowAppropriatePanel(winPanelGameObject);
        SFXMusic.Instance.PlayApplauses();
    }

    private void OnLose()
    {
        AdManager.AdAction();

        ShowAppropriatePanel(losePanelGameObject);
        SFXMusic.Instance.PlayDisappointedCrowd();
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

    public void GetEzMode()
    {
        AdManager.ShowRewardedVideo(() =>
        {
            CrashPlane.planeSpeed = Mathf.Max(0.3f, CrashPlane.planeSpeed - 0.5f);
            SceneManager.LoadScene("CrashPlane");
        }, true);
    }

    public void Close()
    {
        SceneManager.LoadScene("MainMenu");
        SFXMusic.Instance.StopPlayingAllClips();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SFXMusic.Instance.StopPlayingAllClips();
    }
}
