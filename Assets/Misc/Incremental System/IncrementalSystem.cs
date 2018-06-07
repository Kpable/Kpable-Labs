using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kpable.Mechanics;
using TMPro;


/// <summary>
/// 
/// New Feature - Autorun (single) [COMPLETE 032518]
/// 
/// Autorun - basic, adventure capitalist "manager" style. Purchase once and always enabled
///     Reqs:
///     expose the option to purchase autorun in the UI per investment
///         chose to toggle existing investment canvas element to be able to purchase autorun 
///         needed to add a menu button to toggle the investments into this mode
///     display seperate autorun purchase cost
///         updated existing purchase upgrade button to display autorun cost value on mode switch
///         update to switch back when exiting autorun purchase mode
///     validate autorun purchase cost
///         when entering the mode - perform a validation on mode enter through added menu button
///         when purchasing an autorun - reducing the currency triggers validation
///     disable the option to purchase after purchasing
///         this is done in validation
///     continue running investment after completion
///         During investment run check, added flag to check for autorun. if enabled does not disable running flag
///     deduct autorun cost on purchase, triggering validation
///         added deduction in button press action
///     save autorun status
///         saved an array of bools similar to the array of ints for levels
///         potential alternative is to create full investment save object containng all relevant information. 
///     load autorun status
///         load the autorun statuses similarly to level data
///     !!! save investment progress? if not it restarts every time even if autorun is enabled. 
///     
/// Feature Update - Autorun (multiple) [INCOMPLETE]
/// 
/// Autorun (multiple) - idle oil tycoon "ceo" style. idea is to introduce a delay before running again with autorun. 
///     Delay is a purchasable upgrade that reduces each time an upgrad is purchased. 
///     Reqs:
///     need to expose in the editor 
///         if autorun is feature of this system or not
///         if autorun will have multiple levels
///         define how many levels
///         ? expose delay between runs per level
///         ? expose reduction formula (Investment-Time / CEO level)
///         
/// Feature - Batch purchases 1x 10x 100x Max [COMPLETE 032518]
///     Reqs:
///     toggle mode between 4 purchase settings
///     calculate cost among the 4 purchase settings
///     max needs to dynamically update
///     validate on cost change
///     TODO currency may change between calls to getcurrentcost and getmaxnumberoflevels
///     do not apply to autorun purchases
///     
/// Feature - Currency type - eventually these numbers will start getting huge, need a way(s) to display them
///     Reqs:
///     Display full number until million 0 - 999,999. Then display truncated with suffix, 1.25M 
///     ? Make significant digits customizable?
///     idle oil tycoon uses both scientific notation (3.18E7) or compounded SI unit suffixes (K, M, G, T, P, E, Z, Y, KY, ... YY, KYY)
///     endless frontier saga uses letter notation, a, b, c, ..aa, ab, ac
///
/// </summary>
public class IncrementalSystem : MonoBehaviour {

    public string fileName = "IncrementalSystemEditorDemo.json";

    public Transform investmentCanvasElementContainer;
    public GameObject investementCanvasElementPrefab;
    public CurrencyCanvasElement currencyCanvasElement;
    public TextMeshProUGUI purchaseModeText;

    IncrementalSystemModel data;

    public static ObservedValue<float> currency = new ObservedValue<float>(10);

    InvestmentView[] investments;

    public enum PurchaseMode { X1, X10, X100, Max, Total }
    private PurchaseMode purchaseMode = PurchaseMode.X1;

    // Use this for initialization
    void Start() {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        string systemData = DataManager.LoadJson(filePath);

        data = JsonUtility.FromJson<IncrementalSystemModel>(systemData);

        currency.OnValueChanged += Currency_OnValueChanged;

        investments = new InvestmentView[data.investments.Length];

        for (int i = 0; i < data.investments.Length; i++)
        {
            GameObject investmentCanvasElement = Instantiate(investementCanvasElementPrefab);
            investmentCanvasElement.transform.SetParent(investmentCanvasElementContainer, false);
            var hooks = investmentCanvasElement.GetComponent<InvestmentCanvasElement>();
            hooks.Name = data.investments[i].name;

            // linking up hooks from UI
            hooks.OnRunButtonPressed = RunInvestment;
            hooks.OnPurchaseUpgradeButtonPressed = Upgrade;
            hooks.OnPurchaseAutorunButtonPressed = Autorun;

            data.investments[i].OnTimerExpired += AddCurrency;

            investments[i] = new InvestmentView( data.investments[i], hooks);

            // intialization of investment data;
            data.investments[i].CurrentDuration = data.investments[i].baseDuration;
        }

        LoadGame();

        // Fire it off once to get the value;
        Currency_OnValueChanged();
        ValidateInvestmentCosts();

        UpdateUI();

    }

    private void Currency_OnValueChanged()
    {
        currencyCanvasElement.Currency = currency.Value;
        ValidateInvestmentCosts();
        if (purchaseMode == PurchaseMode.Max)
            UpdateUI();
    }

    // Ensure Purchase buttons arent interactable if player cant afford it. 
    void ValidateInvestmentCosts()
    {
        foreach (InvestmentView investmentView in investments)
        {
            if (investmentView.hooks.AutoRunMode)
            {
                // Check if player unlocked investment && not already purchased && if they can afford it
                investmentView.hooks.UpgradeButtonInteractable =
                     investmentView.investment.Level > 0 &&
                     !investmentView.investment.Autorun && 
                     currency.Value >= investmentView.investment.AutorunCost;
            }
            else
            {
                // Check if they can afford it
                investmentView.hooks.UpgradeButtonInteractable =
                     currency.Value >= GetCurrentCost(investmentView);
            }
        }
    }

    // Updates the UI of all the investments
    void UpdateUI()
    {
        foreach (InvestmentView investmentView in investments)
        {
            UpdateUISingle(investmentView);
        }
    }

    // Updates the UI of a single investment
    void UpdateUISingle(InvestmentView investmentView)
    {
        investmentView.hooks.Payout = investmentView.investment.Payout;
        investmentView.hooks.UpgradeCost = 
            investmentView.hooks.AutoRunMode ? investmentView.investment.AutorunCost : GetCurrentCost(investmentView);
        investmentView.hooks.Level = investmentView.investment.Level;
    }

    float GetCurrentCost(InvestmentView investmentView)
    {
        float currentCost = 0;

        switch (purchaseMode)
        {
            case PurchaseMode.X1:
                currentCost = investmentView.investment.Cost;
                break;
            case PurchaseMode.X10:
                currentCost = investmentView.investment.Costx10;
                break;
            case PurchaseMode.X100:
                currentCost = investmentView.investment.Costx100;
                break;
            case PurchaseMode.Max:
                if (GetCurrentNumberOfLevels(investmentView) == 0)
                    currentCost = investmentView.investment.Cost;
                else
                    currentCost = investmentView.investment.MaxCost(currency.Value);
                break;
            case PurchaseMode.Total:    // should never be this
            default:
                currentCost = investmentView.investment.Cost;
                break;
        }

        return currentCost;
    }

    int GetCurrentNumberOfLevels(InvestmentView investmentView)
    {
        int numberOfLevels = 0;

        switch (purchaseMode)
        {
            case PurchaseMode.X1:
                numberOfLevels = 1;
                break;
            case PurchaseMode.X10:
                numberOfLevels = 10;
                break;
            case PurchaseMode.X100:
                numberOfLevels = 100;
                break;
            case PurchaseMode.Max:
                numberOfLevels = investmentView.investment.MaxNumberOfLevels(currency.Value);
                break;
            case PurchaseMode.Total:    // should never be this
            default:
                numberOfLevels = 1;
                break;
        }

        return numberOfLevels;
    }

    // UI Event
    public void Upgrade(InvestmentCanvasElement investmentCanvasElement)
    {
        var i = GetInvestmentView(investmentCanvasElement);

        float currentCost = GetCurrentCost(i);
        if (currency.Value > currentCost)
        {
            currency.Value -= currentCost;
            int levels = GetCurrentNumberOfLevels(i);
            i.investment.Level += (levels == 0 ? 1 : levels);

            UpdateUISingle(i);
        }
    }

    // UI event
    public void RunInvestment(InvestmentCanvasElement investmentCanvasElement)
    {
        var i = GetInvestment(investmentCanvasElement);
        if (i.Level > 0)
            i.Run();
    }

    // UI event
    public void Autorun(InvestmentCanvasElement investmentCanvasElement)
    {
        var i = GetInvestment(investmentCanvasElement);
        
        // Check if this investment has at least one level, it does not already have autorun, && player can afford it
        if (i.Level > 0 && !i.Autorun && currency.Value > i.AutorunCost)
        {
            // need to set autorun before deducting the cost to have validation properly disable button when purchase occurs.
            i.Autorun = true;
            currency.Value -= i.AutorunCost;
            i.Run();
        }
    }

    // Data event
    public void AddCurrency(float amount)
    {
        currency.Value += amount;
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    // Can be done with a Linq where statement
    public Investment GetInvestment(InvestmentCanvasElement investmentCanvasElement)
    {
        foreach (InvestmentView investmentView in investments)
        {
            if (investmentView.hooks.Equals(investmentCanvasElement))
                return investmentView.investment;
        }

        return null;
    }

    // Can be done with a Linq where statement
    public InvestmentCanvasElement GetInvestment(Investment investment)
    {
        foreach (InvestmentView investmentView in investments)
        {
            if (investmentView.investment.Equals(investment))
                return investmentView.hooks;
        }

        return null;
    }
    
    // Can be done with a Linq where statement
    public InvestmentView GetInvestmentView(InvestmentCanvasElement investmentCanvasElement)
    {
        foreach (InvestmentView investmentView in investments)
        {
            if (investmentView.hooks.Equals(investmentCanvasElement))
                return investmentView;
        }

        return null;
    }

    // Can be done with a Linq where statement
    public InvestmentView GetInvestmentView(Investment investment)
    {
        foreach (InvestmentView investmentView in investments)
        {
            if (investmentView.investment.Equals(investment))
                return investmentView;
        }

        return null;
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < data.investments.Length; i++)
        {
            if (data.investments[i].Running)
            {
                data.investments[i].CurrentDuration -= Time.deltaTime;
                GetInvestment(data.investments[i]).Progress = data.investments[i].Progress;
            }
        }
	}

    void SaveGame()
    {
        SaveData save = new SaveData();
        save.currency = currency.Value;

        save.investmentLevels = new int[investments.Length];
        save.autorunStatuses = new bool[investments.Length];

        for (int i = 0; i < investments.Length; i++)
        {
            save.investmentLevels[i] = investments[i].investment.Level;
            save.autorunStatuses[i] = investments[i].investment.Autorun;
        }

        DataManager.SaveJson("IncrementalSaveData.json", JsonUtility.ToJson(save, true));
    }

    void LoadGame()
    {
        string dataAsJson = DataManager.LoadJson("IncrementalSaveData.json");
        if (dataAsJson == "") return;
        SaveData save = JsonUtility.FromJson<SaveData>(dataAsJson);
        currency.Value = save.currency;
        for (int i = 0; i < save.investmentLevels.Length; i++)
        {
            investments[i].investment.Level = save.investmentLevels[i];
            investments[i].investment.Autorun = save.autorunStatuses[i];
            if (investments[i].investment.Autorun) investments[i].investment.Run();
        }
    }

    // button functionally to select which investment to buy autorun for
    public void Menu()
    {
        Debug.Log("Menu button pressed.");
        for (int i = 0; i < data.investments.Length; i++)
        {
            var investment = GetInvestment(data.investments[i]);
            investment.SwitchMode();
            // display autorun costs
            if (investment.AutoRunMode)
                investment.UpgradeCost = data.investments[i].AutorunCost;
            else
                investment.UpgradeCost = GetCurrentCost(GetInvestmentView(investment));
        }

        // validate ui
        ValidateInvestmentCosts();        
    }

    // button functionality to select the purchase mode. 
    public void TogglePurchaseMode()
    {
        // go to next mode, update ui text, update costs, validate costs, 
        int index = (int)purchaseMode;
        index++;
        purchaseMode = (PurchaseMode)index;

        Debug.Log("Togglng purchase mode to " + purchaseMode.ToString());
        if (purchaseMode == PurchaseMode.Total)
            purchaseMode = PurchaseMode.X1;

        purchaseModeText.text = purchaseMode.ToString();

        UpdateUI();
        ValidateInvestmentCosts();
    }
}

[System.Serializable]
public class IncrementalSystemModel
{
    public Investment[] investments;
}

[System.Serializable]
public class Investment
{
    // Serialized fields
    public string name;
    public string description;
    public float baseCost;
    public float basePayout;
    public float baseDuration;
    public float baseGrowthModifier;

    // Not Seriliazed beyond this point

    public delegate void TimerEvent(float payout);
    public TimerEvent OnTimerExpired;

    public float Cost { get { return baseCost * Mathf.Pow(baseGrowthModifier, level); } }
    public float Costx10 { get { return CostMultiplier(10); } }
    public float Costx100 { get { return CostMultiplier(100); } }
    //Todo figure out real auto run costs
    public float AutorunCost { get { return baseCost * 10; } }
    public float Payout { get { return basePayout * Mathf.Pow(baseGrowthModifier, level); } }
    public int Level { get { return level; } set { level = value; } }
    public bool Running { get { return running; } }
    public float CurrentDuration { get { return currentDuration; }
        set {
            currentDuration = Mathf.Clamp(value, 0, baseDuration);
            if (currentDuration == 0)
            {
                if(OnTimerExpired != null)
                {
                    OnTimerExpired(Payout);
                }

                currentDuration = baseDuration;

                if (!autorun)
                    running = false;
            }
        } }
    public float Progress { get { return 1 - currentDuration / baseDuration; } }
    public bool Autorun { get { return autorun; } set { autorun = value; } }

    private int level;
    private float currentDuration;
    private bool running;
    private bool autorun; 

    public void Upgrade()
    {
        Debug.Log("Investments:Upgrade: " + name);
        
    }

    public void Run()
    {
        Debug.Log("Investments:Run: " + name);
        if (!running) running = true;
    }

    public int MaxNumberOfLevels(float currency)
    {
        int maxNumberOfPurchasableLevels = 0;

        maxNumberOfPurchasableLevels =
            //Mathf.FloorToInt( Mathf.Log( ( ( (currency * (baseGrowthModifier - 1) ) / ( baseCost * Mathf.Pow(baseGrowthModifier, level) ) ) + 1 ) , baseGrowthModifier) );
            Mathf.FloorToInt(
                Mathf.Log( ( (Mathf.Pow(baseGrowthModifier, level) - ((currency * (1 - baseGrowthModifier)) / baseCost) ) ), baseGrowthModifier) - level);

        return maxNumberOfPurchasableLevels;
    }

    // cost of max mount given currency.
    public float MaxCost(float currency)
    {
        float maxCost = 0;

        maxCost = CostMultiplier(MaxNumberOfLevels(currency));

        return maxCost;
    }

    // 10x 100x ...
    private float CostMultiplier(int multiplier)
    {
        float multiCost = 0;

        multiCost = baseCost * ((Mathf.Pow(baseGrowthModifier, level) * (Mathf.Pow(baseGrowthModifier, multiplier) - 1)) / (baseGrowthModifier - 1));

        return multiCost;
    }

}

public class InvestmentView
{
    public Investment investment;
    public InvestmentCanvasElement hooks;

    public InvestmentView(Investment investment, InvestmentCanvasElement hooks)
    {
        this.investment = investment;
        this.hooks = hooks;
    }
}

[System.Serializable]
public class SaveData
{
    public float currency;
    public int[] investmentLevels;
    public bool[] autorunStatuses;
}