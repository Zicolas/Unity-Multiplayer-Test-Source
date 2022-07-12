using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;

public class CharacterCosmeticsControllers : MonoBehaviour
{
    public int CurrentColorIndex = 0;
    public Material[] PlayerColors;
    public Image CurrentColorImage;
    public TMP_Text CurrentColorText;

    private void Start()
    {
        CurrentColorIndex = PlayerPrefs.GetInt("CurrentColorIndex", 0);
        CurrentColorImage.color = PlayerColors[CurrentColorIndex].color;
        CurrentColorText.text = PlayerColors[CurrentColorIndex].name;
    }

    public void NextColor()
    {
        if(CurrentColorIndex < PlayerColors.Length - 1)
        {
            CurrentColorIndex++;
            PlayerPrefs.SetInt("CurrentColorIndex", CurrentColorIndex);
            CurrentColorImage.color = PlayerColors[CurrentColorIndex].color;
            CurrentColorText.text = PlayerColors[CurrentColorIndex].name;
        }
    }

    public void PreviousColor()
    {
        if(CurrentColorIndex > 0)
        {
            CurrentColorIndex--;
            PlayerPrefs.SetInt("CurrentColorIndex", CurrentColorIndex);
            CurrentColorImage.color = PlayerColors[CurrentColorIndex].color;
            CurrentColorText.text = PlayerColors[CurrentColorIndex].name;
        }
    }
}
