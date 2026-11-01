# Journal

## 2026-09-14 (Week 1)
- Got Unity 6.6 + Android Build Support installed, phone authorized via adb.
- Built and sideloaded a dev APK successfully, confirmed [Boot] log line.
- Issue: needed to run adb from inside the platform-tools folder / correct project directory to avoid path errors.

## Week 2 - Profiler capture (frame 545, active gameplay)
- CPU main thread time (typical frame): ~6.15ms
- SetPass calls: 20
- Triangles: 1.7k
- GC allocated in frame: 51 B (3 allocations)

## Week 2 Lab B - Lifecycle checks
- Home: it pauses
- Pull the notification shade down and up: it pauses
- Screen off with the power button, back on: it pauses
- Force stop from Settings, relaunch: it pauses

## Week 3 Lab A - Rendering budgets and profiling on device

Worst case profiled: SampleScene with the 200-cube stress test (StressCubes), 32 s capture, 1,939 frames (2026-11-01). Capture: docs/CA2/baseline/w03-profile.data

### Part B - CPU capture
- Worst-frame main-thread time: 20.91 ms (frame 198)
- Tallest marker under PlayerLoop: PostLateUpdate.FinishFrameRendering (17.77 ms), mostly Gfx.WaitForPresentOnGfxThread (9.90 ms) and RenderPipelineManager.DoRenderLoop_Internal (7.46 ms)
- Top three Hierarchy entries under PlayerLoop (Time ms / self time):
  1. PostLateUpdate.FinishFrameRendering - 17.77 ms / 0.26 ms self
  2. PostLateUpdate.ProfilerEndFrame - 0.91 ms / 0.00 ms self
  3. PostLateUpdate.UpdateAllRenderers - 0.27 ms / 0.00 ms self
- Biggest self time (Inverted Hierarchy): Semaphore.WaitForSignal 9.89 ms (waiting for present), WaitForJobGroupID 0.77 ms, Inl_On Record Render Graph 0.71 ms
- GC.Collect in capture: no. GarbageCollector line stays at ~0.01 ms for the whole capture; only GC.Alloc (3 calls, 51 B per frame)

### Part C - Rendering and Memory
- SetPass calls: 26 | Batches: 26 | Triangles: 4.5k | Vertices: 8.5k
- Gfx.WaitForPresentOnGfxThread: 9.90 ms (worst frame)
- Total Reserved: 0.60 GB tracked reserved (194.5 MB in use; total 384.2 MB) | GC Allocated in Frame: 51 B
- Managed heap: 3.2 MB in use / 3.7 MB reserved
- Textures: 39.9 MB (105) | Meshes: 0.09 MB (8) | Audio: 1.1 MB
- Three largest categories: Other/untyped native 79.9 MB in use (501.8 MB reserved), Graphics & Graphics Driver 70.0 MB, Profiler 40.2 MB
- Largest single asset: not available. In Unity 6.6 the Detailed view / Take Sample needs the Memory Profiler package, which is not installed. Textures (39.9 MB) are the largest object category.
- Profiler summary: 0% of CPU and GPU frames over the 60 fps target

### Part D - RenderScaleProbe
- Full scale (0.8): main thread ~15.6 ms (15.11 / 15.42 / 16.29), Gfx.WaitForPresentOnGfxThread ~7.7 ms (6.92 / 8.09 / 8.22)
- Half scale (0.4): main thread ~17.3 ms (18.51 / 17.44 / 15.83), Gfx.WaitForPresentOnGfxThread ~7.5 ms (8.77 / 7.72 / 5.96)
- Probe confirmed: the Frame Debugger render target after the tap was 432x936 (0.4 of 1080x2340)
- Verdict: CPU-bound. Frame time did not drop when render scale was halved. Note: frames are capped at 60 fps (targetFrameRate = 60), so both CPU and GPU are inside budget right now.
- Candidate fix: give StressCubes a shared URP/Lit material so the SRP Batcher can batch the cubes (tallest CPU cost is render submission, DoRenderLoop_Internal 7.46 ms)

### Part E - Frame Debugger
- AGI not used: the Galaxy S22 (Exynos 2200, Xclipse 920 GPU) is not on the AGI supported-devices list, so Frame Debugger route.
- Draw calls total: 118 events in the Main Camera render + 1 GUI | Pass with the most draws: (RP 2:0) RenderLoop.Draw (94 draws)
- Those 94 draws use Hidden/InternalErrorShader (the cubes render magenta): CreatePrimitive's default material has no URP shader in the build, so the draws are not SRP-batched. Next biggest: BloomDownsample (10), BloomUpsample (5).
- Screenshot: docs/CA2/baseline/w03-gpu-pass.png
