using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDateSet : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timeText = null;
    [SerializeField]
    TextMeshProUGUI dateText = null;

    void Update()
    {
        timeText.text = System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes;
        dateText.text = System.DateTime.Now.ToLongDateString().ToString();
    }
}
