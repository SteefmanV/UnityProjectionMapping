using UnityEngine;
using UnityEngine.Video;
using System.Windows.Forms;

public class CustomVideoImporter : MonoBehaviour
{
    public void ImportCustomVideo()
    {    
        string filePath = searchFileInExplorer();
        FindObjectOfType<SelectedUI>().SelectNewVideo(filePath);
    }


    /// <summary>
    /// Opens a file dialog where the user can select a file. Return the string of the selected file. 
    /// </summary>
    private string searchFileInExplorer()
    {
        string filePath = "";

        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "Image (*.MP4;)| *.MP4"; // Show only PNG & JPG files
        fileDialog.FilterIndex = 1;
        fileDialog.Multiselect = false; // Allow only 1 image

        if (fileDialog.ShowDialog() == DialogResult.OK)
            filePath = fileDialog.FileName;

        return filePath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
