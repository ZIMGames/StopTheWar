using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTraitor : MonoBehaviour
{
    public struct Photo
    {
        public int photoIndex;
        public bool isUkrainian;
    }


    public static System.Action onWinCallback = () => { Debug.Log("win"); };
    public static System.Action onLoseCallback = () => { Debug.Log("lose"); };


    public Sprite[] photoArray;
    [SerializeField] private Transform ukrainianContainer;
    [SerializeField] private Transform russianContainer;
    [SerializeField] private GameObject firstStageGameObject;
    [SerializeField] private GameObject secondStageGameObject;
    [SerializeField] private Image photoToIdentifyImage;
    [SerializeField] private Transform pfPhoto;

    private List<Photo> photoSequenceList;
    private int currentPhotoIndex;


    public static List<int> ukrainianIndexList = new List<int> { 0, 1, 2 };
    public static List<int> russianIndexList = new List<int> { 4, 5 };

    public static void SetStaticParams(int ukrainianSize, int russianSize)
    {
        int totalSize = ukrainianSize + russianSize;
        List<int> indexList = YouKo.UtilsClass.PickRandomIndices(23, totalSize);

        ukrainianIndexList.Clear();
        russianIndexList.Clear();
        for (int i = 0; i < totalSize; i++)
        {
            if (i < ukrainianSize)
            {
                ukrainianIndexList.Add(indexList[i]);
            }
            else
            {
                russianIndexList.Add(indexList[i]);
            }
        }
    }

    public static bool isEnded { get; private set; }


    private void Start()
    {
        isEnded = false;

        firstStageGameObject.SetActive(true);
        secondStageGameObject.SetActive(false);

        //clear containers
        foreach (Transform child in ukrainianContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in russianContainer)
        {
            Destroy(child.gameObject);
        }


        foreach (int index in ukrainianIndexList)
        {
            Transform photoTransform = Instantiate(pfPhoto, ukrainianContainer);
            Image photoImage = photoTransform.GetComponent<Image>();
            photoImage.sprite = photoArray[index];
        }

        foreach (int index in russianIndexList)
        {
            Transform photoTransform = Instantiate(pfPhoto, russianContainer);
            Image photoImage = photoTransform.GetComponent<Image>();
            photoImage.sprite = photoArray[index];
        }
    }

    public void ReadyForStart()
    {
        Debug.Log("start identifying");

        firstStageGameObject.SetActive(false);
        secondStageGameObject.SetActive(true);

        GeneratePhotoSequenceList();
        currentPhotoIndex = 0;
        ShowNextPhoto();
    }

    private void GeneratePhotoSequenceList()
    {
        photoSequenceList = new List<Photo>();

        foreach (int index in ukrainianIndexList)
        {
            Photo photo = new Photo { isUkrainian = true, photoIndex = index };
            photoSequenceList.Add(photo);
        }

        foreach (int index in russianIndexList)
        {
            Photo photo = new Photo { isUkrainian = false, photoIndex = index };
            photoSequenceList.Add(photo);
        }

        YouKo.UtilsClass.Shuffle(ref photoSequenceList);
    }

    private void ShowNextPhoto()
    {
        if (currentPhotoIndex >= photoSequenceList.Count)
        {
            isEnded = true;
            onWinCallback?.Invoke();
            secondStageGameObject.SetActive(false);
            return;
        }

        Photo photo = photoSequenceList[currentPhotoIndex];
        photoToIdentifyImage.sprite = photoArray[photo.photoIndex];
    }

    public void IdentifyAsUkrainian()
    {
        if (isEnded)
            return;

        Photo photo = photoSequenceList[currentPhotoIndex];
        if (photo.isUkrainian)
        {
            SFXMusic.Instance.PlayCorrectAnswerSound();
            currentPhotoIndex++;
            ShowNextPhoto();
        }
        else
        {
            isEnded = true;
            secondStageGameObject.SetActive(false);
            onLoseCallback?.Invoke();
        }
    }

    public void IdentifyAsRussian()
    {
        if (isEnded)
            return;

        Photo photo = photoSequenceList[currentPhotoIndex];
        if (!photo.isUkrainian)
        {
            SFXMusic.Instance.PlayCorrectAnswerSound();
            currentPhotoIndex++;
            ShowNextPhoto();
        }
        else
        {
            isEnded = true;
            secondStageGameObject.SetActive(false);
            onLoseCallback?.Invoke();
        }
    }
}
