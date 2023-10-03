using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{

    private List<Vector2Int> resolutionList;
    private int number = 0;

    private bool horizontal = false;

    private void Start()
    {
        resolutionList = new List<Vector2Int>()
        {
            new Vector2Int(1920, 1080),
            new Vector2Int(2688, 1242),
            new Vector2Int(2208, 1242),
            new Vector2Int(2732, 2048),
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetResolution(resolutionList[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetResolution(resolutionList[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetResolution(resolutionList[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetResolution(resolutionList[3]);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            number++;
            TakeScreenshotIE();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            horizontal = !horizontal;
        }
    }


    private void TakeScreenshotIE()
    {
        string m_Path = Application.dataPath;
        ScreenCapture.CaptureScreenshot(m_Path + "/" + Screen.width + "x" + Screen.height + "_" + number + ".png");
    }

    private void SetResolution(Vector2Int resolution)
    {
        if (horizontal)
        {
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
        }
        else
        {
            Screen.SetResolution(resolution.y, resolution.x, Screen.fullScreen);
        }
    }
}
