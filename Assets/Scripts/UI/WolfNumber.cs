using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace UI{
    public class WolfNumber : MonoBehaviour
    {
        TextMeshProUGUI MyText;
        void Start()
        {
            MyText = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            MyText.text = "Number of living wolfs: " + Entities.EntityMap.wolfs.Count;
        }
    }
}
