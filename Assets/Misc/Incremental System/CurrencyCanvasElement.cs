using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CurrencyCanvasElement : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI currencyText;

    public float Currency { set { currencyText.text = "$" + value.ToString(); } }
}
