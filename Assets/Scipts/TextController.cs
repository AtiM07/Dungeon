using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] compassTxt = new TextMeshProUGUI[4];

    string GetString(Side side) => $"{side.direction}: \n {side.forward} \n {side.left} \n {side.left}";

    public void GetText(Path path)
    {
        for (int i = 0; i < compassTxt.Length; i++)
        {
            compassTxt[i].text = GetString(path.side[i]);
        }
    }
}