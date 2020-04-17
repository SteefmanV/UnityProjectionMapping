using UnityEngine;
using UnityEngine.Video;
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
        _hierachyManager.SelectedImageChanged += onSelectedObjectChanged;
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
    public void SelectNewMatrial(Material pMat, Sprite pMaterialThumbnail)
    {
        _selectedImage.ChangeMaterial(pMat);
        _selectedImage.materialThumbnail = pMaterialThumbnail;
        _materialSelector.SetActive(false);
    }


    /// <summary>
    /// Change the Image Projector into a video
    /// </summary>
    /// <param name="pURL"></param>
    public void SelectNewVideo(string pURL)
    {
        _selectedImage.ChangeMaterial(new Material(Shader.Find("HDRP/Lit"))); 

        VideoPlayer player = _selectedImage.SetVideo(pURL);
        player.sendFrameReadyEvents = true;
        player.frameReady += OnFrameReady;

        _materialSelector.SetActive(false);
    }


    /// <summary>
    /// Get the frame from a VideoPlayer and create a thumbnail
    /// </summary>
    /// <remarks> 
    /// This is only used for the first frame, after that we unsubscribe and disable the event
    /// The frameReady event is really heavy on the cpu. 
    /// </remarks>
    private void OnFrameReady(VideoPlayer psource, long index)
    {
        Texture videoTexture = psource.texture;
        Texture2D thumbnail = new Texture2D(videoTexture.width, videoTexture.height, TextureFormat.RGBA32, false);

        RenderTexture renderTexture = new RenderTexture(videoTexture.width, videoTexture.height, 32);
        Graphics.Blit(videoTexture, renderTexture);
        RenderTexture.active = renderTexture;

        thumbnail.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        thumbnail.Apply();
        RenderTexture.active = null;

        Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, psource.width, psource.height), new Vector2(0.5f, 0.5f));
        _selectedImage.materialThumbnail = sprite;
        psource.frameReady -= OnFrameReady;
        psource.sendFrameReadyEvents = false;
    }


    /// <summary>
    /// Toggles the material selection pandel
    /// </summary>
    public void ToggleMaterialSelection()
    {
        _materialSelector.SetActive(!_materialSelector.activeSelf);
    }


    /// <summary>
    /// Updates the selected object
    /// </summary>
    private void onSelectedObjectChanged(object sender, SelectedImageEventArgs e)
    {
        _selectedImage = e.imageProjector;
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
