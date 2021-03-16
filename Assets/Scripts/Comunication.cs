using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comunication : MonoBehaviour
{
    public Renderer Texture;
    public float Radius = 1.0f;
    public float Intensive = 0.24f;
    public HUD Hud;

    private float[] time = new float[20];
    Vector4[] Parameters = new Vector4[20];
    int Current = 0;

    Vector3 LastPos;

    public void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            time[i] = 1;
            Parameters[i] = new Vector4(1, 1, 1, 3);
        }
        Texture.material.SetVectorArray("_Circles", Parameters);
        Texture.material.SetFloatArray("_STimes", time);
    }

    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            if (time[i] > 0.02f)
            {
                time[i] -= Time.deltaTime / 5.0f;
            }
            else { time[i] = 0; }
        }
        Texture.material.SetFloatArray("_STimes", time);
    }

    public void Show(Vector3 MousePos)
    {
        if (MousePos!=LastPos)
        {
            LastPos = MousePos;
            time[Current] = 1;
            Parameters[Current] = new Vector4(MousePos.x / Screen.width, MousePos.y / Screen.height, Radius, Intensive);
            Texture.material.SetVectorArray("_Circles", Parameters);
            Current = (Current + 1) % 20;
        }
    }

    public void Show(Vector3 MousePos, float Radius, float Intensive)
    {
        LastPos = MousePos;
        time[Current] = 1;
        Parameters[Current] = new Vector4(MousePos.x / Screen.width, MousePos.y / Screen.height, Radius, Intensive);
        Texture.material.SetVectorArray("_Circles", Parameters);
        //Current = (Current + 1) % 20;
    }

    public void EndGame(Vector3 MousePos)
    {
        StartCoroutine(Complet(MousePos));
    }

    public IEnumerator Complet(Vector3 MousePos)
    {
        Time.timeScale = 0;
        for (int i = 0; i < 300; i++)
        {
            Parameters[0] = new Vector4(MousePos.x / Screen.width, MousePos.y / Screen.height, i*5.0f, i * 0.0001f);
            time[0] = i;
            Texture.material.SetVectorArray("_Circles", Parameters);
            Texture.material.SetFloatArray("_STimes", time);
            yield return new WaitForSecondsRealtime(0.0001f);
        }

        Hud.EndGame();
    }
}
