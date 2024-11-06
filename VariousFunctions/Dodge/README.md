# 회피 시스템 (Dodge System)

## 기능 설명
회피 시스템은 플레이어가 적의 공격을 회피할 수 있는 메커니즘을 제공합니다. 
특정 조건을 만족할 경우 플레이어는 회피를 발동하고, 이로 인해 다양한 시각적 및 음향 효과가 발생하여 플레이어에게 몰입을 통한 즐거운 경험을 제공합니다.
  
## 회피 시 발생하는 일
회피가 발동되면 다음과 같은 일들이 발생합니다:
1. 사운드 재생: 회피 소리가 재생됩니다.
2. 모션 트레일 활성화: 회피 효과를 위한 잔상 효과가 활성화됩니다.
3. 카메라 흔들림: 카메라가 흔들리며 긴장감을 더합니다.
4. 플래시 효과: 화면이 일시적으로 깜빡이며 회피의 성공을 강조합니다.
5. 무적 상태 부여: 플레이어는 일정 시간 동안 무적 상태가 되어 후속타를 피할 수 있습니다.

## 주요 효과 코드  

### 회피 동작
회피가 발생했을 때의 효과를 처리하는 코루틴입니다. 회피 소리 재생, 화면 흔들림, 플래시 효과 등을 포함합니다.

```cs
IEnumerator Dodge()
{
    SoundManager.GetInstance.SFXPlay("dodge", dodgeSound);
    this.transform.GetComponent<MotionTrail>().ActivateTrail();
    yield return null;
    GameManager.GetInstance.PlayStop(0.05f);
    cameraInfo.TriggerShake();
    EffectManager.GetInstance.TriggerFlash();
    SetDefenseState(DefenseState.Invincible);

    yield return new WaitForSecondsRealtime(0.5f);

    this.transform.GetComponent<MotionTrail>().DeactivateTrail();
    GameManager.GetInstance.PlayStart();

    yield return new WaitForSecondsRealtime(0.25f);

    ClearDefenseState(DefenseState.Invincible);
    dodgeCoroutine = null;
}
```

### 카메라 흔들림 효과
회피 시 카메라 흔들림을 구현하는 코루틴입니다. 
```cs
private IEnumerator Shake()
{
    float elapsed = 0f;

    while (elapsed < shakeDuration)
    {
        Vector3 randomOffset = originalPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;
        cameraObj.transform.localPosition = randomOffset;

        elapsed += Time.unscaledDeltaTime;
        yield return null;
    }

    cameraObj.transform.localPosition = originalPosition; // 원래 위치로 복귀
}
```

### 플래시 효과
플래시 효과를 구현하는 코루틴입니다.
```cs
private IEnumerator FlashEffect()
{
    float elapsed = 0f;
    flashImage.gameObject.SetActive(true);

    while (elapsed < flashDuration / 2)
    {
        elapsed += Time.unscaledDeltaTime;
        float alpha = Mathf.Lerp(0, 0.01f, elapsed / (flashDuration / 2));
        flashImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        yield return null; 
    } 

    elapsed = 0f; 
     
    while (elapsed < flashDuration / 2)
    {
        elapsed += Time.unscaledDeltaTime;
        float alpha = Mathf.Lerp(0.01f, 0, elapsed / (flashDuration / 2));
        flashImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        yield return null; 
    }
     
    flashImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    flashImage.gameObject.SetActive(false); // 플래시 비활성화
}
```

### 잔상 효과
[잔상 효과는 이전의 글에서 설명한 내용을 참조하세요.][Dodgelink]
 
## 유튜브
 [![Video Label](http://img.youtube.com/vi/yIT-iTqE1tA/0.jpg)](https://youtu.be/yIT-iTqE1tA)
  
 [Dodgelink]: /VariousFunctions/Dodge