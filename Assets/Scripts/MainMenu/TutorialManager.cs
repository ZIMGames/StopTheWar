using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Image tutorImage;
    [SerializeField] private Transform[] messageTransformArray;

    private List<TutorialPart> tutorialPartList = new List<TutorialPart>
    {
        new TutorialPart {messageStringIndex = 0, needTutor = true, messageObjectIndex = 0},
        new TutorialPart {messageStringIndex = 1, needTutor = true, messageObjectIndex = 0},
        new TutorialPart {messageStringIndex = 2, needTutor = false, messageObjectIndex = 1},
        new TutorialPart {messageStringIndex = 3, needTutor = false, messageObjectIndex = 2},
        new TutorialPart {messageStringIndex = 4, needTutor = false, messageObjectIndex = 2},
        new TutorialPart {messageStringIndex = 5, needTutor = false, messageObjectIndex = 3},
        new TutorialPart {messageStringIndex = 6, needTutor = false, messageObjectIndex = 4},
    };

    private TextWriter.TextWriterSingle textWriterSingle;

    private int currentTutorialPart;

    private List<string> ukrMessageList = new List<string>()
    {
        "Настали тяжкі часи... Але ми боремося до кінця!",
        "Еххххххххххь!",
        "Тут у магазині ти знайдеш ящики, відкриваючи котрі зможеш прокачати бійців!",
        "Тут тобі доведеться відвоювати всі наші області!",
        "Навіть у разі програшу ти отримаєш монетки за кожного вбитого ворога!",
        "Панель для прокачки бійців!",
        "Тут є квести, виконуючи котрі зможеш заробити трішки грошей!",

    };
    private List<string> rusMessageList = new List<string>()
    {
        "Настали тяжелые времена... Но мы боремся до конца!",
        "Эххххххххххь!",
        "Здесь в магазине ты найдешь ящики, открывая которые сможешь прокачать бойцов!",
        "Здесь тебе прийдется отвоевать все наши области!",
        "Даже в случае поражения ты получишь монетки за каждого убитого врага!",
        "Панель для прокачки бойцов!",
        "Тут есть квесты, выполняя которые сможешь заработать немного денег!",

    };
    private List<string> engMessageList = new List<string>()
    {
        "These are troubling times... but we fight to the end!",
        "Uhhhhhhhh!",
        "In the shop you will find boxes, you can upgrade the fighters by opening the boxes!",
        "This is the place where the fight is going to pop! Get regions back",
        "Even if you lose, you get coins for every enemy you kill!",
        "Upgrade fighters there!",
        "There are quests that you can earn some money from!",

    };
    private List<string> currentTranslationList;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            if (Application.systemLanguage == SystemLanguage.Ukrainian)
            {
                currentTranslationList = ukrMessageList;
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                //currentTranslationList = rusMessageList;
                currentTranslationList = ukrMessageList;
            }
            else
            {
                currentTranslationList = engMessageList;
            }

            tutorialPanel.SetActive(true);
            //Vector3 startLocalPos = messageRectTransform.anchoredPosition;
            //Vector3 offset = new Vector3(0, 20);
            //LeanTween.moveLocal(messageRectTransform.gameObject, startLocalPos + offset, 1.5f).setOnComplete(() =>
            //{
            //    LeanTween.moveLocal(messageRectTransform.gameObject, startLocalPos - offset, 1.5f);
            //});
            currentTutorialPart = -1;
            Invoke("Process", .4f);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void Process()
    {
        if (textWriterSingle != null && textWriterSingle.IsActive())
        {
            textWriterSingle.WriteAllAndDestroy();
        }
        else
        {
            currentTutorialPart++;
            if (currentTutorialPart >= tutorialPartList.Count)
            {
                tutorialPanel.SetActive(false);
                PlayerPrefs.SetInt("Tutorial", 0);
                return;
            }
            TutorialPart tutorialPart = tutorialPartList[currentTutorialPart];
            tutorImage.gameObject.SetActive(tutorialPart.needTutor);
            for (int i = 0; i < messageTransformArray.Length; i++)
            {
                if (i == tutorialPart.messageObjectIndex)
                {
                    messageTransformArray[i].gameObject.SetActive(true);
                }
                else
                {
                    messageTransformArray[i].gameObject.SetActive(false);
                }
            }
            Text uiText = messageTransformArray[tutorialPart.messageObjectIndex].Find("Panel").Find("messageText").GetComponent<Text>();
            textWriterSingle = TextWriter.AddWriter_Static(uiText, currentTranslationList[tutorialPart.messageStringIndex], .06f, true, true, null);
        }
    }

    public class TutorialPart
    {
        public int messageStringIndex;
        public bool needTutor;
        public int messageObjectIndex;
    }
}
