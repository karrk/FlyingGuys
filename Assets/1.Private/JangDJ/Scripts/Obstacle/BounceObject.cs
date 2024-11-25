using UnityEngine;

public class BounceObject : MonoBehaviour, IBounceable
{
    [SerializeField] private float _power;
    [SerializeField] private Animation _anim;

    public float Power
    {
        get
        {
            if(_anim != null)
                _anim.Play();

            return _power;
        }
    }
}