using System.Collections;
using System.Collections.Generic;
using CodeMonkey.MonoBehaviours;
using UnityEngine;
using CodeMonkey.Utils;

public class Country_Manager : MonoBehaviour
{
    public static System.Action onWholeCountriesOccupied;
    public static System.Action<int> onFullTerritoryOccupied;

    [SerializeField] private GameObject arenaPanelGameObject, battleBtnGameObject, shadowPanelGameObject;
    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] private List<HeroUnitGridInfoArmyListSO> enemyHeroUnitGridCombatArmyListSOList;
    [SerializeField] private List<CountryVisual> countryVisualList;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private ResourceTypeSO coinResourceType;

    [SerializeField] private TabState tabState;

    private int countryIndex;
    private int countryPartIndex;
    private int countriesUnlocked;

    private float zoom = 70f;

    private void Awake()
    {

        countriesUnlocked = PlayerPrefs.GetInt("countriesUnlocked");
    }

    private void Start()
    {
        cameraFollow.Setup(() => followPos, () => zoom, false, false);

        Hide();
        for (int i = 0; i < countryVisualList.Count; i++)
        {
            countryVisualList[i].Setup(PlayerPrefs.GetInt("countryPartsUnlocked" + i, 0), this);
            int k = i;
            countryVisualList[i].onFullTerritoryOccupied = () =>
            {
                OnFullTerritoryOccupied(k);
            };
        }
        ShowCorrectCountry();
        tabState.onEnableCallback = () => {
            if (countriesUnlocked < countryVisualList.Count)
                countryVisualList[countriesUnlocked].Show();
        };
        tabState.onDisableCallback = () => {
            if (countriesUnlocked < countryVisualList.Count && countryVisualList[countriesUnlocked] != null)
                countryVisualList[countriesUnlocked].Hide();
        };


        CheckCache();
    }

    private void ShowCorrectCountry()
    {
        for (int i = 0; i < countryVisualList.Count; i++)
        {
            if (i == countriesUnlocked)
            {
                countryVisualList[i].Show();
            }
            else
            {
                countryVisualList[i].Hide();
            }
        }
    }

    private void OnFullTerritoryOccupied(int countryIndex)
    {
        countriesUnlocked = countryIndex + 1;
        PlayerPrefs.SetInt("countriesUnlocked", countriesUnlocked);
        if (countriesUnlocked >= countryVisualList.Count)
        {
            onWholeCountriesOccupied?.Invoke();
        }
        else
        {
            onFullTerritoryOccupied?.Invoke(countryIndex);
            ShowCorrectCountry();
        }
    }

    private void CheckCache()
    {
        if (CountryCache.wonTerritory)
        {
            CountryCache.wonTerritory = false;
            int countriesUnlockedCopy = countriesUnlocked;
            countryVisualList[countriesUnlockedCopy].UnlockTerritory();
            PlayerPrefs.SetInt("countryPartsUnlocked" + countriesUnlockedCopy, countryVisualList[countriesUnlockedCopy].GetCountryPartsUnlocked());
        }
        if (CountryCache.coins > 0)
        {
            int _coins = CountryCache.coins;
            CountryCache.coins = 0;
            StartCoroutine(coinSpawner.Spawn(_coins, new Vector2(0, 0), 10f, new Vector3(18, 69), (int coins) =>
            {
                ResourceManager.Instance.AddResources(coinResourceType, coins);
            }));
        }

    }

    public void PrepareForBattle(CountryVisual countryVisual, int countryPartIndex)
    {
        this.countryIndex = countryVisualList.IndexOf(countryVisual);
        this.countryPartIndex = countryPartIndex;
        Show();
    }

    private Vector3 followPos = new Vector3(0, 2.74f);
    private Vector3 prevPos;
    private float scaleAmont = 0.2f;

    public void Reset()
    {
        followPos = new Vector3(0, 2.74f);
        zoom = 70f;
}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prevPos = UtilsClass.GetMouseWorldPosition();
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = UtilsClass.GetMouseWorldPosition();
            Vector3 dir = (prevPos - pos);
            if (dir.magnitude >= 1f)
            {
                followPos = followPos + dir * scaleAmont;
                followPos = new Vector2(Mathf.Clamp(followPos.x, -50, 50), Mathf.Clamp(followPos.y, -35, 60));
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            int countriesUnlockedCopy = countriesUnlocked;
            countryVisualList[countriesUnlockedCopy].UnlockTerritory();
            PlayerPrefs.SetInt("countryPartsUnlocked" + countriesUnlockedCopy, countryVisualList[countriesUnlockedCopy].GetCountryPartsUnlocked());
        }
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

    public void ZoomIn()
    {
        if (zoom <= 30f)
            return;

        zoom -= 15f;
    }

    public void ZoomOut()
    {
        if (zoom >= 80f)
            return;

        zoom += 15f;
    }


    private void Show()
    {
        arenaPanelGameObject.SetActive(true);
        battleBtnGameObject.SetActive(true);
        shadowPanelGameObject.SetActive(true);
    }

    public void Hide()
    {
        arenaPanelGameObject.SetActive(false);
        battleBtnGameObject.SetActive(false);
        shadowPanelGameObject.SetActive(false);
    }

    public void Battle()
    {
        List<HeroUnitGridInfo> convertedAlliedHeroList = HeroUnitGridInfo.GetHeroListConverted(ArenaManager.GetSelectedHeroList());
        List<HeroUnitGridInfo> convertedEnemyHeroList = HeroUnitGridInfo.GetHeroListConverted(enemyHeroUnitGridCombatArmyListSOList[countryIndex].list[countryPartIndex].list);
        GameManager.PlayBattle(convertedAlliedHeroList, convertedEnemyHeroList);
    }

    private void OnDestroy()
    {
        if (tabState != null)
        {
            tabState.onDisableCallback = null;
        }
    }
}
