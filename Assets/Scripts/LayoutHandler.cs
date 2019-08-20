using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutHandler : MonoBehaviour {

    public GameObject leftPanel;
    private GameObject underlay;
    private Animator menuAnimator;
    private bool menuIsOpen;

    private void Awake()
    {
        menuAnimator = leftPanel.GetComponent<Animator>();
        menuIsOpen = false;
        underlay = leftPanel.transform.GetChild(0).gameObject;
        underlay.SetActive(menuIsOpen);
    }

    public void AnimateMenu(){
        menuIsOpen = !menuIsOpen;
        menuAnimator.SetBool("openMenu", menuIsOpen);
        underlay.SetActive(menuIsOpen);
    }

}
