using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class BaseBox : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private GameObject cachedBase;
    [SerializeField] private Image image;
    private BuildBox buildBox;
    private bool selected;
    private RectTransform statInfoBox;
    private UpgradeButton upgradeButton;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    [SerializeField] [FMODUnity.EventRef] private string returnSFX;
    private Material _material;
    private ParticleSystem activeVFX;
    private Light2D light2D;

    [Header("Light Colors")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color selectable;
    [SerializeField] private Color notSelectable;
    private GameObject replacedBase;

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
        activeVFX = GetComponentInChildren<ParticleSystem>();
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
        light2D = GetComponentInChildren<Light2D>();
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        upgradeButton = FindObjectOfType<UpgradeButton>();
    }

    public void GenerateNewBase()
    {
        cachedBase = TurretConstructor.Main.GetBase();
        var sprite = cachedBase.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
        GetComponentsInChildren<RectTransform>()[1].localScale = new Vector2(1, 0);
        GetComponentInChildren<ExpandAnimation>().Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(cachedBase == null || RewardManager.Main.ActiveSelection)
        {
            AudioManager.Main.PlayInvalidSelection();
            return;
        }
        if(buildBox.CheckCompability(cachedBase.GetComponent<BaseEffectTemplate>()) && !buildBox.CheckBaseBox(this)) 
        {
            if(!buildBox.OnUpgrade)
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                buildBox.ReceiveBase(cachedBase, this);
                selected = true;
                return;
            }
            else if(upgradeButton.onUpgrade)
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                replacedBase = buildBox.selectedBase;
                buildBox.PreviewBaseEffect(cachedBase, buildBox.selectedWeapon);
                buildBox.ReceiveBase(cachedBase, this);
                buildBox.baseToReplace = cachedBase;
                buildBox.SetCostToBaseCost(true);
                selected = true;

                return;
            }
        }
        else if(buildBox.CheckBaseBox(this))
        {
            if(upgradeButton.onUpgrade)
            {
                AudioManager.Main.RequestGUIFX(returnSFX);
                buildBox.selectedWeapon.GetComponent<ActionController>().LoadStats();
                buildBox.ClearBase(out cachedBase);
                buildBox.ReceiveBase(replacedBase);
                buildBox.SetCostToBaseCost(false);
                image.color = Color.white;
                selected = false;
                return;
            }

            AudioManager.Main.RequestGUIFX(returnSFX);
            buildBox.ClearBase(out cachedBase);
            image.color = Color.white;
            selected = false;
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    void Update()
    {
        light2D.color = notSelectable;
        if(cachedBase == null) return;
        var check = buildBox.CheckCompability(cachedBase.GetComponent<BaseEffectTemplate>()) && cachedBase && !buildBox.OnUpgrade;
        if(check || upgradeButton.onUpgrade) light2D.color = selectable;
    }
    
    public void Unselect()
    {
        activeVFX.Stop();
    }


    public void Detach()
    {
        activeVFX.Stop();
        cachedBase = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        activeVFX.Stop();
        Destroy(cachedBase);
        image.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _material.SetInt("_Moving", 0);
        statInfoBox.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cachedBase == null) return;
        _material.SetInt("_Moving", 1);
        AudioManager.Main.RequestGUIFX(hoverSFX);

        var text = "<size=150%><lowercase>" + cachedBase.name;

        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(text, 200);
        }
    }
}