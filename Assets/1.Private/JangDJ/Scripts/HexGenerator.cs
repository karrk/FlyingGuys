using UnityEngine;

public class HexGenerator : MonoBehaviour
{
    [SerializeField] private int _widthCount; // 중심 좌우 갯수
    [SerializeField] private int _heightCount;

    [SerializeField] private float _tileInterval;

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Material _tileColor;

    private float _zOffset = -0.57f;

    private float _scaleRatio;

    private void Start()
    {
        _scaleRatio = _tilePrefab.transform.localScale.x;
        _tilePrefab = Instantiate(_tilePrefab);
        _tilePrefab.GetComponent<MeshRenderer>().material = _tileColor;

        _tileInterval *= _scaleRatio;
        _zOffset *= _scaleRatio;

        Destroy(_tilePrefab.gameObject);
        Create();
    }

    private void Create()
    {
        Vector3 startPos;
        GameObject newTile;

        // 중심에서 위방향으로
        for (int i = 0; i <= _heightCount/2; i++) // 0 1 2
        {
            startPos = new Vector3(
                -1 * (_tileInterval * (_widthCount - i)) / 2 + _tileInterval/2,
                transform.position.y,
                _tileInterval * i + _zOffset * i);

            //if (i % 2 != 0)
            //    startPos.x -= _tileInterval/2;

            for (int j = 0; j < _widthCount - i; j++)
            {
                newTile = Instantiate(_tilePrefab, transform);
                newTile.transform.position = startPos + Vector3.right * (_tileInterval * j);
            }
        }

        // 중심 아래한칸부터 아랫방향으로
        for (int i = 1; i <= _heightCount/2; i++)
        {
            startPos = new Vector3(
                -1 * (_tileInterval * (_widthCount - i)) / 2 + _tileInterval / 2,
                transform.position.y,
                _tileInterval * -i - _zOffset * i);

            for (int j = 0; j < _widthCount - i; j++)
            {
                newTile = Instantiate(_tilePrefab, transform);
                newTile.transform.position = startPos + Vector3.right * (_tileInterval * j);
            }
        }
    }
}
