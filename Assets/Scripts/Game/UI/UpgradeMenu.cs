using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Canvas canvas;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeText;
    public Button sellButton;
    public TextMeshProUGUI sellText;
    public Image upgradeImage;
    private Tower tower;
    private Main main;

    private void Start()
    {
        main = FindFirstObjectByType<Main>();
        StartCoroutine(init());
    }

    private void Update()
    {
        GameData gamedata = main.gameData;
        if (tower == null || !canvas.enabled)
        {
            return;
        }
        upgradeButton.interactable = (tower.level < tower.maxLevel && gamedata.money >= tower.upgradeCost);
    }

    private IEnumerator init()
    {
        yield return null;
        CloseMenu();
    }
    public void ShowMenu(Tower SelectedTower)
    {
        GameData gamedata = main.gameData;
        if(tower != null)
        {
            tower.setRadiusOpacity(0);
        }
        tower = SelectedTower;
        if (tower != null)
        {
            tower.setRadiusOpacity(0.3f);
        }
        else
        {
            Debug.Log("tower is null");
        }
        canvas.enabled = true;
        upgradeText.text = "Upgrade\n" + tower.upgradeCost;
        sellText.text = "Sell\n" + tower.sellValue;
        if(tower.NextLevelSprite != null)
        {
            upgradeImage.sprite = tower.NextLevelSprite;
        }
        else
        {
            SpriteRenderer spriteRenderer = tower.GetComponent<SpriteRenderer>();
            upgradeImage.sprite = spriteRenderer.sprite;
        }
    }

    public void UpgradeTower()
    {
        if (tower != null)
        {
            tower.Upgrade();
        }
    }

    public void SellTower()
    {
        if (tower != null)
        {
            tower.Sell();
            canvas.enabled = false;
        }
    }

    public void CloseMenu()
    {
        canvas.enabled = false;
        if (tower != null)
        {
            tower.setRadiusOpacity(0);
        }
    }
}
