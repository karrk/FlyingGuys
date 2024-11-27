using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class ErrorText : TMP_Text
{
    private StringBuilder _sb;
    private TMP_Text _tmp;
    private Coroutine _routine;
    private float _clearTime = 2f;

    protected override void Awake()
    {
        _sb = new StringBuilder();
        _tmp = this;
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
    }
}
