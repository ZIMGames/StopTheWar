using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameEndGameObject;
    [SerializeField] private Text uiText;
    [TextArea] [SerializeField] private string messageString;

    private void Awake()
    {
        Country_Manager.onWholeCountriesOccupied = () =>
        {
            gameEndGameObject.SetActive(true);
            SFXMusic.Instance.PlayApplauses();
            TextWriter.AddWriter_Static(uiText, messageString, .04f, true, true, null);
        };

        //int times = 8;
        //while (times --> 0)
        //{
        //    int cnt = 1;
        //    while (!BoxProbabilities.bigBoxProbability.Draw())
        //    {
        //        cnt++;
        //    }
        //    Debug.Log(cnt);
        //}
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        SaveManager.needSave = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SaveManager.needSave = true;
    }
}
