using System;

public class SelectedImageEventArgs : EventArgs
{
    public ImageProjector imageProjector;

    public SelectedImageEventArgs(ImageProjector pImageProjector)
    {
        imageProjector = pImageProjector;
    }
}
