using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using ConsentManager.Common;
using UnityEngine;

namespace ConsentManager.ConsentManagerDemo.Scripts
{
    public class AppodealConsent : MonoBehaviour, IConsentFormListener, IConsentInfoUpdateListener, IRewardedVideoAdListener, IInterstitialAdListener
    {
        public enum AdState
        {
            NONE,
            WAITING_FOR_REWARDED,
            WAITING_FOR_INTERSTITIAL
        }

        private AdState adState = AdState.NONE;
        private int adActions = 0;
        private const int adActionsInterstitialLimit = 3;


        private System.Action rewardAction;

        //public UnityEngine.UI.Text debugText;



        public static AppodealConsent instance;
        private bool isAdsEnabled = false;
        private ConsentForm consentForm;
        private ConsentManager consentManager;
        private bool isShouldSaveConsentForm;
        public Consent currentConsent;
#if UNITY_ANDROID
        private string appkey = "637318647e0c93a1ef4e0d480325f2b734df1d8469b4d7a8"; //Android
#endif
#if UNITY_IOS
        private string appkey = "2b67e21d5addc3eb9ef4a29c0e796f82ca88a1a701f0907a"; //iOS
#endif
        private bool isRVAwaited = false;
        private bool isRVLoaded = false;
        private bool isRVFinished = false;
        private int boosterId = -1;

        [SerializeField] private GameObject loadingGameObject;

        private float timer;
        private float timerMax = 5;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            HideLoadingAD();

#if UNITY_IOS
            InitConsentManager();
#endif
#if UNITY_ANDROID
            InitializeSDK();
#endif
        }

        private void ShowLoadingAD()
        {
            loadingGameObject.SetActive(true);
        }

        private void HideLoadingAD()
        {
            loadingGameObject.SetActive(false);
        }

        public void AdAction()
        {
            adActions++;

            if (adActions >= adActionsInterstitialLimit)
            {
                adActions -= adActionsInterstitialLimit;
                ShowInterstitial(false);
            }
        }

        private void InitializeSDK()
        {
            Appodeal.disableLocationPermissionCheck();
            Appodeal.muteVideosIfCallsMuted(true);
            Appodeal.setAutoCache(Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, true);
#if UNITY_IOS
            Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, currentConsent);
#endif
#if UNITY_ANDROID
            Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, true);
#endif
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);

            //debugText.text += " initialized sdk /";
        }

        private void Update()
        {
            switch (adState)
            {
                case AdState.WAITING_FOR_INTERSTITIAL:
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        adState = AdState.NONE;
                        HideLoadingAD();
                        return;
                    }
                    if (Appodeal.canShow(Appodeal.INTERSTITIAL))
                    {
                        Appodeal.show(Appodeal.INTERSTITIAL);
                        HideLoadingAD();
                        adState = AdState.NONE;
                    }
                    break;
                case AdState.WAITING_FOR_REWARDED:
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        adState = AdState.NONE;
                        HideLoadingAD();
                        return;
                    }
                    if (Appodeal.canShow(Appodeal.REWARDED_VIDEO))
                    {
                        Appodeal.show(Appodeal.REWARDED_VIDEO);
                        HideLoadingAD();
                        adState = AdState.NONE;
                    }
                    break;
            }
        }

        public void ShowRewardedVideo(System.Action rewardAction, bool showLoadingPanel)
        {
            this.rewardAction = rewardAction;
            timer = timerMax;
            if (showLoadingPanel)
                ShowLoadingAD();
            adState = AdState.WAITING_FOR_REWARDED;
        }

        public void ShowInterstitial(bool showLoadingPanel)
        {
            timer = timerMax;
            if (showLoadingPanel)
                ShowLoadingAD();
            adState = AdState.WAITING_FOR_INTERSTITIAL;
        }


#region ConsentManager
        private void ShowConsentDialog()
        {
            if (consentForm != null)
            {
                consentForm.showAsDialog();
            }
        }

        public static void ShowConsentDialogGlobal()
        {
            instance.ShowConsentDialog();
        }
        private void InitConsentManager()
        {
            consentManager = ConsentManager.getInstance();
            //AddCustomVendor();
            consentManager.requestConsentInfoUpdate(appkey, this);

            //debugText.text += "requested info update /";
        }

        private void LoadConsentForm()
        {
            //debugText.text += " consent form is loading /";
            consentForm = new ConsentForm.Builder().withListener(this).build();
            consentForm?.load();
        }

        private void IsConsentFormNeeded()
        {
            Consent.ShouldShow consentShouldShow = consentManager.shouldShowConsentDialog();
            if (consentShouldShow == Consent.ShouldShow.TRUE)
            {
                //debugText.text += " needed /";
                LoadConsentForm();
            }
            else
            {
                //debugText.text += " not needed /";
                InitializeSDK();
                //Appodeal.updateConsent(currentConsent);
                //AnimationManager.ShowMainMenuAfterConsentShownGlobal();
            }
        }

        private void AddCustomVendor()
        {
            Vendor customVendor = new Vendor.Builder(
                                        "DNHND",
                                        "dnhnd",
                                        "https://dnhnd.github.io/privacy.html")
                                    .setPurposeIds(new List<int> { 1, 2, 3 })
                                    .setFeatureId(new List<int> { 1, 2, 3 })
                                    .setLegitimateInterestPurposeIds(new List<int> { 1, 2, 3 })
                                    .build();
            consentManager.setCustomVendor(customVendor);
        }
#endregion

#region ConsentFormListener
        public void onConsentFormLoaded()
        {
            //debugText.text += " consent form loaded /";
            ShowConsentDialog();
        }
        public void onConsentFormError(ConsentManagerException exception) { }
        public void onConsentFormOpened() { }
        public void onConsentFormClosed(Consent consent)
        {
            currentConsent = consent;
            //debugText.text += " consent form closed /";
            InitializeSDK();
            //Appodeal.updateConsent(currentConsent);

            //AnimationManager.ShowMainMenuAfterConsentShownGlobal();
        }
#endregion

#region ConsentInfoUpdateListener
        public void onConsentInfoUpdated(Consent consent)
        {
            currentConsent = consent;
            //debugText.text += " consent info updated /";
            IsConsentFormNeeded();
        }
        public void onFailedToUpdateConsentInfo(ConsentManagerException error)
        {
            //AnimationManager.ShowMainMenuAfterConsentShownGlobal();
        }

        public void onRewardedVideoLoaded(bool precache)
        {

        }

        public void onRewardedVideoFailedToLoad()
        {

        }

        public void onRewardedVideoShowFailed()
        {

        }

        public void onRewardedVideoShown()
        {

        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            rewardAction();
        }

        public void onRewardedVideoClosed(bool finished)
        {

        }

        public void onRewardedVideoExpired()
        {

        }

        public void onRewardedVideoClicked()
        {

        }

        public void onInterstitialLoaded(bool isPrecache)
        {

        }

        public void onInterstitialFailedToLoad()
        {

        }

        public void onInterstitialShowFailed()
        {

        }

        public void onInterstitialShown()
        {

        }

        public void onInterstitialClosed()
        {

        }

        public void onInterstitialClicked()
        {

        }

        public void onInterstitialExpired()
        {

        }
#endregion

    }

}