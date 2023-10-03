using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryVisual : MonoBehaviour
{
    [SerializeField] private List<CountryPart> countryPartList;

    public System.Action onFullTerritoryOccupied;

    private int countryPartsUnlocked;
    private Country_Manager countryManager;

    public void Hide()
    {
        countryManager.Reset();
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Debug.Log(countryPartsUnlocked);
        if (countryPartsUnlocked < countryPartList.Count)
            countryPartList[countryPartsUnlocked].Activate();
    }

    private void Awake()
    {
        //countryPartsUnlocked = PlayerPrefs.GetInt("countryPartsUnlocked", 0);

        //tabState.onEnableCallback = () => {
        //    Show();
        //};
        //tabState.onDisableCallback = () => { Hide(); };
    }

    public void Setup(int countryPartsUnlocked, Country_Manager countryManager)
    {
        this.countryPartsUnlocked = countryPartsUnlocked;
        this.countryManager = countryManager;

        for (int i = 0; i < countryPartList.Count; i++)
        {
            CountryPart countryPart = countryPartList[i];
            int countryPartIndex = i;
            countryPart.SetCountry(this, () =>
            {
                OccupyNextTerritory(countryPartIndex);
            });

            if (i < countryPartsUnlocked)
            {
                countryPart.WonTerritory();
            }
            else if (i == countryPartsUnlocked)
            {
                countryPart.Activate();
            }
            else
            {
                countryPart.Inactivate();
            }
        }
    }


    private void OccupyNextTerritory(int countryPartIndex)
    {
        countryManager.PrepareForBattle(this, countryPartIndex);
    }

    public void UnlockTerritory()
    {
        countryPartList[countryPartsUnlocked].WonTerritory();
        countryPartsUnlocked++;
        if (countryPartsUnlocked < countryPartList.Count)
        {
            countryPartList[countryPartsUnlocked].Activate();
        }
        else
        {
            onFullTerritoryOccupied?.Invoke();
        }
    }

    public int GetCountryPartsUnlocked() => countryPartsUnlocked;
}
