using UnityEngine;

public class CursorControll : MonoBehaviour
{
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _rt;
    private Vector2 _temp;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas, Input.mousePosition, null, out _temp);

        _rt.anchoredPosition = _temp;
    }
}
