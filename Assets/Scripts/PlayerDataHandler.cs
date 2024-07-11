using UnityEngine;
using System.IO;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler instance;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI shopCoinsText;
    public GameObject playerPos;
    private GameObject player;
    public Module[] modules;
    public GameObject[] CharacterModels;
    public int[] ChallengeMaxScore;
    public int characterModelIndex;
    public int coins;
    private string dataFilePath;


    [Header("CHARACTER SHOP")]
    public Button previousButton;
    public Button nextButton;
    public GameObject lockedIndicatorPrefab;
    public Button buyButton;
    public GameObject priceDisplay;
    public int[] unlockedCharacters;
    

    private void Awake()
    {
        instance = this;
        dataFilePath = Path.Combine(Application.persistentDataPath, "player_data.json");
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found in the scene.");
        }
    }
    public void SetCoins()
    {
        coinsText.text = coins.ToString();
        shopCoinsText.text = coins.ToString();

    }

    public void CoinReward(int coins)
    {
        this.coins += coins;
        SetCoins();
    }

    public void SetModuleValue(int moduleIndex, int challengeIndex, int challengeScore)
    {
        int currentScore = modules[moduleIndex].challenges[challengeIndex];

        if (challengeScore > currentScore)
        {
            modules[moduleIndex].challenges[challengeIndex] = challengeScore;
        }
        else
        {
            Debug.LogError("Score is not higher.");
        }

        SaveModuleChallengesToJson();
    }

    public void SetModuleConstantValue(int module)
    {
        modules[module].challenges[0] = 8;
        DialogueLua.SetVariable("ModuleFourPercentage", 50);
        SaveModuleChallengesToJson();
    }
    // Save all module_challenge arrays to a JSON file in the persistent data path
    public void SaveModuleChallengesToJson()
    {
        PlayerData data = new PlayerData
        {
            playerPos = this.playerPos.transform.position,
            characterModelIndex = this.characterModelIndex,
            coins = this.coins,
            modules = this.modules,
            unlockedCharacterIndex = this.unlockedCharacters
        };

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(dataFilePath, jsonData);
        Debug.Log("Module challenges saved to: " + dataFilePath);
    }

    // Load module_challenge arrays from a JSON file in the persistent data path
    public void LoadModuleChallengesFromJson()
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);

            // Assign loaded values to module_challenge arrays
            this.playerPos.transform.position = data.playerPos;
            this.characterModelIndex = data.characterModelIndex;
            this.coins = data.coins;
            this.modules = data.modules;
            this.unlockedCharacters = data.unlockedCharacterIndex;

            SetCharacterModelIndex(this.characterModelIndex);
            Debug.Log("Module challenges loaded from: " + dataFilePath);
            SetCoins();
        }
        else
        {
            CreateNewSaveFile();
        }

        LoadAllModulesPercentage();
    }


    private void CreateNewSaveFile()
    {
        // Initialize modules array with default challenges
        modules = new Module[6]; // Change numberOfModules to the actual number of modules
        unlockedCharacters = new int[1];
        for (int i = 0; i < modules.Length; i++)
        {
            modules[i] = new Module
            {
                Id = i,
                challenges = new int[3] // Change numberOfChallenges to the actual number of challenges per module
            };
            for (int j = 0; j < modules[i].challenges.Length; j++)
            {
                modules[i].challenges[j] = 0; // Set all challenges to 0 by default
            }
        }

        // Create new PlayerData instance with default values
        PlayerData newData = new PlayerData
        {
            characterModelIndex = 0,
            coins = 0,
            modules = modules,
            unlockedCharacterIndex = unlockedCharacters,
            playerPos = playerPos.transform.position,
        };
        SetCoins();
        SetCharacterModelIndex(0);
        // Serialize and save the new PlayerData instance to JSON
        string jsonData = JsonUtility.ToJson(newData);
        File.WriteAllText(dataFilePath, jsonData);

        Debug.Log("New save file created with default values at: " + dataFilePath);
    }

    public void CharacterScroll()
    {
        

        if(characterModelIndex <= CharacterModels.Length)
        {
            characterModelIndex++;

            if (characterModelIndex == CharacterModels.Length)
            {
                characterModelIndex = 0;
            }
            SetCharacterModelIndex(characterModelIndex);
            SaveModuleChallengesToJson();
        }
    }
    public void SetCharacterModelIndex(int i)
    {
        foreach(GameObject obj in CharacterModels)
        {
            obj.SetActive(false);
        }

        CharacterModels[i].SetActive(true);
    }

    public void GetModulePercentage(int moduleIndex)
    {
        int sum = modules[moduleIndex].challenges[0] + modules[moduleIndex].challenges[1] + modules[moduleIndex].challenges[2];
        int maxScore = PlayerDataHandler.instance.ChallengeMaxScore[moduleIndex];
        int percentage = Mathf.RoundToInt((float)sum / maxScore * 100);

        switch (moduleIndex)
        {
            case 0:
                DialogueLua.SetVariable("ModuleOnePercentage", percentage);
                break;
            case 1:
                DialogueLua.SetVariable("ModuleTwoPercentage", percentage);
                break;
            case 2:
                DialogueLua.SetVariable("ModuleThreePercentage", percentage);
                break;
            case 3:
                DialogueLua.SetVariable("ModuleFourPercentage", percentage);
                break;
            case 4:
                DialogueLua.SetVariable("ModuleFivePercentage", percentage);
                break;
            case 5:
                DialogueLua.SetVariable("ModuleSixPercentage", percentage);
                break;
        }      
    }

    public void LoadAllModulesPercentage()
    {
        int index = 0;
        foreach (var module in modules)
        {
            GetModulePercentage(index);
            index++;
        }
    }

    public void ShopNext()
    {
        if(characterModelIndex < CharacterModels.Length - 1)
        {
            characterModelIndex++;
            SetCharacterModelIndex(this.characterModelIndex);

            bool isUnlocked = unlockedCharacters.Any(x => x.Equals(characterModelIndex));

            if (!isUnlocked) // IF CHARACTER NOT BOUGHT YET
            {
                lockedIndicatorPrefab.SetActive(true);
                buyButton.interactable = true;
                priceDisplay.gameObject.SetActive(true);
            }
            else // IF CHARACTER OWNED
            {
                lockedIndicatorPrefab.SetActive(false);
                buyButton.interactable = false;
                priceDisplay.gameObject.SetActive(false);
                SaveModuleChallengesToJson();
            }
        }
        
    }

    public void ShopPrevious()
    {
        if (characterModelIndex > 0)
        {
            characterModelIndex--;
            SetCharacterModelIndex(this.characterModelIndex);

            bool isUnlocked = unlockedCharacters.Any(x => x.Equals(characterModelIndex));
            if (!isUnlocked)
            {
                lockedIndicatorPrefab.SetActive(true);
                priceDisplay.gameObject.SetActive(true);
                buyButton.interactable = true;
            }
            else
            {
                lockedIndicatorPrefab.SetActive(false);
                buyButton.interactable = false;
                priceDisplay.gameObject.SetActive(false);
                SaveModuleChallengesToJson();
            }
        }

    }

    public void CloseShop()
    {
        bool isUnlocked = unlockedCharacters.Any(x => x.Equals(characterModelIndex));
        if (isUnlocked)
        {
            SaveModuleChallengesToJson();
        }
        else
        {
            LoadModuleChallengesFromJson();
        }

        lockedIndicatorPrefab.SetActive(false);
        priceDisplay.gameObject.SetActive(false);
        buyButton.interactable = false;
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
    }

    public void BuyCharacter(int characterIndex, int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;

            // Check if the character is already unlocked
            if (!unlockedCharacters.Contains(characterIndex))
            {
                // Create a new array with increased size
                int[] newUnlockedCharacters = new int[unlockedCharacters.Length + 1];

                // Copy existing unlocked characters to the new array
                for (int i = 0; i < unlockedCharacters.Length; i++)
                {
                    newUnlockedCharacters[i] = unlockedCharacters[i];
                }

                // Add the newly unlocked character to the array
                newUnlockedCharacters[unlockedCharacters.Length] = characterIndex;

                // Update the reference to the new array
                unlockedCharacters = newUnlockedCharacters;

                // Update UI or perform any other necessary actions
                // For example, update the UI to reflect the newly unlocked character
                priceDisplay.gameObject.SetActive(false);
                buyButton.interactable = false;  
                // Save changes to JSON
                SaveModuleChallengesToJson();
                SetCoins();
            }
            else
            {
                Debug.Log("Character already unlocked.");
            }
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void PurchaseCharacter()
    {
        int characterIndex = characterModelIndex;
        int cost = 500;

        BuyCharacter(characterIndex, cost);
    }


    [System.Serializable]
    public class PlayerData
    {
        public Vector3 playerPos; 
        public int characterModelIndex;
        public int coins;

        public Module[] modules;
        public int[] unlockedCharacterIndex;
    }

    [System.Serializable]
    public class Module
    {
        public int Id;

        public int[] challenges;
    }
}
