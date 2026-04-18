# Copilot Instructions

## Read Order

- Default: use this file only.
- If a task needs architecture, business meaning, naming rationale, or restart handoff context, read `docs/assistant/project-context.md`.
- If a task needs batch validation or smoke test commands, read `docs/assistant/validation-skill.md`.
- If a task is specifically about the local root rename session, read `docs/root-rename-session-handoff.md`.

## Project Snapshot

- Unity version: `2023.2.22f1`
- Project name: `Oboro` / `朧`
- Old project name: `SDFSample`
- Package path: `Assets/com.onovich.oboro`
- Runtime assembly: `Onovich.Oboro`
- Sample assembly: `Onovich.Oboro.Sample`
- Git remote: `git@github.com:onovich/Oboro.git`
- Sample scene: `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`

## Architecture Rules

- Keep Runtime and Sample clearly separated.
- Runtime contains reusable contour / field extraction logic and must avoid sample-specific assumptions.
- Sample contains presentation, interaction, tuning constants, and demo-only convenience behavior.
- `OboroSampleEntry` is the actual sample runtime component.
- Sample scenes are expected to attach `OboroSampleEntry` explicitly; do not reintroduce auto-bootstrap behavior unless product requirements change.

## Coding Style

- Prefer small focused classes with single responsibility.
- Prefer straightforward procedural flow over clever abstractions.
- Match existing naming: PascalCase for types/methods, camelCase for fields/locals.
- Keep methods short and readable; extract helpers when responsibilities diverge.
- Avoid adding unnecessary frameworks, editor tooling, or configuration layers.
- Preserve current namespace split: `Onovich.Oboro`, `Onovich.Oboro.Sample`, `Onovich.Oboro.*.Inside`.

## Product Intent

- Oboro is a reusable Unity SDF contour runtime plus a sample showing blurred, fused, moonlight-like contour aesthetics.
- Sample is meant to be testable from an explicit scene setup without hidden startup helpers.
- Do not add automatic bootstrap layers that interfere with upstream user tests.

## Current Status

- Rename from `SDFSample` to `Oboro` is already completed.
- README, package naming, assembly naming, and remote URL were updated before this session.
- Root path rename to `D:/UnityProjects/Oboro` has already been validated.
- Batchmode Unity compile under the new path was rechecked successfully on `2026-04-18`.
- Most remaining old-path references are expected only in Unity-generated caches, logs, or local layouts.
- `OboroSampleBootstrap` has been removed; the sample entry is now scene-owned.
