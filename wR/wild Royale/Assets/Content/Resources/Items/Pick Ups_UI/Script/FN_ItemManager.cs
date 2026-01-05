using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FN_ItemManager : MonoBehaviour
{
    //Reference
    private InputManager _inputManager;

    // Adding Multiplayer Network Connectivity
    private PhotonView photonView;

    [Header("---Pick Up Item Tool Tip Parameters---")]
    public string ItemName;
    public string ItemType;
    public enum ItemRarityEnum { Common, Uncommon, Rare, Epic, Legendary, Mythic }
    public ItemRarityEnum ItemRarity;

    public string ItemAmmount;
    public string PickUpButtonText;

    [Header("--Setup Parameters---")]
    public GameObject ToolTipWidget;
    public RawImage ItemBackground;

    private TextMeshProUGUI[] TMPTexts;
    private TextMeshProUGUI PickUpButton;
    private TextMeshProUGUI TMP_ItemType;
    private TextMeshProUGUI TMP_ItemName;
    private TextMeshProUGUI TMP_ItemRarity;
    private TextMeshProUGUI TMP_ItemAmount;

    // NEW ================================================================
    private GameObject _player;
    private bool inRange = false;
    private GameObject ItemHolder;
    private Animator anim;
    private GameObject _GameManager;
    private InventoryManager _IM;
    private Rigidbody rb;
    private BoxCollider itemHolderCollider;
    // ====================================================================

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Get a handle to the GameManager (PlayerDummy) and InventoryManager
        _GameManager = GameObject.Find("PlayerDummy");
        _IM = _GameManager.GetComponent<InventoryManager>();

        // Get the ItemHolder
        var IHTrans = this.gameObject.transform.GetChild(1);
        ItemHolder = IHTrans.gameObject;

        // Get the ItemHolder's Animator
        anim = ItemHolder.GetComponent<Animator>();

        // Cache the Rigidbody and ItemHolder's BoxCollider
        rb = GetComponent<Rigidbody>();
        itemHolderCollider = ItemHolder.GetComponent<BoxCollider>();

        // Setup UI
        TMPTexts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var tmp in TMPTexts)
        {
            switch (tmp.name)
            {
                case "_PickUpBtnText":
                    PickUpButton = tmp;
                    PickUpButton.text = PickUpButtonText;
                    PickUpButtonText = PickUpButtonText.ToLower();
                    break;
                case "_txtType":
                    TMP_ItemType = tmp;
                    TMP_ItemType.text = ItemType;
                    break;
                case "_txtItemName":
                    TMP_ItemName = tmp;
                    TMP_ItemName.text = ItemName;
                    break;
                case "_txtRarity":
                    TMP_ItemRarity = tmp;
                    TMP_ItemRarity.text = ItemRarity.ToString();
                    break;
                case "_txtAmount":
                    TMP_ItemAmount = tmp;
                    TMP_ItemAmount.text = ItemAmmount;
                    break;
            }
        }

        // Use RarityManager to get the color
        if (RarityManager.Instance != null)
        {
            ItemBackground.color = RarityManager.Instance.GetColor(ItemRarity.ToString());
        }
        else
        {
            Debug.LogWarning("RarityManager instance not found!");
        }

        ToolTipWidget.SetActive(false);
    }

    void Update()
    {
        if (inRange && _GameManager.GetComponent<InputManager>().actionInput)
        {
            _GameManager.GetComponent<InputManager>().actionInput = false;

            // Request ownership if not owned by us (Photon)
            if (!photonView.IsMine)
            {
                photonView.RequestOwnership();
            }

            // Locally disable Rigidbody and BoxCollider on ItemHolder for smooth pickup animation
            photonView.RPC("RPC_DisablePhysicsAndCollider", RpcTarget.AllBuffered);

            // Play animation locally
            anim.SetTrigger("IsPickingUp?");

            // Inform all players to destroy the item after pickup
            photonView.RPC("RPC_PickupItem", RpcTarget.AllBuffered);

            // Add item to local player's inventory
            _IM.AddToInventory();
        }
    }

    [PunRPC]
    void RPC_DisablePhysicsAndCollider()
    {
        if (rb != null)
            rb.isKinematic = true;

        if (itemHolderCollider != null)
            itemHolderCollider.enabled = false;
    }

    [PunRPC]
    void RPC_PickupItem()
    {
        ToolTipWidget.SetActive(false);

        // Disable the collider on the item itself to prevent repeated triggers
        var col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Play animation locally (for non-owners)
        anim.SetTrigger("IsPickingUp?");

        // Destroy item after animation plays
        Destroy(gameObject, 1.0f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            ToolTipWidget.SetActive(true);
            _player = col.gameObject;
            _inputManager = _player.GetComponent<InputManager>();
            inRange = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            ToolTipWidget.SetActive(false);
            inRange = false;
        }
    }
}
