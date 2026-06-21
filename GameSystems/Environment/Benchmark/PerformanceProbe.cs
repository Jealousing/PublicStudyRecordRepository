using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Unity.Profiling;
using UnityEngine;

/// <summary>
/// mesh ↔ RT 적설 방식의 성능을 동일 조건에서 측정하는 프로브.
///
/// 측정 주장("mesh는 CPU 병목 → RT로 GPU 이전")을 검증하려면 fps보다 메인 스레드 시간이
/// 직접적인 근거이므로, ProfilerRecorder로 메인 스레드 시간 + 프레임타임을 함께 수집한다.
///
/// 사용: 빈 GameObject에 부착 → runLabel을 "mesh"/"RT"로 지정 → 빌드에서 실행.
/// 두 방식은 재실행으로 전환하므로(종료 → 모드 전환 → 재실행), 이 프로브는 현재 실행 중인
/// 방식이 무엇이든 그 성능을 measureSeconds 동안 모아 콘솔 + CSV로 출력한다.
/// 같은 CSV에 append하므로 mesh/RT 두 번의 실행 결과가 한 파일에 쌓여 비교가 쉽다.
/// </summary>
public class PerformanceProbe : MonoBehaviour
{
    #region Inspector

    [Header("Run")]
    [Tooltip("이번 실행이 어느 방식인지 (CSV/로그 라벨). 예: mesh, RT")]
    [SerializeField] private string runLabel = "RT";

    [Header("Timing")]
    [Tooltip("측정 전 워밍업(초) — 셰이더 컴파일·풀 생성 등 초기 스파이크 제외")]
    [SerializeField] private float warmupSeconds = 3f;

    [Tooltip("측정 구간(초)")]
    [SerializeField] private float measureSeconds = 30f;

    [Tooltip("이 시간(초)보다 긴 프레임은 측정에서 제외 — 에디터 멈춤/컴파일/포커스 아웃으로 생기는 거대 프레임 배제")]
    [SerializeField] private float maxValidFrameSeconds = 0.5f;

    [Header("Output")]
    [Tooltip("CSV 파일명 (Application.persistentDataPath에 저장)")]
    [SerializeField] private string csvFileName = "snow_perf.csv";

    [Tooltip("측정 종료 후 컴포넌트 자동 비활성화")]
    [SerializeField] private bool disableWhenDone = true;

    #endregion

    #region State

    // 메인 스레드 프레임 시간(ns) — CPU 병목 검증의 핵심 지표
    private ProfilerRecorder mainThreadRecorder;

    private readonly List<float> frameMs = new();
    private readonly List<float> mainThreadMs = new();

    private float elapsed;
    private bool measuring;
    private bool done;

    #endregion

    #region Lifecycle

    private void OnEnable()
    {
        // "Main Thread" 카운터는 Internal 카테고리에 노출됨 (ns 단위)
        mainThreadRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread");
    }

    private void OnDisable()
    {
        if (mainThreadRecorder.Valid)
            mainThreadRecorder.Dispose();
    }

    private void Update()
    {
        if (done) return;

        float dt = Time.unscaledDeltaTime;

        // 에디터 멈춤·컴파일·포커스 아웃은 한 프레임에 수십 초로 잡힌다.
        // 이런 거대 프레임은 elapsed 누적과 샘플 수집에서 모두 제외해야
        // (1) 측정이 조기 종료되지 않고 (2) max/평균이 오염되지 않는다.
        if (dt > maxValidFrameSeconds)
            return;

        elapsed += dt;

        // 워밍업 구간: 수집하지 않음
        if (elapsed < warmupSeconds)
            return;

        if (!measuring)
        {
            measuring = true;
            Debug.Log($"[PerfProbe] '{runLabel}' 측정 시작 ({measureSeconds}s)");
        }

        frameMs.Add(dt * 1000f);
        if (mainThreadRecorder.Valid)
            mainThreadMs.Add(mainThreadRecorder.LastValue * 1e-6f); // ns → ms

        if (elapsed >= warmupSeconds + measureSeconds)
            Finish();
    }

    #endregion

    #region Measurement

    private void Finish()
    {
        done = true;

        var frame = Summarize(frameMs);
        var main = Summarize(mainThreadMs);
        float avgFps = frame.avg > 0f ? 1000f / frame.avg : 0f;

        string report =
            $"[PerfProbe] ===== '{runLabel}' 결과 ({frameMs.Count} frames) =====\n" +
            $"  Frame time (ms)  avg {frame.avg:F2} | median {frame.median:F2} | p95 {frame.p95:F2} | max {frame.max:F2}\n" +
            $"  Main thread (ms) avg {main.avg:F2} | median {main.median:F2} | p95 {main.p95:F2} | max {main.max:F2}\n" +
            $"  Avg FPS          {avgFps:F1}";
        Debug.Log(report);

        WriteCsv(frame, main, avgFps);

        if (disableWhenDone)
            enabled = false;
    }

    private struct Stats { public float avg, median, p95, max, min; }

    private Stats Summarize(List<float> samples)
    {
        var s = new Stats();
        if (samples.Count == 0) return s;

        var sorted = new List<float>(samples);
        sorted.Sort();

        double sum = 0;
        foreach (float v in sorted) sum += v;

        s.min = sorted[0];
        s.max = sorted[sorted.Count - 1];
        s.avg = (float)(sum / sorted.Count);
        s.median = sorted[sorted.Count / 2];
        s.p95 = sorted[Mathf.Clamp(Mathf.CeilToInt(sorted.Count * 0.95f) - 1, 0, sorted.Count - 1)];
        return s;
    }

    private void WriteCsv(Stats frame, Stats main, float avgFps)
    {
        string path = Path.Combine(Application.persistentDataPath, csvFileName);
        bool newFile = !File.Exists(path);

        var sb = new StringBuilder();
        if (newFile)
            sb.AppendLine("label,frames,duration_s,frame_avg_ms,frame_median_ms,frame_p95_ms,frame_max_ms,avg_fps,main_avg_ms,main_median_ms,main_p95_ms,main_max_ms");

        var c = CultureInfo.InvariantCulture;
        sb.AppendLine(string.Join(",", new[]
        {
            runLabel,
            frameMs.Count.ToString(c),
            measureSeconds.ToString("F0", c),
            frame.avg.ToString("F3", c),
            frame.median.ToString("F3", c),
            frame.p95.ToString("F3", c),
            frame.max.ToString("F3", c),
            avgFps.ToString("F2", c),
            main.avg.ToString("F3", c),
            main.median.ToString("F3", c),
            main.p95.ToString("F3", c),
            main.max.ToString("F3", c),
        }));

        File.AppendAllText(path, sb.ToString());
        Debug.Log($"[PerfProbe] CSV 기록: {path}");
    }

    #endregion
}
