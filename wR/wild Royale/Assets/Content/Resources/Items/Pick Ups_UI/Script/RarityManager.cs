using UnityEngine;

public class RarityManager : MonoBehaviour
{
    // Singleton instance
    public static RarityManager Instance { get; private set; }

    [Header("---Rarity Colors---")]
    public Color Common;
    public Color Uncommon;
    public Color Rare;
    public Color Epic;
    public Color Legendary;
    public Color Mythic;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }

    // Helper method to get the color by rarity name
    public Color GetColor(string rarity)
    {
        switch (rarity)
        {
            case "Common": return Common;
            case "Uncommon": return Uncommon;
            case "Rare": return Rare;
            case "Epic": return Epic;
            case "Legendary": return Legendary;
            case "Mythic": return Mythic;
            default: return Color.white;
        }
    }
}
