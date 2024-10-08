using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class CameraSetting : MonoBehaviour
{
    [Header("Setting")]
    public Vector2 requestedRatio; //설정하고자 하는 카메라 비율
    public int requestedFPS; //설정하고자 하는 프레임
    private bool twotimesButton = false;
    private bool fourtimesButton = false;
    private bool resetButton = false;

    [Header("Component")]
    public RawImage webCamRawImage; //Canvas에 올려둔 RawImage
    //public Button captureButton; // Button to capture the image

    [Header("Data")]
    public WebCamTexture webCamTexture; //Camera 화면을 지닌 Texture
    public GoogleVisionAPI visionAPI; // Reference to Vision API script
    // Start is called before the first frame update
    public float captureInterval = 3.0f; // Time interval between captures

    void Start()
    {
        SetWebCam();

        //captureButton.onClick.AddListener(CaptureAndAnalyze);
    }
    private void SetWebCam() //WebCam을 지정하는 함수
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera)) //이미 카메라 권한을 획득한 경우에는
            CreateWebCamTexture(); //WebCamTexture를 생성하는 함수 바로 실행
        else //카메라 권한이 없는 경우에는
        {
            PermissionCallbacks permissionCallbacks = new();
            permissionCallbacks.PermissionGranted += CreateWebCamTexture; //권한 획득 후 실행될 함수로 WebCamTexture를 생성하는 함수 추가
            Permission.RequestUserPermission(Permission.Camera, permissionCallbacks); //카메라 권한을 요청
        }
    }

    private void CreateWebCamTexture(string permissionName = null) //WebCamTexture를 생성하는 함수
    {
        if (webCamTexture) //이미 WebCamTexture가 존재하는 경우에는
        {
            Destroy(webCamTexture); //삭제한다 (메모리 관리)
            webCamTexture = null;
        }

        WebCamDevice[] webCamDevices = WebCamTexture.devices; //현재 접근할 수 있는 카메라 기기들을 모두 가져온다
        if (webCamDevices.Length == 0) return; //만약, 카메라가 없다면 종료

        int backCamIndex = -1; //후방 카메라를 저장하기 위한 변수
        for (int i = 0, l = webCamDevices.Length; i < l; ++i) //카메라를 탐색하면서
        {
            if (!webCamDevices[i].isFrontFacing) //후방 카메라를 발견하면
            {
                backCamIndex = i; //인덱스 저장
                break; //반복문 빠져나오기
            }
        }

        if (backCamIndex != -1) //후방 카메라를 발견했으면
        {
            int requestedWidth = Screen.width; //설정하고자 하는 가로 픽셀 변수 선언 (현재 화면의 가로 픽셀을 기본값으로 지정)
            int requestedHeight = Screen.height; //설정하고자 하는 세로 픽셀 변수 선언 (현재 화면의 세로 픽셀을 기본값으로 지정)
            for (int i = 0, l = webCamDevices[backCamIndex].availableResolutions.Length; i < l; ++i) //현재 선택된 후방 카메라가 활용할 수 있는 해상도를 탐색하면서
            {
                Resolution resolution = webCamDevices[backCamIndex].availableResolutions[i];
                if (GetAspectRatio((int)requestedRatio.x, (int)requestedRatio.y).Equals(GetAspectRatio(resolution.width, resolution.height))) //설정하고자 하는 비율과 일치하는 해상도를 발견하면
                {
                    requestedWidth = resolution.width; //설정하고자 하는 가로 픽셀로 지정
                    requestedHeight = resolution.height; //설정하고자 하는 세로 픽셀로 지정
                    break; //반복문 빠져나오기
                }
            }

            webCamTexture = new WebCamTexture(webCamDevices[backCamIndex].name, requestedWidth, requestedHeight, requestedFPS); //카메라 이름으로 WebCamTexture 생성
            webCamTexture.filterMode = FilterMode.Trilinear;
            webCamTexture.Play(); //카메라 재생

            webCamRawImage.texture = webCamTexture; //RawImage에 할당하기

            StartCoroutine(CaptureAndAnalyzeRepeatedly()); // Start capturing images repeatedly
        }
    }

    private string GetAspectRatio(int width, int height, bool allowPortrait = false) //비율을 반환하는 함수
    {
        if (!allowPortrait && width < height) Swap(ref width, ref height); //세로가 허용되지 않는데, (가로 < 세로)이면 변수값 교환
        float r = (float)width / height; //비율 저장
        return r.ToString("F2"); //소수점 둘째까지 잘라서 문자열로 반환
    }
    private void Swap<T>(ref T a, ref T b) //두 변수값을 교환하는 함수
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    private void Update()
    {
        UpdateWebCamRawImage();

        if (twotimesButton)
        {
            ScaleRawImageByFactor(1.5f);
            twotimesButton = false;
        }
        if (fourtimesButton)
        {
            ScaleRawImageByFactor(3f);
            fourtimesButton = false;
        }
        if (resetButton)
        {
            ScaleRawImageByFactor(1f);
            resetButton = false;
        }
    }
    private void UpdateWebCamRawImage() //RawImage를 관리하는 함수
    {
        if (!webCamTexture) return; //WebCamTexture가 존재하지 않으면 종료

        int videoRotAngle = webCamTexture.videoRotationAngle;
        webCamRawImage.transform.localEulerAngles = new Vector3(0, 0, -videoRotAngle); //카메라 회전 각도를 반영

        int width, height;
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) //세로 화면이면
        {
            width = Screen.width; //가로를 고정하고
            height = Screen.width * webCamTexture.width / webCamTexture.height; //WebCamTexture의 비율에 따라 세로를 조절
        }
        else //가로 화면이면
        {
            height = Screen.height; //세로를 고정하고
            width = Screen.height * webCamTexture.width / webCamTexture.height; //WebCamTexture의 비율에 따라 가로를 조절
        }

        if (Mathf.Abs(videoRotAngle) % 180 != 0f) Swap(ref width, ref height); //WebCamTexture 자체가 회전되어있는 경우 가로/세로 값을 교환
        webCamRawImage.rectTransform.sizeDelta = new Vector2(width, height); //RawImage의 size로 지정
    }

    private IEnumerator CaptureAndAnalyzeRepeatedly() // Coroutine to capture and analyze images repeatedly
    {
        while (true)
        {
            CaptureAndAnalyze(); // Capture the current image
            yield return new WaitForSeconds(captureInterval); // Wait for the specified interval before capturing again
        }
    }
    void CaptureAndAnalyze()
    {
        webCamTexture.Pause(); // Pause camera to capture the image
        Texture2D capturedImage = new Texture2D(webCamTexture.width, webCamTexture.height);
        capturedImage.SetPixels(webCamTexture.GetPixels());
        capturedImage.Apply();

        // Send captured image to Vision API
        visionAPI.CallVisionAPI(capturedImage);

        webCamTexture.Play(); // Resume camera feed
    }

    void ScaleRawImageByFactor(float scaleFactor)
    {
        if (webCamRawImage != null)
        {
            // Access the RectTransform of the RawImage
            RectTransform rectTransform = webCamRawImage.rectTransform;

            // Scale the RawImage by changing its localScale
            rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    public void twoTimesButtonClicked()
    {
        twotimesButton = true;
    }
    public void fourTimesButtonClicked()
    {
        fourtimesButton = true;
    }
    public void resetButtonClicked()
    {
        resetButton = true;
    }

}
