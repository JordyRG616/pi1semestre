using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponBox : MonoBehaviour, IPointerClickHandler
{
    private GameObject cachedWeapon;
    [SerializeField] private Image image;
    private BuildBox buildBox;
    private bool selected;

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
    }

    public void GenerateNewWeapon(RewardLevel level)
    {
        cachedWeapon = TurretConstructor.Main.GetTop(level);
        var sprite = cachedWeapon.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(buildBox.CheckCompability(cachedWeapon.GetComponent<ActionController>()) && !selected) 
        {
            buildBox.ReceiveWeapon(cachedWeapon, this);
            selected = true;
        }
        else if(selected)
        {
            buildBox.ClearWeapon(out cachedWeapon);
            image.color = Color.white;
            selected = false;
        }
    }

    public void Detach()
    {
        cachedWeapon = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        Destroy(cachedWeapon);
        image.color = Color.clear;
    }
}
