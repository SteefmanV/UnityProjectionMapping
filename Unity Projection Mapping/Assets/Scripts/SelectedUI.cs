using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedUI : MonoBehaviour
{
    [SerializeField] private HierachyManager _hierachyManager = null;

    [Header("Selected UI")]
    [SerializeField] private TMP_InputField _name = null;
    [SerializeField] private TextMeshProUGUI _topLeft = null;
    [SerializeField] private TextMeshProUGUI _topRight = null;
    [SerializeField] private TextMeshProUGUI _bottomLeft = null;
    [SerializeField] private TextMeshProUGUI _bottomRight = null;

    [SerializeField] private GameObject _materialSelector = null;
    [SerializeField] private Image _materialPreview = null;

    private ImageProjector _selectedImage = null;


    private void Awake()
    {
        _hierachyManager.SelectedImageChanged += OnSelectedObjectChanged;
    }


    private void Update()
    {
        if (_selectedImage != null) updateSelectionUI();
    }


    /// <summary>
    /// Changes the name of the Image Projector
    /// </summary>
    public void NameChanged(string pNewName)
    {
        _selectedImage.projectorName = pNewName;
        _hierachyManager.NameChanged(_selectedImage);
    }


    /// <summary>
    /// Changes the material of the Image Projector
    /// </summary>
    public void selectNewMatrial(Material pMat, Sprite pMaterialThumbnail)
    {
        _selectedImage.ChangeMaterial(pMat);
        _selectedImage.materialThumbnail = pMaterialThumbnail;
        _materialSelector.SetActive(false);
    }


    /// <summary>
    /// Updates the selected object
    /// </summary>
    private void OnSelectedObjectChanged(object sender, SelectedImageEventArgs e)
    {
        _selectedImage = e.imageProjector;
    }


    /// <summary>
    /// Toggles the material selection pandel
    /// </summary>
    public void ToggleMaterialSelection()
    {
        _materialSelector.SetActive(!_materialSelector.activeSelf);
    }


    /// <summary>
    /// Update the UI information of the selected Image Projector
    /// </summary>
    private void updateSelectionUI()
    {
        _name.text = _selectedImage.projectorName;

        _topLeft.text = Vec3String(_selectedImage.topLeft);
        _topRight.text = Vec3String(_selectedImage.topRight);
        _bottomLeft.text = Vec3String(_selectedImage.bottomLeft);
        _bottomRight.text = Vec3String(_selectedImage.bottomRight);

        _materialPreview.sprite = _selectedImage.materialThumbnail;
    }


    /// <summary>
    /// Converts a Vector3 to a nice formated string
    /// </summary>
    private string Vec3String(Vector3 pVec)
    {
        return pVec.x.ToString("0") + ", " + pVec.y.ToString("0");
    }
}
