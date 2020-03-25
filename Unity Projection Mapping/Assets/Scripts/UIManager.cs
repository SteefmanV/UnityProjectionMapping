using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _hierachy = null;
    [SerializeField] private GameObject _selectedPanel = null;
    [SerializeField] private GameObject _materialSelector = null;

    [Header("Tool Bar")]
    [SerializeField] private GameObject _toolbar = null;
    [SerializeField] private Animator _toolbarAnim = null;

    [Header("Animators")]
    [SerializeField] private Animator _fullScreenAnim = null;
    [SerializeField] private Animator _hierachyAnim = null;
    [SerializeField] private Animator _selectedPanelAnim = null;

    private bool _toolbarShow = true;
    private bool _hierachyShow = true;
    private bool _selectedPanelShow = true;

    private HierachyManager _hierachyManager;
    private MaskDrawer _maskDrawer = null;

    private bool _fullScreen = false;

    private void Awake()
    {
        _selectedPanel.SetActive(false);
        _maskDrawer = FindObjectOfType<MaskDrawer>();
        _hierachyManager = FindObjectOfType<HierachyManager>();
    }


    private void Update()
    {
        if(_fullScreen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleFullScreen();
        }
    }


    public void ToggleFullScreen()
    {
        _fullScreen = !_fullScreen;

        if (_selectedPanel.activeSelf) _selectedPanel.SetActive(false);
        if(_materialSelector.activeSelf) _materialSelector.SetActive(false);

        _hierachy.SetActive(!_fullScreen);
        _toolbar.SetActive(!_fullScreen);

        _hierachyManager.DeselectCurrentProjector();
        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        _fullScreenAnim.SetTrigger("enable");
    }


    public void ToggleSelectionPanel(bool pActive)
    {
        if (_selectedPanel.activeSelf != pActive) _selectedPanel.SetActive(pActive);
        _selectedPanelAnim.SetBool("show", _selectedPanelShow);
    }


    public bool IsDrawing()
    {
        return _maskDrawer.currentState != MaskDrawer.DrawState.off;
    }


    public void ToggleToolBar()
    {
        _toolbarShow = !_toolbarShow;
        _toolbarAnim.SetBool("show", _toolbarShow);
    }


    public void ToggleHierachy()
    {
        _hierachyShow = !_hierachyShow;
        _hierachyAnim.SetBool("show", _hierachyShow);
    }


    public void toggleSelectedPanel()
    {
        _selectedPanelShow = !_selectedPanelShow;
        _selectedPanelAnim.SetBool("show", _selectedPanelShow);
    }
}
