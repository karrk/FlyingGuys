using System;
using UnityEngine;

public interface IPooledObject
{
    public Enum MyType { get; }
    public GameObject MyObject { get; }
    public void Return();
}