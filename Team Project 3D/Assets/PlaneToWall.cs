using UnityEngine;

public class PlaneToWall : MonoBehaviour
{
    void Start()
    {
        // Plane들 찾기 (태그나 이름으로)
        GameObject[] planes = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in planes)
        {
            if (obj.GetComponent<MeshFilter>() != null)
            {
                // Plane인지 확인
                if (obj.GetComponent<MeshFilter>().sharedMesh.name.Contains("Plane"))
                {
                    // 위치, 회전, 크기 저장
                    Vector3 pos = obj.transform.position;
                    Quaternion rot = obj.transform.rotation;
                    Vector3 scale = obj.transform.localScale;
                    Material mat = obj.GetComponent<Renderer>().sharedMaterial;

                    // Cube 생성
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = pos;
                    cube.transform.rotation = rot;

                    // 벽은 얇게 (Z축 0.1)
                    cube.transform.localScale = new Vector3(scale.x, scale.y, 0.1f);

                    // Material 적용
                    cube.GetComponent<Renderer>().material = mat;

                    // 이름 복사
                    cube.name = obj.name.Replace("Plane", "Wall");

                    // 원래 Plane 삭제
                    DestroyImmediate(obj);
                }
            }
        }
    }
}