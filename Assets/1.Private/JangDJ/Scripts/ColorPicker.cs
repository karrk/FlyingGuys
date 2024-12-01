using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    private static Vector2 LastPostion = Vector2.zero;

    [SerializeField] private Image _palette;
    [SerializeField] private Image _picker;
    [SerializeField] public Color SelectedColor;
    [SerializeField] private Renderer _characterRenderer;
    [SerializeField] private CircleCollider2D _coll;

    [SerializeField] private MenuColorLoader _character;

    private Vector2 _size;

    private void OnEnable()
    {
        _picker.transform.position = LastPostion;
    }

    private void Start()
    {
        InitSize();
    }

    private void OnDisable()
    {
        Vector3 convertedValue = new Vector3(SelectedColor.r, SelectedColor.g, SelectedColor.b);
        PhotonNetwork.LocalPlayer.SetColor(convertedValue);
        LastPostion = _picker.transform.position;
    }

    private void InitSize()
    {
        _size = new Vector2(
            _palette.GetComponent<RectTransform>().rect.width,
            _palette.GetComponent<RectTransform>().rect.height);
    }

    private void SelectColor()
    {
        Vector3 offset = Input.mousePosition - transform.position;
        Vector3 diff = Vector3.ClampMagnitude(offset, _coll.radius);

        _picker.transform.position = transform.position + diff;

        SelectedColor = GetColor();
        _character.SetColor(SelectedColor);
    }

    private Color GetColor()
    {
        Vector2 circlePalettePosition = _palette.transform.position;
        Vector2 pickerPosition = _picker.transform.position;

        Vector2 position = pickerPosition - circlePalettePosition + _size * 0.5f;
        Vector2 normalized = new Vector2(
            (position.x / (_palette.GetComponent<RectTransform>().rect.width)),
            (position.y / (_palette.GetComponent<RectTransform>().rect.height)));

        Texture2D texture = _palette.mainTexture as Texture2D;
        Color circularSelectedColor = texture.GetPixelBilinear(normalized.x, normalized.y);

        return circularSelectedColor;
    }

    public void MousePointerDown()
    {
        SelectColor();
    }

    public void MouseDrag()
    {
        SelectColor();
    }
}
