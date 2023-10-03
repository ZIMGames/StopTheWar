using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;
    }

    private int iState = 0;
    private bool bLoadDone = true;
    private bool bBeginMapGeneration = false;
    private string szToLoad = "";
    private AsyncOperation asyncLoad;

    //private void Update()
    //{
    //    //the main state machine
    //    switch (iState)
    //    {
    //        case 0:


    //            bLoadDone = true;
    //            StartCoroutine(LoadAsyncScene());
    //            iState++;
    //            break;
    //        //case 1:
    //        //    //while loading level
    //        //    if (bBeginMapGeneration)
    //        //    {
    //        //        Debug.Log("Load scene, level 90%");
    //        //        if (!bLoadDone) bLoadDone = GameLevel.Load();
    //        //        if (bLoadDone)
    //        //        {
    //        //            Debug.Log("Generate map done");
    //        //            iState++;
    //        //        }
    //        //    }
    //        //    break;
    //        //case 2:
    //        //    //running game
    //        //    //...
    //        //    break;
    //    }
    //}

    private IEnumerator LoadAsyncScene()
    {
        //the Application loads the scene in the background as the current scene runs
        // this is good for not freezing the view... done by separating some work to a thread
        // and having the rest split in ~7ms jobs

        asyncLoad = SceneManager.LoadSceneAsync(szToLoad, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        LoadingScreen.Instance.SetBarSize(asyncLoad.progress);
        Debug.Log(asyncLoad.progress);

        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //scene has loaded as much as possible, the last 10% can't be multi-threaded
            LoadingScreen.Instance.SetBarSize(asyncLoad.progress);
            Debug.Log(asyncLoad.progress);
            if (asyncLoad.progress >= 0.9f)
            {
                bBeginMapGeneration = true;
                if (bLoadDone)
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }


    public static void PlayWolfAndEggs(int waves, float timeBetweenWaves, float moveSpeed)
    {
        WolfAndEggs.SetStaticParams(waves, timeBetweenWaves, moveSpeed);
        LoadingScreen.Instance.Show();
        Instance.szToLoad = "WolfAndEggs";
        Instance.StartCoroutine(Instance.LoadAsyncScene());
    }

    public static void PlayFindTraitor(int ukrainianSize, int russianSize)
    {
        FindTraitor.SetStaticParams(ukrainianSize, russianSize);
        SceneManager.LoadScene("FindTraitor");
    }

    public static void PlayMinesweeper(int width, int height, int minesInLine)
    {
        Minesweeper.SetStaticParams(width, height, minesInLine);
        SceneManager.LoadScene("Minesweeper");
    }

    public static void PlayCrashPlane(int totalPlanes, int enemyPlanes, float timeSpawnRate, float planeSpeed, float bulletSpeed)
    {
        CrashPlane.SetStaticParams(totalPlanes, enemyPlanes, timeSpawnRate, planeSpeed, bulletSpeed);
        SceneManager.LoadScene("CrashPlane");
    }

    public static void PlayDecoderGame(int width, int height, List<int> activatedIndexList, bool crossActivation)
    {
        DecoderGame.SetStaticParams(width, height, activatedIndexList, crossActivation);
        SceneManager.LoadScene("DecoderGame");
    }

    public static void PlayOpenPetition(int value, int maxValue, int valueStepPerClick, string petitionString)
    {
        OpenPetition.SetStaticParams(value, maxValue, valueStepPerClick, petitionString);
        SceneManager.LoadScene("OpenPetition");
    }

    public static void PlayBattle(List<HeroUnitGridInfo> alliedheroList, List<HeroUnitGridInfo> enemyHeroList)
    {
        GridCombatSystem.SetParams(alliedheroList, enemyHeroList);
        LoadingScreen.Instance.Show();
        Instance.szToLoad = "GameScene_GridCombatSystem";
        Instance.StartCoroutine(Instance.LoadAsyncScene());
    }
}
