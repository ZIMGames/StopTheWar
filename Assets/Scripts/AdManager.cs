using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager
{
    public static void ShowInterstitial(bool showLoadingPanel)
    {
        ConsentManager.ConsentManagerDemo.Scripts.AppodealConsent.instance.ShowInterstitial(showLoadingPanel);
    }

    public static void ShowRewardedVideo(System.Action rewardAction, bool showLoadingPanel)
    {
        ConsentManager.ConsentManagerDemo.Scripts.AppodealConsent.instance.ShowRewardedVideo(rewardAction, showLoadingPanel);
    }

    public static void AdAction()
    {
        ConsentManager.ConsentManagerDemo.Scripts.AppodealConsent.instance.AdAction();
    }
}
