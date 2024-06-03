using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TOS.Tool;

public class RaceResult : MonoBehaviour
{
	public GameObject itemPrefab;
	public RectTransform itemParent;
	public RankItem playerItem;
		
	public string stageName;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadLeaderBoard()
    {
        string leaderboardKey = String.Format("Leaderboard_{0}", stageName);
		string leaderboardStr = PlayerPrefs.GetString( leaderboardKey, String.Empty);	
		RaceLeaderboard leaderboard = RaceLeaderboard.CreateFromJSON(leaderboardStr);
		
		int playerRank = PlayerPrefs.GetInt( String.Format("RaceResultRanking_{0}", stageName), 0);
		
		for(int i=0; i<leaderboard.recordList.Count; i++)
		{
			if(i==playerRank)
			{
				playerPrefab.transform.SetAsLastSibling();
				playerItem.UpdateData(i, leaderboard.recordList[i].playerName, leaderboard.recordList[i].raceRecord);
			}
			else
			{
				GameObject itemObj = Instantiate<GameObject>(itemPrefab, itemParent);
				
				RankItem rankItem = itemObj.GetComponent<RankItem>();
				rankItem.UpdateData(i, leaderboard.recordList[i].playerName, leaderboard.recordList[i].raceRecord);
				
				itemObj.SetActive(true);
			}
		}
		
		//Scroll to Player rank
    }
}
