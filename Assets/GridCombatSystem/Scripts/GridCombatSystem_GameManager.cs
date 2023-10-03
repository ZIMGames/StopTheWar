using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridCombatSystem_GameManager : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private GridCombatSystem gridCombatSystem;
    private int coins;

    [SerializeField] private GameObject winPanelGameObject, losePanelGameObject, loseShadowPanelGameObject, winShadowPanelGameObject;
    private GameObject shadowPanelGameObject;

    private void Start()
    {
        coins = 0;
        coinsText.text = coins.ToString();

        winPanelGameObject.SetActive(false);
        losePanelGameObject.SetActive(false);
        if (shadowPanelGameObject != null)
            shadowPanelGameObject.SetActive(false);

        GridCombatSystem.onWinCallback = OnWin;
        GridCombatSystem.onLoseCallback = OnLose;
        GridCombatSystem.onEnemyDiedCallback = OnEnemyDied;

        //if (Random.Range(1, 11) <= 4)
        //{
        //    SFXMusic.Instance.PlayGoodAfternoon();
        //}
    }

    private void OnEnemyDied(Hero.HeroType heroType)
    {
        if (heroType == Hero.HeroType.RusHero)
        {
            coins += 7;
            coinsText.text = coins.ToString();
        }
        else if (heroType == Hero.HeroType.RusTank)
        {
            coins += 10;
            coinsText.text = coins.ToString();
        }
    }

    private void OnWin()
    {
        //AdManager.ShowInterstitial(false);
        AdManager.AdAction();

        shadowPanelGameObject = winShadowPanelGameObject;

        coins += 50;
        CountryCache.coins = coins;
        CountryCache.wonTerritory = true;
        SFXMusic.Instance.PlayApplauses();
        ShowAppropriatePanel(winPanelGameObject);
    }

    private void OnLose()
    {
        AdManager.AdAction();

        shadowPanelGameObject = loseShadowPanelGameObject;

        CountryCache.coins = coins;
        CountryCache.wonTerritory = false;
        SFXMusic.Instance.PlayDisappointedCrowd();
        ShowAppropriatePanel(losePanelGameObject);
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

    //public void Restart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    SFXMusic.Instance.StopPlayingAllClips();
    //}
}
