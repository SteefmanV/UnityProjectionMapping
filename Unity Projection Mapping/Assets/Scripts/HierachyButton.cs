using UnityEngine;
using TMPro;

/// <summary>
/// Functionality for the ImageProjector button in the hierachy
/// </summary>
public class HierachyButton : MonoBehaviour
{
    public bool selected = false;

    [SerializeField] private TextMeshProUGUI _nameText = null;

    private HierachyManager _selectedManager = null;

    private void Awake()
    {
        _selectedManager = FindObjectOfType<HierachyManager>();
    }


    /// <summary>
    /// Select the Image Projector
    /// </summary>
    public void SelectImage()
    {
        if (selected)
        {
            _selectedManager.DeselectCurrentProjector();
            FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }
        else
        {
            _selectedManager.SelectObject(gameObject);
        }

        selected = !selected;
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
