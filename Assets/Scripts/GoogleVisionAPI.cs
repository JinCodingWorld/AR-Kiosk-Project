using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoogleVisionAPI : MonoBehaviour
{
    private string apiKey = "AIzaSyC54yBM_zkDNQc8-_ZlaPJjr1JN8mqIZsA"; // Replace with your actual API key
    private string url = "https://vision.googleapis.com/v1/images:annotate?key=";
    public Text resultText; // UI Text to display results

    public GameObject ToCheck;

    public void CallVisionAPI(Texture2D texture)
    {
        StartCoroutine(Upload(texture));
    }

    private IEnumerator Upload(Texture2D texture)
    {
        byte[] imageBytes = texture.EncodeToPNG();
        string base64Image = System.Convert.ToBase64String(imageBytes);

        var requestData = new
        {
            requests = new[]
            {
                new
                {
                    image = new
                    {
                        content = base64Image
                    },
                    features = new[]
                    {
                        new
                        {
                            type = "TEXT_DETECTION",
                            maxResults = 10
                        }
                    }
                }
            }
        };

        string jsonData = JsonConvert.SerializeObject(requestData);
        UnityWebRequest www = new UnityWebRequest(url + apiKey, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            resultText.text = "Error: " + www.error;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ProcessResponse(www.downloadHandler.text);
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        var response = JsonConvert.DeserializeObject<VisionResponse>(jsonResponse);
        if (response.responses != null && response.responses.Length > 0)
        {
            var annotations = response.responses[0].textAnnotations;
            if (annotations != null && annotations.Length > 0)
            {
                string detectedText = annotations[0].description;
                resultText.text = detectedText;

                // Check for specific text and trigger actions
                if (detectedText.Contains("매점"))
                {
                    SceneManager.LoadScene(7);
                }
                else if(detectedText.Contains("티켓구매"))
                {
                    SceneManager.LoadScene(7);
                }
            }
            else
            {
                resultText.text = "No text detected.";
            }
        }
        else
        {
            resultText.text = "No response from API.";
        }
    }

    public class VisionResponse
    {
        public Response[] responses { get; set; }
    }

    public class Response
    {
        public TextAnnotation[] textAnnotations { get; set; }
    }

    public class TextAnnotation
    {
        public string description { get; set; }
        public BoundingPoly boundingPoly { get; set; }
    }

    public class BoundingPoly
    {
        public Vertex[] vertices { get; set; }
    }

    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
