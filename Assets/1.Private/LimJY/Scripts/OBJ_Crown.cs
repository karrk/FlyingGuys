using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OBJ_Crown : MonoBehaviour
{
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        // TODO : 왕관 전용 애니메이션 재생
    }

    // -------------------------------------------------------------------------

    // Trigger는 추후 Player에서 구현할 예정이므로, 왕관 오브젝트가 잡혓을 때 사용될 public 함수만 구성해둘 것.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //    // TODO : 닿은 플레이어 내부의 bool형 변수를 true로 변경한다.
            //    IsGrabEnable = true;
            //}
            CrownDisable(other.gameObject.GetComponent<PlayerController>());
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //if (other.CompareTag("Player"))
    //{
    //    // TODO : 닿은 플레이어 내부의 bool형 변수를 false로 변경한다.         
    //    IsGrabEnable = false;
    //}
    //}

    // -------------------------------------------------------------------------

    /// <summary>
    /// 왕관 오브젝트 비활성화
    /// <summary>
    void CrownDisable(PlayerController Player)
    {
        //if (IsGrabEnable)
        //{
        // 잡기 기능이 활성화 상태일 때,
        // Player의 State가 grab으로 변경 시, 왕관 오브젝트를 비활성화 한다.
        /*

            for (int i = 0; i < Player.Length; i++)
            {
                if (Player.PlayerController.State !=  State.Grab) return;

                PlayerWinner = Player[i];
            }
         */
        //}
        //else
        //{
        // TODO : 잡기 기능 비활성화 상태일 땐, 왕관 오브젝트와의 상호작용(Grab 비활성화, 잡기 아이콘 비활성화 등)을 비활성화한다.
        //}

        gameObject.SetActive(false);
    }

    // -------------------------------------------------------------------------

    private void OnDisable()
    {
        // TODO : 왕관 오브젝트가 존재하는 씬 전역의 코루틴 사용을 중지한다. or 모든 장애물의 State를 Idle로 변경한다.

        // 임시로 선언한 Player 오브젝트를 보관하는 배열. 해당 배열은 추후 PlayerSpawn 기능에서 저장 후 불러올 예정.

        //for (int i = 0; i < Player.Length; i++)
        //{
        // 모든 플레이어 오브젝트의 MoveSpeed를 0으로 만든다.
        // 또한, 모든 플레이어들은 Idle 상태가 된다.
        //}

        // PlayerWinner.animator.Play(우승 모션 재생);
    }
}

