using UnityEngine;

[ExecuteInEditMode]
public class PSXEffect : MonoBehaviour
{
    public int pixelWidth = 320;
    public int pixelHeight = 240;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture lowRes = RenderTexture.GetTemporary(pixelWidth, pixelHeight);
        Graphics.Blit(src, lowRes);
        Graphics.Blit(lowRes, dest);
        RenderTexture.ReleaseTemporary(lowRes);
    }
}
