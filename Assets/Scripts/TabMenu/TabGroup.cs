using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    private List<TabButton> tabButtonList;
    //public Sprite tabIdle;
    //public Sprite tabHover;
    //public Sprite tabActive;

    [SerializeField] private List<GameObject> objectsToSwap;


    [SerializeField] private TabButton selectedTabButton;

    public void Subscribe(TabButton tabButton)
    {
        if (tabButtonList == null)
        {
            tabButtonList = new List<TabButton>();
        }

        tabButtonList.Add(tabButton);
    }

    public void OnTabEnter(TabButton tabButton)
    {
        ResetTabs();
        //if (selectedTabButton == null || tabButton != selectedTabButton)
            //tabButton.backgroundImage.sprite = tabHover;
    }

    public void OnTabExit(TabButton tabButton)
    {
        ResetTabs();    
    }

    public void OnTabSelected(TabButton tabButton)
    {

        if (selectedTabButton != null)
        {
            if (selectedTabButton == tabButton)
                return;
            selectedTabButton.Deselect();
        }

        selectedTabButton = tabButton;

        selectedTabButton.Select();

        ResetTabs();        
        //tabButton.backgroundImage.sprite = tabActive;
        int index = tabButton.transform.GetSiblingIndex();
        //Debug.Log(index);
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
                objectsToSwap[i].SetActive(true);
            else
                objectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtonList)
        {
            if (selectedTabButton != null && button == selectedTabButton)
                continue;

            //button.backgroundImage.sprite = tabIdle;
        }
    }
}
