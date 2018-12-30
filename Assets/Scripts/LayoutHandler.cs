using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutHandler : MonoBehaviour {

    public GameObject leftPanel;
    private Animator menuAnimator;
    private bool menuIsOpen;

    private void Awake()
    {
        menuAnimator = leftPanel.GetComponent<Animator>();
        menuIsOpen = false;
    }

    public void AnimateMenu(){
        menuIsOpen = !menuIsOpen;
        menuAnimator.SetBool("openMenu", menuIsOpen);
    }

}
