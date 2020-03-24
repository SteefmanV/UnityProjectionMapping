using UnityEngine;
using UnityEngine.UI;

public class MaskDrawer : MonoBehaviour
{

    public enum DrawState { off, draw, erase };
    public DrawState currentState = DrawState.off;

    [SerializeField] private RawImage _maskImage;
    [SerializeField] private Image _drawCursorImage = null;
    [SerializeField] private SelectedManager _selectedManager = null;
    private Texture2D _maskTexture;

    [Header("Draw Settings")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private float _radius;
    [SerializeField] private float _scrollSpeed = 1;

    private Color _black = Color.black;
    private Color _transparant = new Color(1, 1, 1, 0);
    private Color[] _imageColors;

    private Vector2 _lastPos;
    private int _screenWidth;


    private void Awake()
    {
        _maskTexture = new Texture2D(Screen.width, Screen.height);
        _maskImage.texture = _maskTexture;
        _screenWidth = Screen.width;
        _selectedColor = _black;

        ResetTexture();
        resetBuffer();
        _lastPos = Input.mousePosition;

        updateCursorSize();
    }


    void Update()
    {
        handleMouseInput();
    }


    /// <summary>
    /// Clears the texture
    /// </summary>
    public void ResetTexture()
    {
        Color[] _maskPixels = new Color[Screen.width * Screen.height];

        for (int x = 0; x < Screen.width; ++x)
        {
            for (int y = 0; y < Screen.height; ++y)
            {
                _maskPixels[y * Screen.width + x] = _transparant;
            }
        }

        _maskTexture.SetPixels(_maskPixels);
        _maskTexture.Apply();

        resetBuffer();
    }


    /// <summary>
    /// Toggles draw mode
    /// </summary>
    public void ToggleDraw()
    {
        if (currentState != DrawState.draw)
        {
            currentState = DrawState.draw;
            _selectedColor = _black;
            _selectedManager.DeselectCurrentProjector();
        }
        else currentState = DrawState.off;
    }


    /// <summary>
    /// Toggles erase mode
    /// </summary>
    public void ToggleErase()
    {
        if (currentState != DrawState.erase)
        {
            currentState = DrawState.erase;
            _selectedColor = _transparant;
            _selectedManager.DeselectCurrentProjector();
        }
        else currentState = DrawState.off;
    }


    /// <summary>
    /// Handle mouse put
    /// </summary>
    private void handleMouseInput()
    {
        updateMouseCursor();

        if (currentState != DrawState.off)
        {
            if (Input.GetMouseButtonDown(0))
            {
                drawCircle((int)Input.mousePosition.x, (int)Input.mousePosition.y, (int)_radius);
            }

            if (Input.GetMouseButton(0))
            {
                drawThickLine(_lastPos, Input.mousePosition);
                drawCircle((int)Input.mousePosition.x, (int)Input.mousePosition.y, (int)_radius);
                applyChanges();
            }

            _drawCursorImage.gameObject.SetActive(true);
        }
        else
        {
            _drawCursorImage.gameObject.SetActive(false);
        }

        _lastPos = Input.mousePosition;
    }


    /// <summary>
    /// Draws a circle
    /// </summary>
    private void drawCircle(int pXCenter, int pYCenter, int pRadius)
    {
        for (int x = 0; x < pRadius * 2; ++x)
        {
            for (int y = 0; y < pRadius * 2; ++y)
            {
                float d = distance(x, y, pRadius, pRadius);
                if (d < pRadius)
                {
                    SetPixel(x + pXCenter - pRadius, y + pYCenter - pRadius, _selectedColor);
                }
            }
        }
    }


    /// <summary>
    /// Draws a line from p1 to p2
    /// </summary>
    private void DrawLine(Vector2 p1, Vector2 p2)
    {
        Vector2 t = p1;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;

            SetPixel((int)t.x, (int)t.y, _selectedColor);
            SetPixel((int)t.x + 1, (int)t.y, _selectedColor);
            SetPixel((int)t.x + 1, (int)t.y + 1, _selectedColor);
            SetPixel((int)t.x, (int)t.y + 1, _selectedColor);
        }
    }


    /// <summary>
    /// Draws a line from p1 to p2 including it's radius
    /// </summary>
    private void drawThickLine(Vector2 p1, Vector2 p2)
    {
        Vector2 offset = p2 - p1;
        offset.Normalize();
        Vector2 rotOffset = new Vector2(-offset.y, offset.x);

        for (int i = (int)-_radius; i < _radius + 1; i++)
        {
            DrawLine(p1 + i * rotOffset, p2 + i * rotOffset);
        }
    }


    /// <summary>
    /// Return the distance between 2 points
    /// </summary>
    private float distance(int pX1, int pY1, int pX2, int pY2)
    {
        int deltaX = pX2 - pX1;
        int deltaY = pY2 - pY1;
        return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }


    /// <summary>
    /// Empty the imageColors array
    /// </summary>
    private void resetBuffer()
    {
        _imageColors = new Color[Screen.width * Screen.height];
    }


    /// <summary>
    /// Apply color imageColors to the texture
    /// </summary>
    private void applyChanges()
    {
        _maskTexture.SetPixels(_imageColors);
        _maskTexture.Apply();
    }


    /// <summary>
    /// Set pixels color of the texture
    /// </summary>
    private void SetPixel(int pX, int pY, Color pColor)
    {
        _imageColors[pY * _screenWidth + pX] = pColor;
    }


    /// <summary>
    /// Update mousecursor position and size
    /// </summary>
    private void updateMouseCursor()
    {
        _drawCursorImage.rectTransform.localPosition = (Vector2)Input.mousePosition - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0)
        {
            _radius += mouseScroll * _scrollSpeed; // Change radius size 
            updateCursorSize();
        }
    }


    /// <summary>
    /// Sets the cursor size equal to the draw radius
    /// </summary>
    private void updateCursorSize()
    {
        // Cursor image is 1000x1000 so *0.001f makes it 1 pixel
        _drawCursorImage.rectTransform.localScale = new Vector2(_radius * 2 * 0.001f, _radius * 2 * 0.001f);
    }
}
