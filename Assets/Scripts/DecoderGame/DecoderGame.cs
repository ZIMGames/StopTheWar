using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoderGame : MonoBehaviour
{
    public static System.Action onWinCallback = () => { Debug.Log("decoded!"); };

    public static int width = 4;
    public static int height = 4;
    public static List<int> activatedIndexList = new List<int> {0};
    public static bool crossActivation = false;

    public static void SetStaticParams(int _width, int _height, List<int> _activatedIndexList, bool _crossActivation)
    {
        width = _width;
        height = _height;
        activatedIndexList = _activatedIndexList;
        crossActivation = _crossActivation;
    }

    public float offsetX;
    public float offsetY;

    [SerializeField] private Transform pfActivator;
    [SerializeField] private Transform activatorContainer;

    private bool[,] field;
    private DecoderGame_ActivatorSingle[,] activatorSingleArray;



    private bool isEnded = false;

    private void Start()
    {
        field = new bool[width, height];
        activatorSingleArray = new DecoderGame_ActivatorSingle[width, height];

        Vector2 borders = new Vector2(-offsetX * (width - 1) / 2, -offsetY * (height - 1) / 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * width + j;
                if (activatedIndexList.Contains(index))
                {
                    field[i, j] = true;
                }

                Transform activatorTransform = Instantiate(pfActivator, activatorContainer);
                activatorTransform.position = borders + new Vector2(i * offsetX, j * offsetY);
                //RectTransform activatorRectTransform = activatorTransform.GetComponent<RectTransform>();
                //activatorRectTransform.anchoredPosition = borders + new Vector2(i * offsetX, j * offsetY);
                DecoderGame_ActivatorSingle activatorSingle = activatorTransform.GetComponent<DecoderGame_ActivatorSingle>();
                activatorSingle.UpdateState(field[i, j]);
                int x = i;
                int y = j;
                activatorSingle.SetOnClickCallback(() =>
                {
                    OnClick(x, y);
                });
                activatorSingleArray[i, j] = activatorSingle;
            }
        }
    }

    private void Update()
    {
        //TESTING
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            field[i, j] = true;
        //            activatorSingleArray[i, j].UpdateState(field[i, j]);
        //        }
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    crossActivation = !crossActivation;
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    onWinCallback = () => { Debug.Log("sasuke"); };
        //}
    }

    private void OnClick(int i, int j)
    {
        if (isEnded)
            return;


        for (int x = 0; x < width; x++)
        {
            ChangeStateOnActivator(x, j);
        }
        for (int y = 0; y < height; y++)
        {
            ChangeStateOnActivator(i, y);
        }
        if (crossActivation)
        {
            ChangeStateOnActivator(i, j);
        }

        CheckGameState();
    }

    private void ChangeStateOnActivator(int i, int j)
    {
        field[i, j] = !field[i, j];
        activatorSingleArray[i, j].UpdateState(field[i, j]);
    }

    private void CheckGameState()
    {
        bool res = true;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                res = res && field[i, j];
            }
        }
        if (res)
        {
            isEnded = true;
            onWinCallback?.Invoke();
        }
    }
}
