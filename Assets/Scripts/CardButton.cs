using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    private CardMover mover;
    private RectTransform rectTrans;
    public int timesClicked;
    public int nthCard;
    public float scaleFactor;
    private Vector2 oldPos;

    private void Start()
    {
        mover = GameObject.Find("Handler").GetComponent<CardMover>();
        rectTrans = GetComponent<RectTransform>();
        timesClicked = 0;
    }
    public void CardClicked()
    {
        if(timesClicked < 1)
        {
            mover.RearrangeHand(nthCard);
            timesClicked = 1;
            oldPos = rectTrans.position;
        }
        else
        {
            timesClicked++;
            mover.closeZoomButton.SetActive(true);
            rectTrans.position = new Vector2(transform.parent.GetComponent<RectTransform>().rect.width/2, transform.parent.position.y);
            rectTrans.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
        }
    }
    public void ZoomBack()
    {
        rectTrans.position = oldPos;
        rectTrans.localScale = new Vector3(1, 1, 1);
        timesClicked = 1;

        mover.closeZoomButton.SetActive(false);
    }
}
