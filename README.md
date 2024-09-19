# 내 손안의 키오스크 도우미

1. **서비스 설명** : 디지털 소외 계층인 고령자들을 위한 키오스크 증강현실(AR) 콘텐츠 제작
2. **제작기간** : 24.4.8 ~ 24.9.2
3. **유튜브 영상** : [내 손안의 키오스크 도우미](https://youtu.be/DO94RlasXu4)
4. **개발과정**
   - Google Cloud API 서비스를 사용하기 위해 Nuget 패키지와 API 키를 사용하여 4가지 기능(Cloud Vision, STT, TTS, Translation API)를 구현하였습니다.
   - 슬라이더로 돋보기 기능을 사용하기 위해 WebCamTexture 클래스를 통해 휴대폰 카메라 디바이스를 가져오고, RawImage의 텍스처에 입혀주어 휴대폰 화면에 출력하도록 하였습니다.
   - 사용자가 어떤 가게에 있는지 인식하기 위해 QR코드를 비추면, 뷰포리아 엔진의 이미지 타깃 DB에 저장된 QR이미지와 비교하여 해당 장소의 안내 기능들이 구현된 씬(Scene)으로 자동 전환될 수 있도록 설정하였습니다.
5. **문제점 및 해결과정 (도전과제 및 해결과정)**
   - 돋보기를 사용하는 씬과 안내문구가 띄워지는 씬이 구분되어 있어, 돋보기 씬은 WebcamTexture를 사용하고, 안내문구 씬은 뷰포리아의 AR 카메라를 사용하여 두 씬 간 전환 시, 돋보기 씬의 화면이 검정으로 보이는 현상이 지속됐습니다.
   - WebcamTexture 클래스 특성 상 씬 전환 시 파괴가 되는 것 같았고 씬 혹은 오브젝트의 생명 주기와 연관이 있어, 안내문구 씬으로 넘어갈 때 OnDisable() 함수를 활용하여 WebCamTexture의 변수를 Stop() 해주었더니 정상적으로 작동하였습니다.

---

## 프로젝트 개요서, 수행계획서, 개발보고서, 제작설계서 첨부
- <https://drive.google.com/drive/folders/1FGy-p7iJd5zJpWaYiSj3JrBHzWvxivop?usp=sharing>
  
---
  
## 이미지 첨부
<img src= "https://github.com/user-attachments/assets/207550c5-a644-4aeb-9b10-27e79815bd47" width="40%">  
<img src = "https://github.com/user-attachments/assets/042de945-e2f0-4c4a-8105-fabf17d8e613" width= "40%">
<img width="40%" alt="01" src="https://github.com/user-attachments/assets/0a2719a7-8382-4cf5-8dfe-baa2d2d6f987">
<img src = "https://github.com/user-attachments/assets/58c2ed1d-a135-4c1c-bf09-525de8237a34" width = "40%">
<img width="40%" alt="04" src="https://github.com/user-attachments/assets/dee4b084-75b4-42bb-99c9-00cc69c01fd2">
<img width="40%" alt="05" src="https://github.com/user-attachments/assets/f09cc18f-4496-4116-a81d-cd172b5a9095">
<img width="40%" alt="06" src="https://github.com/user-attachments/assets/839409d5-f7aa-471d-8ebc-bb44eb2a1803">
