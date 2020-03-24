using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedManager : MonoBehaviour
{
    [SerializeField] private GameObject _imageProjectorPrefab = null;
    [SerializeField] private GameObject _imageButtonPrefab = null;
    [SerializeField] private MaskDrawer _maskDrawer;

    [Header("Selected UI")]
    [SerializeField] private GameObject _selectedPanel = null;
    [SerializeField] private TextMeshProUGUI _topLeft = null;
    [SerializeField] private TextMeshProUGUI _topRight = null;
    [SerializeField] private TextMeshProUGUI _bottomLeft = null;
    [SerializeField] private TextMeshProUGUI _bottomRight = null;

    private Dictionary<GameObject, ImageProjector> _imageObjects = new Dictionary<GameObject, ImageProjector>();
    private ImageProjector _selectedImage = null;
    private Button _selectedButton = null;
    private int imageID = 0; // todo: replace this with ectual image names (in the Image projector)


    private void Update()
    {
        if (_selectedImage != null)
        {
            updateSelectionUI();
            _selectedButton.Select();
        }
        handleMouseSelection();
    }


    /// <summary>
    /// Creates a new Image Projector
    /// </summary>
    public void CreateNewImage()
    {
        imageID += 1;
        GameObject newImage = Instantiate(_imageProjectorPrefab);
        GameObject newButton = Instantiate(_imageButtonPrefab, transform);
        _imageObjects.Add(newButton, newImage.GetComponent<ImageProjector>());
        newButton.GetComponent<HierachyButton>().SetName(imageID + ".");
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
        updateSelectionUI();
        pImageButton.GetComponent<Button>().Select();
        _maskDrawer.currentState = MaskDrawer.DrawState.off;
        _selectedPanel.SetActive(true);
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
        updateSelectionUI();
        _selectedButton.Select();
        _maskDrawer.currentState = MaskDrawer.DrawState.off;
        _selectedPanel.SetActive(true);
    }


    /// <summary>
    /// Deslect all Image Projector
    /// </summary>
    public void DeselectCurrentProjector()
    {
        if(_selectedImage != null) _selectedImage.ToggleSelected(false);

        _selectedButton = null;
        _selectedImage = null;
        _selectedPanel.SetActive(false);
    }


    /// <summary>
    /// Delete Image Projector
    /// </summary>
    public void DeleteObject(GameObject pImageButton)
    {
        Destroy(_imageObjects[pImageButton].gameObject);
        _imageObjects.Remove(pImageButton);
        Destroy(pImageButton);

        _selectedPanel.SetActive(false);
    }


    /// <summary>
    /// Select a object by clicking on it
    /// </summary>
    private void handleMouseSelection()
    {
        if (_maskDrawer.currentState == MaskDrawer.DrawState.off && Input.GetMouseButtonDown(0))
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


    /// <summary>
    /// Update the UI information of the selected Image Projector
    /// </summary>
    private void updateSelectionUI()
    {
        _topLeft.text = Vec3String(_selectedImage.topLeft);
        _topRight.text = Vec3String(_selectedImage.topRight);
        _bottomLeft.text = Vec3String(_selectedImage.bottomLeft);
        _bottomRight.text = Vec3String(_selectedImage.bottomRight);
    }


    /// <summary>
    /// Converts a Vector3 to a nice formated string
    /// </summary>
    private string Vec3String(Vector3 pVec)
    {
        return pVec.x.ToString("0") + ", " + pVec.y.ToString("0");
    }
}
