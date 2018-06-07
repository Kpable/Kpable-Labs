using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InvestmentCanvasElement : MonoBehaviour {

    public delegate void ButtonEvent(InvestmentCanvasElement element);
    public ButtonEvent OnRunButtonPressed;
    public ButtonEvent OnPurchaseUpgradeButtonPressed;
    public ButtonEvent OnPurchaseAutorunButtonPressed;

    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI payoutText;
    [SerializeField]
    private TextMeshProUGUI upgradeCostText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Button runButton;
    [SerializeField]
    private Button upgradeButton;

    private bool autorunMode;

    public float Progress { set { progressImage.fillAmount = Mathf.Clamp01(value); } }
    public string Name { set { nameText.text = value; } }
    public float Payout { set { payoutText.text = "$" + ((int)value).ToString(); } }
    public float UpgradeCost { set { upgradeCostText.text = "$" + ((int) value).ToString(); } }
    public int Level { set { levelText.text = "Level " + value.ToString(); } }
    public bool AutoRunMode { get { return autorunMode; } }

    public bool RunButtonInteractable { set { runButton.interactable = value; } }
    public bool UpgradeButtonInteractable { set { upgradeButton.interactable = value; } }

    public void Run()
    {
        Debug.Log("InvestmentCanvasElement:Run: " + nameText.text);

        if (OnRunButtonPressed != null)
        {
            OnRunButtonPressed(this);
        }
    }

    public void Upgrade()
    {
        Debug.Log("InvestmentCanvasElement:Upgrade: " + nameText.text);
        if (autorunMode)
        {
            if (OnPurchaseAutorunButtonPressed != null)
            {
                OnPurchaseAutorunButtonPressed(this);
            }
        }
        else
        {
            if (OnPurchaseUpgradeButtonPressed != null)
            {
                OnPurchaseUpgradeButtonPressed(this);
            }
        }
    }

    public void SwitchMode()
    {
        autorunMode = !autorunMode;
        payoutText.gameObject.SetActive(!autorunMode);
        levelText.gameObject.SetActive(!autorunMode);
        runButton.gameObject.SetActive(!autorunMode);
        progressImage.gameObject.SetActive(!autorunMode);

    }
    
}
