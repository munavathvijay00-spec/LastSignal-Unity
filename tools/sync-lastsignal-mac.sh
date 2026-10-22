#!/bin/bash
# LAST SIGNAL — update Unity scripts from GitHub without replacing Unity project settings.
set -euo pipefail

REPO_ZIP="https://github.com/munavathvijay00-spec/LastSignal-Unity/archive/refs/heads/main.zip"
echo "=== LAST SIGNAL | Unity Code Sync ==="
echo "Close Unity or stop Play mode before syncing."
echo
read -r -p "Full path to your Unity project folder (drag folder here, then Enter): " PROJECT
PROJECT="${PROJECT%/}"
# Finder drag/drop may add escaped spaces or wrapping quotes.
PROJECT="${PROJECT#\"}"; PROJECT="${PROJECT%\"}"
PROJECT="${PROJECT#\'}"; PROJECT="${PROJECT%\'}"
PROJECT="${PROJECT//\\ / }"
if [[ ! -d "$PROJECT/Assets" || ! -d "$PROJECT/Packages" || ! -d "$PROJECT/ProjectSettings" ]]; then
  echo "ERROR: This does not look like a Unity project. Select the folder CONTAINING Assets, Packages and ProjectSettings."
  exit 1
fi
if [[ ! -d "$PROJECT/Assets/_LastSignal" ]]; then
  echo "ERROR: Existing Assets/_LastSignal directory not found. This sync is designed for the project you already imported."
  exit 1
fi

TMP="$(mktemp -d)"
trap 'rm -rf "$TMP"' EXIT
echo "Downloading latest source from GitHub..."
curl --fail --location --silent --show-error "$REPO_ZIP" --output "$TMP/source.zip"
ditto -x -k "$TMP/source.zip" "$TMP"
SRC="$TMP/LastSignal-Unity-main/Assets/_LastSignal"
if [[ ! -d "$SRC/Scripts" || ! -d "$SRC/Editor" ]]; then
  echo "ERROR: Download did not contain the expected scripts."
  exit 1
fi

BACKUP="$PROJECT/LastSignal-CodeBackup-$(date +%Y%m%d-%H%M%S)"
mkdir -p "$BACKUP"
ditto "$PROJECT/Assets/_LastSignal/Scripts" "$BACKUP/Scripts"
ditto "$PROJECT/Assets/_LastSignal/Editor" "$BACKUP/Editor"

echo "Copying only updated gameplay and editor code..."
# Existing scene and Unity project settings are deliberately untouched.
ditto "$SRC/Scripts" "$PROJECT/Assets/_LastSignal/Scripts"
ditto "$SRC/Editor" "$PROJECT/Assets/_LastSignal/Editor"

echo
echo "SUCCESS: Latest LAST SIGNAL scripts synced."
echo "Backup: $BACKUP"
echo "Open Unity and inspect Console for compilation errors."
echo "The generated scene is NOT overwritten. If needed, use Last Signal menus inside Unity."
