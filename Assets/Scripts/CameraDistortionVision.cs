using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/CameraDistortion")]

public class CameraDistortionVision : MonoBehaviour {
    // public data
    public Shader shader;
    public Color luminence;
    public float noiseFactor = 0.005f;
    //private data
    private Material mat;



    void Start() {
        shader = Shader.Find("Image Effects/CameraDistortion");
        mat = new Material(shader);
        mat.SetVector("lum", new Vector4(luminence.g, luminence.g, luminence.g, luminence.g));
        mat.SetFloat("noiseFactor", noiseFactor);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        mat.SetFloat("time", Mathf.Sin(Time.time * Time.deltaTime));
        Graphics.Blit(source, destination, mat);
    }

    void UpdateNoiseFactor(float noise) {

    }

}