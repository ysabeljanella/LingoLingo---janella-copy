using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMatchForm : MonoBehaviour
{
    static ObjectMatchForm Instance;

    public MatchScriptableObject MatchScriptableObject;
    public Transform Left;
    public Transform Right;
    public Transform gameObjectcontainer;
    public GameObject levelCompleteUI;
    public TextMeshProUGUI timer;
    public float gameDuration;
    public int module;
    public int challengeIndex;
    private int points = 0;
    private int maxPoints = 0;
    // Start is called before the first frame update
    private List<MatchItem> matches = new List<MatchItem>();
    private List<MatchItem> rightItems = new List<MatchItem>();
    public int itemsCount;
    private int coinReward;


    void Start()
    {
        
        Instance = this;
        LoadItems();
        GameTimerScript.instance.FinishGameEvent += FinalizeGame;
        GameTimerScript.instance.StartTimer(gameDuration);
        GameTimerScript.instance.timerText = timer;
        coinReward = 0;
    }
    private void LoadItems()
    {
        int intName = 0;
        foreach(string left in MatchScriptableObject.Lefts)
        {
            if(intName < itemsCount)
            {
                intName++;
                GameObject item = Instantiate(MatchScriptableObject.leftGameObject, Left);

                item.GetComponentInChildren<TextMeshProUGUI>().text = left;
                MatchItem leftitem = item.GetComponent<MatchItem>();

                leftitem.itemName = intName.ToString();
                matches.Add(leftitem);
            }         
        }
        maxPoints = intName;
        intName = 0;
        
        int audioIndex = 0;
        int imageIndex = 0;
        foreach (string right in MatchScriptableObject.Rights)
        {
            if(intName < itemsCount)
            {

                intName++;
                GameObject item = Instantiate(MatchScriptableObject.rightGameObject, Right);
                MatchItem rightItem = item.GetComponent<MatchItem>();
                item.GetComponentInChildren<TextMeshProUGUI>().text = right;
                rightItem.itemName = intName.ToString();

                if(MatchScriptableObject.audioClips.Count > 0)
                {
                    item.GetComponentInChildren<PlayAudio>().consVowsClip = MatchScriptableObject.audioClips[audioIndex];
                    audioIndex++;
                }

                if(MatchScriptableObject.ImageRight.Length > 0)
                {
                    item.GetComponentInChildren<Image>().sprite = MatchScriptableObject.ImageRight[imageIndex];
                    imageIndex++;
                }

                rightItems.Add(rightItem);
            }
        }


        RandomizeOrder();
    }

    public void FinalizeGame()
    {
        GameTimerScript.instance.StopTimer();

        points = 0;
        if (matches.Count > 0)
        {
            foreach (MatchItem item in matches)
            {
                if (!string.IsNullOrEmpty(item.itemName) && item.answer == item.itemName)
                {
                    points++;
                    coinReward += 10;
                }
            }
        }

        SetResult();
    }

    public void RandomizeOrder()
    {
        // Using Fisher-Yates shuffle algorithm
        for (int i = matches.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            MatchItem temp = matches[i];
            matches[i] = matches[randomIndex];
            matches[randomIndex] = temp;
        }

        // Reposition the items according to the new order
        for (int i = 0; i < matches.Count; i++)
        {
            matches[i].transform.SetSiblingIndex(i);
        }

        // Using Fisher-Yates shuffle algorithm
        for (int i = rightItems.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            MatchItem temp = rightItems[i];
            rightItems[i] = matches[randomIndex];
            rightItems[randomIndex] = temp;
        }

        // Reposition the items according to the new order
        for (int i = 0; i < rightItems.Count; i++)
        {
            rightItems[i].transform.SetSiblingIndex(i);
        }
    }
 
    public void SetResult()
    {
        Debug.Log($"Result: {points}/{maxPoints}");
        GameObject result = Instantiate(levelCompleteUI,gameObjectcontainer);
        result.GetComponentInChildren<TextMeshProUGUI>().text = $"Your score {points}/{maxPoints}\n+{coinReward} coins";
        PlayerDataHandler.instance.SetModuleValue(module, challengeIndex, points);
        PlayerDataHandler.instance.CoinReward(coinReward);
        StartCoroutine(CloseThisGame());
    }


    private IEnumerator CloseThisGame()
    {       
        yield return new WaitForSeconds(3f);
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameTimerScript.instance.FinishGameEvent -= FinalizeGame;
    }
}