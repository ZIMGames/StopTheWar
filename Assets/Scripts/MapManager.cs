using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private List<Transform> pointTransformList;
    [SerializeField] private MovePositionDirect iMovePosition;
    [SerializeField] private Transform particles;
    [SerializeField] private GameObject mapGameObject;


    private void Start()
    {
        Hide();

        Country_Manager.onFullTerritoryOccupied = (int countryIndex) =>
        {
            Debug.Log("hohma");
            Show();
            iMovePosition.transform.position = pointTransformList[countryIndex].position;
            iMovePosition.SetMovePosition(iMovePosition.transform.position, null);
            CompletePoint(countryIndex);
        };
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        CompletePoint();
    //    }
    //}

    public void Show()
    {
        mapGameObject.SetActive(true);
    }

    public void Hide()
    {
        mapGameObject.SetActive(false);
    }

    public void CompletePoint(int currentProgressIndex)
    {
        int currentProgressIndexCopy = currentProgressIndex;
        FunctionTimer.Create(() =>
        {
            pointTransformList[currentProgressIndexCopy].GetComponent<Animator>().Play("circleGetsGreenAnimation");
        }, 1.3f);
        currentProgressIndex++;
        pointTransformList[currentProgressIndex].GetComponent<Animator>().Play("circleFrameAnimation");

        FunctionTimer.Create(() =>
        {
            iMovePosition.SetMovePosition(pointTransformList[currentProgressIndex].position, () =>
            {
                Hide();
                Transform particlesTransform = Instantiate(particles, null);
                Destroy(particlesTransform.gameObject, 4f);
            });
        }, 1f);
    }
}
