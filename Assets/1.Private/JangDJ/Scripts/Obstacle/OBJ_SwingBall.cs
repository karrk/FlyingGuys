using UnityEngine;

public class OBJ_SwingBall : OBJ_Hurdle_Roller
{
    [SerializeField] private float _turnTime = 2f;
    private float _time;

    private float _sign = 1;
    private bool _once = false;

    protected override void Start()
    {
        base.Start();
        _time = _turnTime;
    }

    protected override void FixedUpdate()
    {
        if (_time >= 0)
            _time -= Time.fixedDeltaTime;
        else
        {
            _time = _turnTime;
            _sign *= -1;

            if(_once == false)
            {
                _sign = -2;
                _once = true;
            }
        }

        //_rb.AddTorque(_rotateSpeed * _sign);
    }
}