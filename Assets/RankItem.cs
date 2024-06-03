using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankItem : MonoBehaviour
{
	public TMP_Text rankText;
	public TMP_Text playerNameText;
	public TMP_Text recordText;
	
    public void UpdateData(int rank, string playerName, float record)
    {
        rankText.text = rank.ToString();
		playerNameText.text = playerName;
		recordText.text = record.ToString("F1");
    }
}
