using UnityEngine;

public class TooltipUIManager : MonoBehaviour
{
    [Header("Tooltip UI Widget")]
    [SerializeField] private GameObject tooltipWidget;

    public void ShowTooltip()
    {
        if (tooltipWidget != null)
        {
            tooltipWidget.SetActive(true);
        }
    }

    public void HideTooltip()
    {
        if (tooltipWidget != null)
        {
            tooltipWidget.SetActive(false);
        }
    }

    private void Awake()
    {
        if (tooltipWidget != null)
        {
            tooltipWidget.SetActive(false); // Ensure it’s hidden by default
        }
    }
}
