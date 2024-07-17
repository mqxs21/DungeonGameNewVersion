using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class coinTrackerScript : MonoBehaviour
{
    public int coinAmount;
    public TextMeshProUGUI coinsText;
    void Update(){
        
        coinsText.text = coinAmount.ToString();

    }
}
