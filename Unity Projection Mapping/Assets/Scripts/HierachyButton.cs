using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HierachyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    private SelectedManager _selectedManager = null;

    private void Awake()
    {
        _selectedManager = FindObjectOfType<SelectedManager>();
    }

    public void SelectImage()
    {
        _selectedManager.SelectObject(gameObject);
    }

    public void DeleteImage()
    {
        _selectedManager.DeleteObject(gameObject);
    }

    public void SetName(string pName)
    {
        _nameText.text = pName;
    }
}
