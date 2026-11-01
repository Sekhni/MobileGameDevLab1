# Performance Baseline Sheet

One row per build; add a row whenever something meaningful changes.

| Field | Row 1 |
|-------|-------|
| Date, commit, versionName / versionCode, Release or Dev, Unity version | 2026-11-01, 8f7231a + Lab B changes, 0.1.1 / 3, Release (frame times, PSS, cold start, APK) + Dev (GC, Total Reserved, rendering), Unity 6000.6.0f1 |
| Device model, Android version, SoC / GPU, graphics API, refresh rate | Samsung Galaxy S22 SM-S901B, Android 16 (API 36), Exynos 2200 (s5e9925) / Xclipse 920, Vulkan, 120 Hz panel (Adaptive), game runs at 60 Hz |
| Target fps | 60 (targetFrameRate 60, vSyncCount 0, Optimized Frame Pacing on) |
| Menu: avg ms / p99 ms | 16.68 / 16.74 |
| Steady gameplay: avg ms / p99 ms | 16.68 / 16.73 |
| Worst case: avg ms / p99 ms | 16.69 / 16.73 (200-cube stress test on) |
| GC allocated per frame (bytes), allocating markers | 51 B (3 x 17 B): NativeInputSystem.NotifyBeforeUpdate() in PreUpdate.NewInputUpdate and FixedUpdate.NewInputFixedUpdate; FrameEvents.NewInputBeforeRenderUpdate in PostLateUpdate.PlayerSendFrameStarted (Input System). No GC.Collect in 22 s |
| Peak Total Reserved (MB) / TOTAL PSS (MB) | 342.4 (Total Committed, Dev) / 306.2 (313,503 kB, Release after ~4 min) |
| Worst case: SetPass / batches / triangles | 24 / 24 / 3.4k |
| Cold start ms (median of 3), first interactive s | 142 ms (169 / 140 / 142), ~2.5 s |
| APK size (MB) | 41.55 (43,572,837 bytes) |
| Thermal delta, throttling (Week 7) | |
| Load time (Week 5) | |

Evidence: [w03-sampler-log.png](w03-sampler-log.png), journal entry "Week 3 Lab B" in [/docs/journal.md](../../journal.md).
