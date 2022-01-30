using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeerNumber : MonoBehaviour
{
    TextMeshProUGUI MyText;
    void Start()
    {
        MyText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        MyText.text = "Number of living deer: " + Entities.EntityMap.deer.Count;
    }
}
