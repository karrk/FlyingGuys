using System.Collections;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    private const string ROTATION_PROPERTY = "_Rotation";
    private Material _skyMat;
    [SerializeField] private float _speed;

    //private static int RotateID = -1;
    
    private void Start()
    {
        _skyMat = RenderSettings.skybox;
        
        //if(RotateID == -1)
        //{
        //    Shader shader = _skyMat.shader;
        //    RotateID = shader.FindPropertyIndex(ROTATION_PROPERTY);
        //}

        StartCoroutine(RotateSkybox());
    }

    private IEnumerator RotateSkybox()
    {
        float value = Random.Range(0,180);

        while (true)
        {
            value += _speed * Time.deltaTime;

            // TODO 최적화
            _skyMat.SetFloat(ROTATION_PROPERTY, value);

            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
