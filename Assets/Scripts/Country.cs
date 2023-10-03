using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country
{
    public event System.Action onFullTerritoryOccupied;
    public event System.Action onTerritoryUnlocked;

    private int partsUnlocked;
    private int partUnlockedMax;

    public Country(int partsUnlocked)
    {
        this.partsUnlocked = partsUnlocked;
    }

    public void UnlockTerritory()
    {
        partsUnlocked++;
    }
}
