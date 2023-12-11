using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ImageDownloader : MonoBehaviour
{
    public RawImage poster; // Assign this in the inspector or find it dynamically

    public void SetImageFromUrl(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    private IEnumerator DownloadImage(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            // Send the request and wait for the desired image
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the downloaded texture
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                // If you want to apply this image to a UI element in Unity, convert it to a Sprite
                Sprite posterSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Set the RawImage's texture to the downloaded texture
                poster.texture = texture;

                // Or if you're applying to an Image component instead of a RawImage, set the sprite
                // yourImageComponent.sprite = posterSprite;
            }
            else
            {
                Debug.LogError("Image download failed! URL: " + imageUrl + " Error: " + request.error);
            }
        }
    }
}