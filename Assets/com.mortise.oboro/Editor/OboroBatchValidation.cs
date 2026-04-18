using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MortiseFrame.Oboro.Editor {

    public static class OboroBatchValidation {

        const string SampleScenePath = "Assets/com.mortise.oboro/Resources_Sample/SampleEntry.unity";
        const double SmokeDurationSeconds = 1.5d;
        const double SmokeTimeoutSeconds = 15d;
        const string SmokePendingKey = "OboroBatchValidation.SmokePending";
        const string SmokeRunningKey = "OboroBatchValidation.SmokeRunning";
        const string SmokeStartTimeKey = "OboroBatchValidation.SmokeStartTime";
        const string SmokeErrorCountKey = "OboroBatchValidation.SmokeErrorCount";
        const string SmokeFirstErrorKey = "OboroBatchValidation.SmokeFirstError";
        const string EnterPlayModeOptionsEnabledKey = "OboroBatchValidation.EnterPlayModeOptionsEnabled";
        const string EnterPlayModeOptionsKey = "OboroBatchValidation.EnterPlayModeOptions";

        static bool callbacksAttached;

        [InitializeOnLoadMethod]
        static void InitializeOnLoad() {
            AttachCallbacksIfNeeded();
        }

        public static void CompileProject() {
            Debug.Log("[OboroBatchValidation] CompileProject invoked.");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Debug.Log("[OboroBatchValidation] Compile passed under batchmode.");
            EditorApplication.Exit(0);
        }

        public static void PlayModeSmokeSampleEntry() {
            if (IsSmokePending() || IsSmokeRunning()) {
                Fail("Smoke test is already running.");
                return;
            }

            if (!System.IO.File.Exists(SampleScenePath)) {
                Fail($"Scene not found: {SampleScenePath}");
                return;
            }

            ResetSmokeState();
            SessionState.SetBool(SmokePendingKey, true);
            AttachCallbacksIfNeeded();
            SaveEnterPlayModeOptions();
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;

            Debug.Log($"[OboroBatchValidation] Opening scene: {SampleScenePath}");
            EditorSceneManager.OpenScene(SampleScenePath, OpenSceneMode.Single);
            Debug.Log("[OboroBatchValidation] Starting PlayMode smoke test.");
            EditorApplication.isPlaying = true;
        }

        static void OnEditorUpdate() {
            if (!IsSmokePending() && !IsSmokeRunning()) {
                return;
            }

            double elapsed = EditorApplication.timeSinceStartup - SessionState.GetFloat(SmokeStartTimeKey, 0f);
            if (IsSmokeRunning() && elapsed > SmokeTimeoutSeconds) {
                Fail($"Smoke test timed out after {SmokeTimeoutSeconds:0.0}s.");
                return;
            }

            if (IsSmokeRunning() && EditorApplication.isPlaying && elapsed >= SmokeDurationSeconds) {
                Debug.Log("[OboroBatchValidation] Smoke duration reached, stopping PlayMode.");
                EditorApplication.isPlaying = false;
            }
        }

        static void OnPlayModeStateChanged(PlayModeStateChange stateChange) {
            if (!IsSmokePending() && !IsSmokeRunning()) {
                return;
            }

            if (stateChange == PlayModeStateChange.EnteredPlayMode) {
                SessionState.SetBool(SmokePendingKey, false);
                SessionState.SetBool(SmokeRunningKey, true);
                SessionState.SetFloat(SmokeStartTimeKey, (float)EditorApplication.timeSinceStartup);
                return;
            }

            if (stateChange != PlayModeStateChange.EnteredEditMode || !IsSmokeRunning()) {
                return;
            }

            int smokeErrorCount = SessionState.GetInt(SmokeErrorCountKey, 0);
            string firstErrorMessage = SessionState.GetString(SmokeFirstErrorKey, string.Empty);
            if (smokeErrorCount > 0) {
                Fail($"Smoke test captured {smokeErrorCount} error log(s). First: {firstErrorMessage}");
                return;
            }

            Succeed("[OboroBatchValidation] PlayMode smoke passed for SampleEntry scene.");
        }

        static void OnLogMessageReceived(string condition, string stackTrace, LogType type) {
            if (!IsSmokePending() && !IsSmokeRunning()) {
                return;
            }

            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) {
                return;
            }

            int smokeErrorCount = SessionState.GetInt(SmokeErrorCountKey, 0) + 1;
            SessionState.SetInt(SmokeErrorCountKey, smokeErrorCount);

            string firstErrorMessage = SessionState.GetString(SmokeFirstErrorKey, string.Empty);
            if (string.IsNullOrEmpty(firstErrorMessage)) {
                firstErrorMessage = string.IsNullOrEmpty(stackTrace) ? condition : $"{condition}\n{stackTrace}";
                SessionState.SetString(SmokeFirstErrorKey, firstErrorMessage);
            }
        }

        static void AttachCallbacksIfNeeded() {
            if (callbacksAttached) {
                return;
            }

            Application.logMessageReceived += OnLogMessageReceived;
            EditorApplication.update += OnEditorUpdate;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            callbacksAttached = true;
        }

        static bool IsSmokePending() {
            return SessionState.GetBool(SmokePendingKey, false);
        }

        static bool IsSmokeRunning() {
            return SessionState.GetBool(SmokeRunningKey, false);
        }

        static void ResetSmokeState() {
            SessionState.EraseBool(SmokePendingKey);
            SessionState.EraseBool(SmokeRunningKey);
            SessionState.EraseFloat(SmokeStartTimeKey);
            SessionState.EraseInt(SmokeErrorCountKey);
            SessionState.EraseString(SmokeFirstErrorKey);
        }

        static void SaveEnterPlayModeOptions() {
            SessionState.SetBool(EnterPlayModeOptionsEnabledKey, EditorSettings.enterPlayModeOptionsEnabled);
            SessionState.SetInt(EnterPlayModeOptionsKey, (int)EditorSettings.enterPlayModeOptions);
        }

        static void RestoreEnterPlayModeOptions() {
            EditorSettings.enterPlayModeOptionsEnabled = SessionState.GetBool(EnterPlayModeOptionsEnabledKey, false);
            EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)SessionState.GetInt(EnterPlayModeOptionsKey, 0);
            SessionState.EraseBool(EnterPlayModeOptionsEnabledKey);
            SessionState.EraseInt(EnterPlayModeOptionsKey);
        }

        static void Succeed(string message) {
            Debug.Log(message);
            Cleanup();
            EditorApplication.Exit(0);
        }

        static void Fail(string message) {
            Debug.LogError($"[OboroBatchValidation] {message}");
            Cleanup();
            EditorApplication.Exit(1);
        }

        static void Cleanup() {
            ResetSmokeState();
            RestoreEnterPlayModeOptions();

            if (EditorApplication.isPlaying) {
                EditorApplication.isPlaying = false;
            }
        }

    }

}