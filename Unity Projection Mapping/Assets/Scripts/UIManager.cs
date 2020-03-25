using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _hierachy = null;
    [SerializeField] private GameObject _selectedPanel = null;
    [SerializeField] private GameObject _toolBar = null;

    private HierachyManager _hierachyManager;
    private MaskDrawer _maskDrawer = null;

    private bool _fullScreen = false;

    private void Awake()
    {
        _selectedPanel.SetActive(false);
        _maskDrawer = FindObjectOfType<MaskDrawer>();
        _hierachyManager = FindObjectOfType<HierachyManager>();
    }


    public void ToggleFullScreen()
    {
        _fullScreen = !_fullScreen;
        if (_selectedPanel.activeSelf) _selectedPanel.SetActive(false);
        _hierachy.SetActive(!_fullScreen);
        _toolBar.SetActive(!_fullScreen);

        _hierachyManager.DeselectCurrentProjector();
        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


    public void ToggleSelectionPanel(bool pActive)
    {
        _selectedPanel.SetActive(pActive);
    }


    public bool IsDrawing()
    {
        return _maskDrawer.currentState != MaskDrawer.DrawState.off;
    }
}
