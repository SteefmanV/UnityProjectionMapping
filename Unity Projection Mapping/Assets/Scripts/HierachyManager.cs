using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HierachyManager : MonoBehaviour
{
    public event EventHandler<SelectedImageEventArgs> SelectedImageChanged;

    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private GameObject _hierachyGroup = null;

    [Header("Prefabs")]
    [SerializeField] private GameObject _imageProjectorPrefab = null;
    [SerializeField] private GameObject _imageButtonPrefab = null;

    private Dictionary<GameObject, ImageProjector> _imageObjects = new Dictionary<GameObject, ImageProjector>();
    private ImageProjector _selectedImage = null;
    private Button _selectedButton = null;


    private void Update()
    {
        handleMouseSelection();
    }


    /// <summary>
    /// Creates a new Image Projector
    /// </summary>
    public void CreateNewImage()
    {
        GameObject newImage = Instantiate(_imageProjectorPrefab);
        GameObject newButton = Instantiate(_imageButtonPrefab, _hierachyGroup.transform);
        _imageObjects.Add(newButton, newImage.GetComponent<ImageProjector>());
        newButton.GetComponent<HierachyButton>().SetName(newImage.GetComponent<ImageProjector>().projectorName);
        SelectObject(newButton);
    }


    /// <summary>
    /// Selects a Image Projector
    /// </summary>
    public void SelectObject(GameObject pImageButton)
    {
        DeselectCurrentProjector();

        _selectedImage = _imageObjects[pImageButton];
        _selectedButton = pImageButton.GetComponent<Button>();
        _selectedImage.ToggleSelected(true);
        _uiManager.ToggleSelectionPanel(true);

        _selectedButton.GetComponent<Image>().color = new Color(0.55f, 0.274f, 0.73f);
        SelectedImageChanged?.Invoke(this, new SelectedImageEventArgs(_selectedImage));
    }


    /// <summary>
    /// Selects a Image Projector
    /// </summary>
    public void SelectObject(ImageProjector pImageProjector)
    {
        DeselectCurrentProjector();

        _selectedImage = pImageProjector;
        _selectedButton = _imageObjects.FirstOrDefault(x => x.Value == pImageProjector).Key.GetComponent<Button>();
        _selectedImage.ToggleSelected(true);
        _uiManager.ToggleSelectionPanel(true);

        _selectedButton.GetComponent<Image>().color = new Color(0.55f, 0.274f, 0.73f);
        SelectedImageChanged?.Invoke(this, new SelectedImageEventArgs(_selectedImage));
    }


    /// <summary>
    /// Deslect all Image Projector
    /// </summary>
    public void DeselectCurrentProjector()
    {
        if (_selectedImage != null) _selectedImage.ToggleSelected(false);
        if (_selectedButton != null)
        {
            _selectedButton.GetComponent<HierachyButton>().selected = false;
            _selectedButton.GetComponent<Image>().color = Color.white;
        }

        _selectedButton = null;
        _selectedImage = null;
        _uiManager.ToggleSelectionPanel(false);
    }


    /// <summary>
    /// Delete Image Projector
    /// </summary>
    public void DeleteObject(GameObject pImageButton)
    {
        Destroy(_imageObjects[pImageButton].gameObject);
        _imageObjects.Remove(pImageButton);
        Destroy(pImageButton);

        _uiManager.ToggleSelectionPanel(false);
    }


    /// <summary>
    /// Update name in Hierachy button
    /// </summary>
    public void NameChanged(ImageProjector pImageProjector)
    {
        _selectedButton = _imageObjects.FirstOrDefault(x => x.Value == pImageProjector).Key.GetComponent<Button>();
        _selectedButton.GetComponent<HierachyButton>().SetName(pImageProjector.projectorName);
    }


    /// <summary>
    /// Select a object by clicking on it
    /// </summary>
    private void handleMouseSelection()
    {
        if (!_uiManager.IsDrawing() && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                ImageProjector image = hit.collider.gameObject.GetComponent<ImageProjector>();
                if (image != null)
                {
                    SelectObject(image);
                }
            }
        }
    }
}
