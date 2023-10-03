using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance { get; private set; }


    private Dictionary<QuestSO, QuestUI> questUIDictionary = new Dictionary<QuestSO, QuestUI>();

    [SerializeField] private Transform pfQuest;
    [SerializeField] private Transform questContainerTransform;
    [SerializeField] private QuestListSO questListSO;
    [SerializeField] private int maxActiveQuests;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private ResourceTypeSO coinResourceType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckCache();

        Vector2 startPos = new Vector2(0, -300);
        Vector2 offset = new Vector2(0, -160);


        int questsAlreadyCompleted = 0;
        int questsSpawned = 0;
        int questsSpawnedMax = 7;
        for (int i = 0; i < questListSO.list.Count; i++)
        {
            QuestSO questSO = questListSO.list[i];
            if (PlayerPrefs.HasKey(questSO.uniqueCode))
            {
                questsAlreadyCompleted++;
                continue;
            }
            Transform questTransform = Instantiate(pfQuest, questContainerTransform);
            //RectTransform questRectTransform = questTransform.GetComponent<RectTransform>();
            //questRectTransform.anchoredPosition = startPos + offset * (i - questsAlreadyCompleted);
            QuestUI questUI = questTransform.GetComponent<QuestUI>();
            questUIDictionary[questSO] = questUI;
            questUI.SetQuestSO(questSO, (i - questsAlreadyCompleted), () =>
            {
                PlayAppropriateGame(questSO);
            });
            if (i - questsAlreadyCompleted < maxActiveQuests)
            {
                questUI.Activate();
            }
            else
            {
                questUI.Inactivate();
            }

            questsSpawned++;
            if (questsSpawned >= questsSpawnedMax)
            {
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();
            SaveManager.needSave = false;
        }
    }

    private void CheckCache()
    {
        if (QuestCache.questCompleted)
        {
            QuestCache.questCompleted = false;
            PlayerPrefs.SetInt(QuestCache.questUniqueCode, 0);
            QuestSO questSO = GetQuestFromUniqueCode(QuestCache.questUniqueCode);
            StartCoroutine(coinSpawner.Spawn(questSO.coins, new Vector2(0, 0), 10f, new Vector3(18, 69), (int coins) =>
            {
                ResourceManager.Instance.AddResources(coinResourceType, coins);
            }));
        }
    }

    private QuestSO GetQuestFromUniqueCode(string uniqueCode)
    {
        foreach (QuestSO questSO in questListSO.list)
        {
            if (string.Equals(questSO.uniqueCode, uniqueCode))
            {
                return questSO;
            }
        }
        return null;
    }

    private void PlayAppropriateGame(QuestSO questSO)
    {
        QuestCache.questUniqueCode = questSO.uniqueCode;
        QuestCache.questCompleted = false;
        if (questSO is QuestCrashPlaneSO)
        {
            QuestCrashPlaneSO q = (QuestCrashPlaneSO)questSO;
            GameManager.PlayCrashPlane(q.totalPlanes, q.enemyPlanes, q.timeSpawnRate, q.planeSpeed, q.bulletSpeed);
            return;
        }
        if (questSO is QuestDecoderGameSO)
        {
            QuestDecoderGameSO q = (QuestDecoderGameSO)questSO;
            GameManager.PlayDecoderGame(q.width, q.height, q.activatedIndexList, q.crossActivation);
            return;
        }
        if (questSO is QuestFindTraitorSO)
        {
            QuestFindTraitorSO q = (QuestFindTraitorSO)questSO;
            GameManager.PlayFindTraitor(q.ukrainianSize, q.russianSize);
            return;
        }
        if (questSO is QuestMinesweeperSO)
        {
            QuestMinesweeperSO q = (QuestMinesweeperSO)questSO;
            GameManager.PlayMinesweeper(q.width, q.height, q.minesInLine);
            return;
        }
        if (questSO is QuestOpenPetitionSO)
        {
            QuestOpenPetitionSO q = (QuestOpenPetitionSO)questSO;
            GameManager.PlayOpenPetition(q.value, q.maxValue, q.valueStepPerClick, q.petitionString);
            return;
        }
        if (questSO is QuestWolfAndEggsSO)
        {
            QuestWolfAndEggsSO q = (QuestWolfAndEggsSO)questSO;
            GameManager.PlayWolfAndEggs(q.waves, q.timeBetweenWaves, q.moveSpeed);
            return;
        }
    }
}
