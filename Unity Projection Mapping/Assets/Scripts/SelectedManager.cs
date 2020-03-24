using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedManager : MonoBehaviour
{
    [SerializeField] private GameObject _imageProjectorPrefab = null;
    [SerializeField] private GameObject _imageButtonPrefab = null;

    private Dictionary<GameObject, ImageProjector> _imageObjects = new Dictionary<GameObject, ImageProjector>();
    private ImageProjector _selectedImage = null;
    private Button _selectedButton = null;

    [SerializeField] private TextMeshProUGUI _topLeft = null;
    [SerializeField] private TextMeshProUGUI _topRight = null;
    [SerializeField] private TextMeshProUGUI _bottomLeft = null;
    [SerializeField] private TextMeshProUGUI _bottomRight = null;

    int i = 0;

    private void Update()
    {
        if (_selectedImage == null) return;
        updateSelectionUI();
        _selectedButton.Select();
    }


    public void CreateNewImage()
    {
        i += 1;
        GameObject newImage = Instantiate(_imageProjectorPrefab);
        GameObject newButton = Instantiate(_imageButtonPrefab, transform);
        _imageObjects.Add(newButton, newImage.GetComponent<ImageProjector>());
        newButton.GetComponent<HierachyButton>().SetName(i + ".");
    }


    public void SelectObject(GameObject pImageButton)
    {
        foreach(ImageProjector image in _imageObjects.Values)
        {
            image.SetSelected(false);
        }

        _selectedImage = _imageObjects[pImageButton];
        _selectedButton = pImageButton.GetComponent<Button>();

        _selectedImage.SetSelected(true);
        updateSelectionUI();
        pImageButton.GetComponent<Button>().Select();
    }


    public void DeleteObject(GameObject pImageButton)
    {
        Destroy(_imageObjects[pImageButton].gameObject);
        _imageObjects.Remove(pImageButton);
        Destroy(pImageButton);
    }

    private void updateSelectionUI()
    {
        _topLeft.text = Vec3String(_selectedImage.topLeft);
        _topRight.text = Vec3String(_selectedImage.topRight);
        _bottomLeft.text = Vec3String(_selectedImage.bottomLeft);
        _bottomRight.text = Vec3String(_selectedImage.bottomRight);
    }

    private string Vec3String(Vector3 pVec)
    {
        return pVec.x.ToString("0") + ", " + pVec.y.ToString("0");
    }
}
