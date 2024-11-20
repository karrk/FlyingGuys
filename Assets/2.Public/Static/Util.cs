using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// 값 변수 동기화
    /// </summary>
    public static void SendAndReceiveStruct<T>(this PhotonStream stream, ref T value) where T : struct
    {
        if (stream.IsWriting)
        {
            stream.SendNext((T)value);
        }
        else if (stream.IsReading)
        {
            value = (T)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 컴포넌트 동기화
    /// </summary>
    public static void SendAndReceiveClass<T>(this PhotonStream stream, ref T value) where T : Component
    {
        if (stream.IsWriting)
        {
            if (value == null) // null 인 경우 임의 값 -1 넣음
            {
                stream.SendNext(-1);
            }
            else
            {
                PhotonView photonView = value.GetComponent<PhotonView>();
                stream.SendNext(photonView.ViewID);
            }
        }
        else if (stream.IsReading)
        {
            int id = (int)stream.ReceiveNext();
            if (id == -1) // -1 인 경우 null
            {
                value = null;
            }
            else
            {
                PhotonView target = PhotonView.Find(id);
                value = target.GetComponent<T>();
            }
        }
    }
}
