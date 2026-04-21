using TMPro;
using UnityEngine;

public class UI_Coin_Panel : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public PlayerHealth playerCoins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCoins.OnCoinChanged += OnCoinChanged;
    }

    public void OnCoinChanged(float addcoin)
    {
        //Debug.Log("On Coins Changed");
        coinText.text = addcoin.ToString();
    }
}
