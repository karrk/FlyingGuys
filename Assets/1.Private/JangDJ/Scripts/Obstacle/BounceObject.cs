using UnityEngine;

public class BounceObject : MonoBehaviour, IBounceable
{
    [SerializeField] private float _power;
    [SerializeField] private Animation _anim;

    public float Power
    {
        get
        {
            _anim.Play();

            return _power;
        }
    }


}