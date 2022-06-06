using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RerrollButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private RectTransform tipBox;
    [SerializeField] private UIAnimations cashTextAnim;
    public int rerrollCost;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private Sprite ogSprite;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;
    private int offerTimelineIndex;

    public event EventHandler OnReroll;

    void Start()
    {
        rewardManager = RewardManager.Main;
        image = GetComponent<Image>();

        ogSprite = image.sprite;

        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();

        buildBox = FindObjectOfType<BuildBox>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
        tipBoxText.text = "reset (" + rerrollCost + "$)";
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        var locked = FindObjectOfType<LockButton>().locked;

        if(rewardManager.TotalCash >= rerrollCost && !locked && !buildBox.OnUpgrade)
        {
            AudioManager.Main.RequestGUIFX(clicksSFX);
            image.sprite = clickSprite;

            rewardManager.SpendedCash = 2;  
            cashTextAnim.PlayReverse();

            Reroll();
            OnReroll?.Invoke(this, EventArgs.Empty);
            return;
        } else
        {
            if(locked) AudioManager.Main.PlayInvalidSelection("Offer is locked");
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
    }

    public void Reroll()
    {
        rewardManager.EliminateOffer();
        rewardManager.GenerateOffer();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Invoke("ResetSprite", .1f);
    }

    private void ResetSprite()
    {
        image.sprite = ogSprite;
    }
}
