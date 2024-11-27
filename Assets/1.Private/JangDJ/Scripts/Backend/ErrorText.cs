using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using WebSocketSharp;

[RequireComponent(typeof(TMP_Text))]
public class ErrorText : MonoBehaviour
{
    private StringBuilder _sb;
    private TMP_Text _tmp;
    private Coroutine _routine;
    private float _clearTime = 2f;

    protected void Awake()
    {
        _sb = new StringBuilder();
    }

    private void OnEnable()
    {
        _tmp = GetComponent<TMP_Text>();
    }

    public void ChangeText(string message, bool isAutoClear = true)
    {
        _sb.Clear();
        _sb.Append(message);
        _tmp.text = _sb.ToString();

        if(isAutoClear)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(ClearWaitRoutine());
        }
    }

    public void ClearText()
    {
        _sb.Clear();
        _tmp.text = _sb.ToString();
    }

    private IEnumerator ClearWaitRoutine()
    {
        float timer = 0;

        while (true)
        {
            if (timer >= _clearTime)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        ClearText();
    }

    private void OnDisable()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _tmp = null;
    }
}
