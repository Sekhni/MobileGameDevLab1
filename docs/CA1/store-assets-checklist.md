# Store-asset checklist (CA1): Pixel Sprint

Nothing is uploaded this semester; this is the plan. Specs checked on 2026-11-04 against Play Console Help, "Add preview assets to showcase your app" (https://support.google.com/googleplay/android-developer/answer/9866151). Re-check on the day of submission.

| Asset | Spec | Status | Notes |
|---|---|---|---|
| App icon | 512 x 512, 32-bit PNG with alpha, up to 1024 KB | planned | No rounded corners or drop shadow (Play masks it); no badges or text suggesting ranking, price or category |
| Feature graphic | 1024 x 500, JPEG or 24-bit PNG, no alpha | planned | Shown above a listing video and in game browse rows; keep the key art centred, no small text near the edges |
| Phone screenshots | 2 to 8 per device type; JPEG or 24-bit PNG, no alpha; longest side 320 to 3840 px and at most 2x the shortest side | planned | For game promotion formats Play recommends 3+ portrait shots at 9:16, minimum 1080 x 1920. Gameplay first |
| Preview video (optional) | YouTube URL | not planned | Recommended for games; out of scope for CA1 |

**Spec note.** The lab sheet lists "each side 1080 to 7680 px, 16:9 or 9:16" for phone screenshots; on the current Play page that wording belongs to large-screen (tablet/Chromebook) screenshots. The phone rule is 320 to 3840 px with a 2:1 maximum ratio. This matters for my phone: a raw S22 capture is 1080 x 2340 (2.17:1), which Play would reject, so store shots are captured or cropped to **1080 x 1920 (9:16)**.

## Screenshot plan (portrait, 1080 x 1920)

| Shot | Working caption | What must be visible |
|---|---|---|
| 1 | "Swipe. Dodge. Keep running." | Mid-run on the street track, red blocks ahead, score visible at the top and clear of the camera cut-out, Pause button top-right |
| 2 | "One swipe, one lane" | Lane change in progress (capsule between lanes), nothing overlapping the status bar or gesture bar |
| 3 | "Faster every second" | Late in a run with blocks close together (speed-up visible); HUD inside the safe area |
| 4 | "Pause any time" | Pause menu over the game: "Paused", Resume, Settings; game visible behind the dim overlay |
| 5 | "Play it your way" | Settings panel with text size set to Large and the Haptics toggle visible, showing the accessibility options |
| 6 | "Beat your best" | Crash / run-over screen with the score, the saved best score and "tap to play again" |

Rules for every shot: release build only (no Probe or Stress debug buttons, so the Stress button is hidden or removed before capture), no notifications in the status bar, text size Normal unless the shot is about text size, real gameplay rather than mock-ups.

## Where each asset comes from (Week 10)

- **Screenshots:** in-game capture from the release build on the S22 (`adb shell screencap -p`), cropped from 1080 x 2340 to 1080 x 1920 around the play area; or the Unity Game view at 1080 x 1920 with Unity Recorder, then exported as 24-bit PNG.
- **App icon:** from the MDA key art (the Pixel Sprint runner dodging a red block on the street), drawn at 1024 x 1024 and exported at 512 x 512 as a 32-bit PNG under 1024 KB; the same art feeds the Android adaptive launcher icon in Player Settings.
- **Feature graphic:** the same key art on a 1024 x 500 canvas with the "Pixel Sprint" title, exported as JPEG or 24-bit PNG with no alpha.
