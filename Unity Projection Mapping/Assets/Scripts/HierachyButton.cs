using UnityEngine;
using TMPro;

/// <summary>
/// Functionality for the ImageProjector button in the hierachy
/// </summary>
public class HierachyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    private SelectedManager _selectedManager = null;

    private void Awake()
    {
        _selectedManager = FindObjectOfType<SelectedManager>();
    }

    /// <summary>
    /// Select the Image Projector
    /// </summary>
    public void SelectImage()
    {
        _selectedManager.SelectObject(gameObject);
    }

    /// <summary>
    /// Delete Image projector from the view
    /// </summary>
    public void DeleteImage()
    {
        _selectedManager.DeleteObject(gameObject);
    }

    /// <summary>
    /// Change Image Projector name
    /// </summary>
    public void SetName(string pName)
    {
        _nameText.text = pName;
    }
}
