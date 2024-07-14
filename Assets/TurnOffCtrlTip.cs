
using UnityEngine;

public class TurnOffCtrlTip : MonoBehaviour
{
    void OnTriggerEnter(){
        GameObject.Find("ctrlTi").SetActive(false);
}
}
