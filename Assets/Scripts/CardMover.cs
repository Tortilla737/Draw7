using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    public GameObject cardContainer;
    public GameObject startPoint;
    public GameObject cardPref;
    public GameObject pictureModeIcon;
    public GameObject animModeIcon;
    public GameObject closeZoomButton;
    public float smoothTime;
    public bool pictureMode = true;
    private bool animatedMode = false;

    
    public void ToggleAnimatedMode()
    {
        animatedMode = !animatedMode;

        animModeIcon.SetActive(animatedMode);
    }
    public void TogglePictureMode()
    {
        pictureMode = !pictureMode;

        pictureModeIcon.SetActive(pictureMode);
    }

    public void AnimateToPosition(GameObject panel)
    {
        int nthCard = panel.GetComponent<CardButton>().nthCard;
        Vector2 newPos = GetNewPosition(nthCard);
        
        if (animatedMode)
        {
            MoveAnimation(panel, newPos);
        }
        else
        {
            StopAllCoroutines();
            panel.transform.position = newPos;
        }
    }
    private Vector2 GetNewPosition(int nthCard)
    {
        Rect containerRect = cardContainer.GetComponent<RectTransform>().rect;

        float minX = cardContainer.transform.position.x - (containerRect.width / 2);
        float maxX = cardContainer.transform.position.x + (containerRect.width / 2);
        minX = minX + (cardPref.GetComponent<RectTransform>().rect.width/2);
        maxX = maxX - (cardPref.GetComponent<RectTransform>().rect.width/2);
        float deltaX = maxX - minX;

        float newY = cardContainer.transform.position.y - (containerRect.height / 2);
        if (nthCard % 2 == 1)   //wenn die gezogene Karte ungerade wird, kommt sie nach oben
        {
            newY = newY + (containerRect.height * 3 / 4);
            deltaX = deltaX / GetComponent<DrawHand>().cardsDrawn;
            deltaX = deltaX * nthCard;
        }
        else
        {
            newY = newY + (containerRect.height / 4);
            deltaX = deltaX / GetComponent<DrawHand>().cardsDrawn;
            deltaX = deltaX * (nthCard-1);
        }
        
        Vector2 posBuffer = new Vector2(minX + deltaX, newY);
        return posBuffer;
    }
    private void MoveAnimation(GameObject objToMove, Vector2 newPos)
    {
        StartCoroutine(MoveAnimationCoRo(objToMove, new Vector3(newPos.x, newPos.y, 0)));
    }
    private IEnumerator MoveAnimationCoRo(GameObject objToMove, Vector3 newPos)
    {
        while (Vector3.Distance(objToMove.transform.position, newPos) > 1)
        {
            objToMove.transform.position = Vector3.Slerp(objToMove.transform.position, newPos, smoothTime * Time.deltaTime);

            //Debug.Log("running");
            yield return null;
        }
        yield return null;
    }

    public void RearrangeHand(int nthCard)
    {
        int drawn = GetComponent<DrawHand>().cardsDrawn;
        List<GameObject> panels = GetComponent<DrawHand>().cardPanels;

        for (int i = 1 - (nthCard % 2); i < nthCard; i = i + 2)
        {
            panels[i].GetComponent<CardButton>().timesClicked = 0;
            panels[i].transform.SetAsLastSibling();
        }
        for (int i = drawn-(1 + Math.Abs((nthCard%2) - (drawn%2))); i > nthCard; i = i-2)
        {
            panels[i].GetComponent<CardButton>().timesClicked = 0;
            panels[i].transform.SetAsLastSibling();
        }
        panels[nthCard - 1].transform.SetAsLastSibling();
    }
    public void CloseZoomedCard()
    {
        foreach (GameObject panel in GetComponent<DrawHand>().cardPanels)
        {
            if (panel.GetComponent<CardButton>().timesClicked > 1)
            {
                panel.GetComponent<CardButton>().ZoomBack();
            }
        }
    }
}
