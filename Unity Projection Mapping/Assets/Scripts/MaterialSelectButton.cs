using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectButton : MonoBehaviour
{
    public Material material;
    private Sprite _matThumb;

    private void Awake()
    {
        _matThumb = GetComponent<Image>().sprite;
    }

    public void SelectMaterial()
    {
        FindObjectOfType<SelectedUI>().selectNewMatrial(material, _matThumb);
    }
}
