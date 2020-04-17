using System.IO;
using UnityEngine;
using System.Windows.Forms;

public class CustomTextureImport : MonoBehaviour
{
    private Material _imageMaterial = null;


    private void Start()
    {
        _imageMaterial = new Material(Shader.Find("HDRP/Lit"));
    }


    public void ImportCustomFile()
    {
        string filePath = searchFileInExplorer();
        Texture2D texture = loadFileIntoTexture(filePath);

        if (texture != null)
        {
            _imageMaterial.SetTexture("_BaseColorMap", texture);
            Sprite materialThumnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
            FindObjectOfType<SelectedUI>().SelectNewMatrial(_imageMaterial, materialThumnail);
        }
    }


    /// <summary>
    /// Opens a file dialog where the user can select a file. Return the string of the selected file. 
    /// </summary>
    private string searchFileInExplorer()
    {
        string filePath = "";

        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "Image (*.PNG; *.JPG;)| *.PNG; *.JPG;"; // Show only PNG & JPG files
        fileDialog.FilterIndex = 1;
        fileDialog.Multiselect = false; // Allow only 1 image

        if (fileDialog.ShowDialog() == DialogResult.OK)
            filePath = fileDialog.FileName;

        return filePath;
    }


    /// <summary>
    /// Loads image from filepath and converts it to a Texture2D
    /// </summary>
    private Texture2D loadFileIntoTexture(string pFilePath)
    {
        Texture2D texture = new Texture2D(1,1); // Create default texture, size doesn't matter

        if (File.Exists(pFilePath))
        {
            byte[] fileData = File.ReadAllBytes(pFilePath);
            texture.LoadImage(fileData); // Loads image data and resets the texture size
        }

        return texture;
    }
}

