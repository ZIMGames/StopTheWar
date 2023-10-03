using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minesweeper_BlockSingle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject whenPressed;
    public Image image;
    public new Animation animation;

    private Minesweeper minesweeper;



    public void OnPointerClick(PointerEventData eventData)
    {
        minesweeper.Select(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        minesweeper.OnBlockEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        minesweeper.OnBlockExit(this);
    }

    public void Setup(Minesweeper minesweeper)
    {
        this.minesweeper = minesweeper;
    }
}
