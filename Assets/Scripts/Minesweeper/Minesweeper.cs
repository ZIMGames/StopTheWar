using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minesweeper : MonoBehaviour
{

    public static System.Action onWinCallback = () => { Debug.Log("win"); };
    public static System.Action onLoseCallback = () => { Debug.Log("lose"); } ;

    private bool[,] field;
    private Minesweeper_BlockSingle[,] blockSingleArray;

    public bool isVertical;
    private int currentActiveLine = 0;

    public Sprite activeSprite;
    public Sprite hoverSprite;
    public Sprite idleSprite;

    public static int width = 5;
    public static int height = 6;
    public static int minesInLine = 1;


    public static void SetStaticParams(int _width, int _height, int _minesInLine)
    {
        width = _width;
        height = _height;
        minesInLine = _minesInLine;
    }

    private float mineOffset = 50f;
    private float cellSize;
    private float offsetX;
    private float offsetY = 250f;

    [Space]
    public Transform pfMine;
    public Transform pfFree; //START POSITION
    public Transform contentTransform;

    private bool isEnded = false;

    private Vector2 startPos;

    private void Awake()
    {
        PrepareField();
    }

    public void PrepareField()
    {
        ClearField();

        field = new bool[width, height];

        blockSingleArray = new Minesweeper_BlockSingle[width, height];

        if (!isVertical)
        {
            for (int i = 0; i < width; i++)
            {
                List<int> indices = YouKo.UtilsClass.PickRandomIndices(height, minesInLine);
                foreach (int j in indices)
                {
                    field[i, j] = true;
                }
            }
        }
        else
        {
            for (int j = 0; j < height; j++)
            {
                List<int> indices = YouKo.UtilsClass.PickRandomIndices(width, minesInLine);
                foreach (int i in indices)
                {
                    field[i, j] = true;
                }
            }
        }


        cellSize = (1080 - (width + 1) * mineOffset) / width;
        offsetX = cellSize + mineOffset;
        startPos = new Vector2(mineOffset + cellSize * .5f, Mathf.Max(offsetY - cellSize * .5f, cellSize * .5f + 10f));


        pfMine.gameObject.SetActive(false);
        pfFree.gameObject.SetActive(false);

        currentActiveLine = 0;
        isEnded = false;

        SpawnBlocks();
        OpenNewLine();
    }

    private void ClearField()
    {
        if (blockSingleArray != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var blockSingle = blockSingleArray[i, j];
                    Destroy(blockSingle.gameObject);
                }
            }
        }
    }


    private void SpawnBlocks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Transform prefab;
                if (field[i, j])
                {
                    prefab = pfMine;
                }
                else
                {
                    prefab = pfFree;
                }


                var instance = Instantiate(prefab, contentTransform);
                RectTransform instanceRectTransform = instance.GetComponent<RectTransform>();
                instanceRectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                instanceRectTransform.anchoredPosition = GetSpawnPosition(i, j);
                Minesweeper_BlockSingle blockSingle = instance.GetComponent<Minesweeper_BlockSingle>();
                blockSingle.whenPressed.SetActive(false);
                blockSingle.image.sprite = idleSprite;
                blockSingle.Setup(this);
                instance.gameObject.SetActive(true);

                blockSingleArray[i, j] = blockSingle;
            }
        }
    }

    public void OnBlockEnter(Minesweeper_BlockSingle blockSingle)
    {
        GetIndexOfSingleBlock(blockSingle, out int i, out int j);
        if (!isVertical && (i != currentActiveLine || isEnded))
            return;
        if (isVertical && (j != currentActiveLine || isEnded))
            return;

        blockSingle.image.sprite = hoverSprite;
    }

    public void OnBlockExit(Minesweeper_BlockSingle blockSingle)
    {
        GetIndexOfSingleBlock(blockSingle, out int i, out int j);
        if (!isVertical && (i != currentActiveLine || isEnded))
            return;
        if (isVertical && (j != currentActiveLine || isEnded))
            return;

        blockSingle.image.sprite = activeSprite;
    }

    public void Select(Minesweeper_BlockSingle blockSingle)
    {
        GetIndexOfSingleBlock(blockSingle, out int i, out int j);
        if (!isVertical && (i != currentActiveLine || isEnded))
            return;
        if (isVertical && (j != currentActiveLine || isEnded))
            return;

        //show all line
        if (!isVertical)
        {
            for (int y = 0; y < height; y++)
            {
                blockSingleArray[i, y].whenPressed.SetActive(true);
            }
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                blockSingleArray[x, j].whenPressed.SetActive(true);
            }
        }
        //blockSingle.whenPressed.SetActive(true);
        //blockSingle.animation.Play();

        currentActiveLine++;
        if (field[i, j]) //there's mine at this position
        {
            SFXMusic.Instance.PlayBomb();

            onLoseCallback?.Invoke();
            isEnded = true;
        }
        else
        {
            SFXMusic.Instance.PlayBombSwepped();

            if (!isVertical && i == width - 1)
            {
                onWinCallback?.Invoke();
                isEnded = true;
            }
            else if (isVertical && j == height - 1)
            {
                onWinCallback?.Invoke();
                isEnded = true;
            }
            else
            {
                OpenNewLine();
            }
        }
        DisablePreviousLine();
    }

    private void DisablePreviousLine()
    {
        if (!isVertical)
        { 
            for (int j = 0; j < height; j++)
            {
                var blockSingle = blockSingleArray[currentActiveLine - 1, j];
                blockSingle.animation.Stop();
            }
        }
        else
        {
            for (int i = 0; i < width; i++)
            {
                var blockSingle = blockSingleArray[i, currentActiveLine - 1];
                blockSingle.animation.Stop();
            }
        }
    }

    private void OpenNewLine()
    {
        if (!isVertical)
        {
            for (int j = 0; j < height; j++)
            {
                var blockSingle = blockSingleArray[currentActiveLine, j];
                blockSingle.image.sprite = activeSprite;
                blockSingle.animation.Play();
            }
        }
        else
        {
            for (int i = 0; i < width; i++)
            {
                var blockSingle = blockSingleArray[i, currentActiveLine];
                blockSingle.image.sprite = activeSprite;
                blockSingle.animation.Play();
            }
        }
    }

    private void GetIndexOfSingleBlock(Minesweeper_BlockSingle blockSingle, out int i, out int j)
    {
        i = 0;
        j = 0;

        for (i = 0; i < width; i++)
        {
            for (j = 0; j < height; j++)
            {
                if (blockSingleArray[i, j] == blockSingle)
                {
                    return;
                }
            }
        }
    }

    private Vector2 GetSpawnPosition(int i, int j)
    {
        float newX = startPos.x + i * offsetX;
        float newY = startPos.y + j * offsetY;
        return new Vector2(newX, newY);
    }
}
