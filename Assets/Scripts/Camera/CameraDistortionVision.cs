using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/CameraDistortion")]

public class CameraDistortionVision : MonoBehaviour {
    // public data
    public Shader shader;
    public Color luminence;
    public float noiseFactor = 0.005f;
    //private data
    private Material _distortionMat;
    private Material _defaultMat;
    private Material _selectedMat;
    //shader
    private Shader _distortionShader;
    private Shader _defaultShader;

    void Start() {
        _defaultShader = Shader.Find("Image Effects/Camera");
        _defaultMat = new Material(_defaultShader);

        _distortionShader = Shader.Find("Image Effects/CameraDistortion");
        _distortionMat = new Material(_distortionShader);
        _distortionMat.SetVector("lum", new Vector4(luminence.g, luminence.g, luminence.g, luminence.g));
        _distortionMat.SetFloat("noiseFactor", noiseFactor);

        _selectedMat = _distortionMat;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        _distortionMat.SetFloat("time", Mathf.Sin(Time.time * Time.deltaTime));
        Graphics.Blit(source, destination, _selectedMat);
    }

    void UpdateNoiseFactor(float noise) {
        //todo
    }

    public void ChangeDistortion(bool active) {
        if (active) _selectedMat = _distortionMat;
        else _selectedMat = _defaultMat;
    }


}