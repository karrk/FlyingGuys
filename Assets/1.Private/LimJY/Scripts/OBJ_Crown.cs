using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OBJ_Crown : MonoBehaviour
{
    [SerializeField] bool IsGrabEnable; // 임시 설정된 플레이어의 bool 변수. 추후 삭제 예정.
    [SerializeField] GameObject PlayerWinner; // 임시 설정된, 왕관을 잡은 플레이어를 저장하는 변수. 추후 삭제 예정

    [SerializeField] Image ShortLoadingImage; // 화면이 넘어갈 때 잠시 재생되는 이미지

    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        // TODO : 왕관 전용 애니메이션 재생
        StartCoroutine(ShortLoadingPlay());
    }

    // -------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO : 닿은 플레이어 내부의 bool형 변수를 true로 변경한다.
            IsGrabEnable = true;
        }
    }

    //private void OnTriggerStay(Collider[] other)
    //{
    //    //if (other[].CompareTag("Player"))
    //    //{

    //    //}
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO : 닿은 플레이어 내부의 bool형 변수를 false로 변경한다.         
            IsGrabEnable = false;
        }
    }

    // -------------------------------------------------------------------------

    /// <summary>
    /// 왕관 오브젝트 비활성화
    /// <summary>
    void CrownDisable(GameObject[] Player)
    {
        if (IsGrabEnable)
        {
            // 잡기 기능이 활성화 상태일 때,
            // Player의 State가 grab으로 변경 시, 왕관 오브젝트를 비활성화 한다.
            /*

                for (int i = 0; i < Player.Length; i++)
                {
                    if (Player.PlayerController.State !=  State.Grab) return;

                    PlayerWinner = Player[i];
                }
             */
            gameObject.SetActive(false); // 추후 if문 사용 예정
        }
        else
        {
            // TODO : 잡기 기능 비활성화 상태일 땐, 왕관 오브젝트와의 상호작용(Grab 비활성화, 잡기 아이콘 비활성화 등)을 비활성화한다.
        }
    }

    // -------------------------------------------------------------------------

    private void OnDisable()
    {
        // TODO : 왕관 오브젝트가 존재하는 씬 전역의 코루틴 사용을 중지한다. or 모든 장애물의 State를 Idle로 변경한다.

        // 임시로 선언한 Player 오브젝트를 보관하는 배열. 해당 배열은 추후 PlayerSpawn 기능에서 저장 후 불러올 예정.
        GameObject[] Player = new GameObject[7]; // 임시로 8칸을 만들어둔다.

        for (int i = 0; i < Player.Length; i++)
        {
            // 모든 플레이어 오브젝트의 MoveSpeed를 0으로 만든다.
            // 또한, 모든 플레이어들은 Idle 상태가 된다.
        }

        // PlayerWinner.animator.Play(우승 모션 재생);
        StartCoroutine(ShortLoadingPlay());

    }

    IEnumerator ShortLoadingPlay()
    {
        ShortLoadingImage.gameObject.SetActive(true);

        for (float i = 0; i == 1f; i += 0.1f)
        {
            ShortLoadingImage.fillAmount = i;
        }

        ShortLoadingImage.gameObject.SetActive(false);
        yield return null;
    }


    IEnumerator ShortLoadingRePlay()
    {
        ShortLoadingImage.gameObject.SetActive(true);

        for (float i = 1; i == 0f; i -= 0.1f)
        {
            ShortLoadingImage.fillAmount = i;
        }

        ShortLoadingImage.gameObject.SetActive(false);
        yield return null;
    }
}
