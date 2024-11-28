# 유니티 폴더 툴팁 시스템
 이 시스템은 유니티 에디터에서 폴더에 대한 툴팁을 관리, 표시, 편집할 수 있는 도구를 제공합니다.    
 다음의 3가지 주요 구성 요소로 이루어져 있습니다.   

---
 
## 1. Folder Info Inspector
 폴더 인스펙터를 커스터마이징하여 폴더 툴팁과 하위 내용을 표시합니다.
 
 ### 주요 기능:
* 툴팁 표시: 선택한 폴더에 설정된 툴팁을 표시합니다.
* 폴더 내용 확인: 선택한 폴더의 하위 폴더 및 파일 목록을 보여줍니다.
* 미리보기: 최대 10개의 파일에 대해 미리보기를 제공합니다.

 ### 코드 주요 내용:
* CustomEditor(typeof(DefaultAsset))을 통해 DefaultAsset을 대상으로 폴더를 선택하면  자동으로 툴팁 및 내용을 표시
* 캐시 관리: 디렉터리와 파일 정보를 캐싱하여 성능 최적화.
* 툴팁 로드: JSON 파일을 통해 툴팁 데이터를 불러옵니다.
 
---
  
## 2.  Folder Tooltip Editor
 폴더 툴팁을 관리할 수 있는 커스텀 에디터 창을 제공합니다.
 
 ### 주요 기능:
* 툴팁 추가/수정: 선택한 폴더에 툴팁을 설정하거나 수정할 수 있습니다.
* 저장 및 정리: 툴팁 데이터를 JSON 파일로 저장하고, 더 이상 존재하지 않는 폴더의 툴팁을 제거합니다.

 ### 코드 주요 내용:
* 툴팁 관리: 툴팁을 추가, 수정, 삭제할 수 있습니다.
* 유효성 검사: 삭제된 폴더나 파일에 연결된 툴팁을 자동으로 제거합니다.
 
---
  
## 3. Folder Tooltip Drawer
 프로젝트 창에서 폴더에 마우스를 올릴 때 툴팁을 표시합니다.
 
 ### 주요 기능:
* 툴팁 표시: 폴더 위에 마우스를 올리면 툴팁이 팝업으로 표시됩니다.
* 실시간 업데이트: JSON 파일에서 툴팁 데이터를 자동으로 불러옵니다.

 ### 코드 주요 내용:
* EditorApplication.projectWindowItemOnGUI를 사용하여 폴더 툴팁을 동적으로 렌더링합니다.
 
---

## 그 외

### JSON 데이터 구조
``` json
{
  "keys": ["Assets/MyFolder"],
  "values": ["이 폴더에 대한 툴팁 설명입니다."]
} 
```

### 주요 이점
* 프로젝트에서 폴더 구조를 효율적으로 관리.
* 폴더에 대한 메모나 설명을 빠르게 확인 가능.
* 전용 에디터 창을 통해 직관적으로 툴팁 관리.

---