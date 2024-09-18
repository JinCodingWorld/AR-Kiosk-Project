using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GoogleTranslation : MonoBehaviour
{
    private string apiKey = "  "; // Replace with your actual Translation API key
    private string url = "https://translation.googleapis.com/language/translate/v2?key=";
    public Text translatedText; // UI Text to display the translated text

    public void TranslateText(string textToTranslate)
    {
        StartCoroutine(Translate(textToTranslate));
    }

    private IEnumerator Translate(string textToTranslate)
    {
        var requestData = new
        {
            q = textToTranslate,
            target = "ko", // Korean language code
            source = "en" // English language code
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
            translatedText.text = "Error: " + www.error;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ProcessResponse(www.downloadHandler.text);
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        var response = JsonConvert.DeserializeObject<TranslationResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.translations.Length > 0)
        {
            string translatedTextContent = response.data.translations[0].translatedText;
            translatedText.text = translatedTextContent;
        }
        else
        {
            translatedText.text = "Translation failed.";
        }
    }

    public class TranslationResponse
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public Translation[] translations { get; set; }
    }

    public class Translation
    {
        public string translatedText { get; set; }
    }
}
