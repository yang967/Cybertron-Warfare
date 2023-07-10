using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject camera_;
    [SerializeField] GameObject Terrain;
    Camera cam;

    public static GameObject instance;
    static float CamSize;
    static Vector3 CamPosition;
    static float length;
    static Vector3 TerrainPosition;

    bool MouseOver;

    private void Awake()
    {
        cam = camera_.GetComponent<Camera>();
        instance = gameObject;
        CamSize = cam.orthographicSize;
        CamPosition = camera_.transform.position;
        TerrainPosition = Terrain.transform.position;
        length = GetComponent<RectTransform>().sizeDelta.x;
        MouseOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetPosition(GameObject obj, GameObject icon)
    {
        Vector3 PosDelta = (obj.transform.position - TerrainPosition) / CamSize / 2.0f;
        Vector3 Pos = PosDelta * length;
        /*float rad = Mathf.Deg2Rad * 135.0f;
        float x = Pos.z * Mathf.Cos(rad) + Pos.x * Mathf.Sin(rad);
        float y = Pos.x * Mathf.Cos(rad) - Pos.z * Mathf.Sin(rad);*/
        Vector3 NewPos = GameManager.VectorRotate(Pos, new Vector3(0, 0, 0), 225.0f);
        icon.GetComponent<RectTransform>().localPosition = new Vector2(NewPos.x, NewPos.z);
    }

    public void OnMouseOver()
    {
        MouseOver = true;
    }

    public void OnMouseExit()
    {
        MouseOver = false;
    }
}
