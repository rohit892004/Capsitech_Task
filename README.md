# 🧩 Grid Puzzle Challenge

A polished, grid-based mobile puzzle game built with **Unity (C#)** — featuring swipe-based controls, deterministic undo/redo history, and clean separation between game logic and UI rendering.

---

## 📱 Demo & Download

| Resource | Link |
|----------|------|
| 🔗 GitHub Repository | [rohit892004/Capsitech_Task](https://github.com/rohit892004/Capsitech_Task) |
| 📦 APK Download | [Google Drive](https://drive.google.com/drive/folders/1eSFtPwtauxYj6KeF_l0MymUUzjE9doAr?usp=sharing) |

---

## 🎮 Core Features

- **N × M Grid Simulation** — logical grid model fully decoupled from the rendering layer
- **Swipe-Based Input** — gesture detection supporting Up, Down, Left, Right directions
- **Undo / Move History** — deterministic state stack; players can reverse any move
- **HUD Display** — live score, remaining moves, and win/lose overlay states
- **Extra Gameplay Hook** — power-ups and special tile mechanics for added depth

---

## 🏗️ System Architecture Map

The app follows a strict **3-layer architecture** ensuring separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│                        UI LAYER                         │
│          (Unity UI Toolkit / uGUI — rendering only)     │
│                                                         │
│  ┌─────────────┐  ┌────────────┐  ┌────────┐  ┌──────┐ │
│  │  GameScreen │→ │  GridView  │  │ HUDBar │  │Overlay│ │
│  │ Root canvas │  │Tile render │  │Score/  │  │Win/  │ │
│  └─────────────┘  └────────────┘  │Moves   │  │Lose  │ │
│                                   └────────┘  └──────┘ │
└──────────────────────┬──────────────────────────────────┘
                       │  State events (no direct UI calls)
┌──────────────────────▼──────────────────────────────────┐
│                   GAME LOGIC LAYER                      │
│        (Pure C# — no UnityEngine.UI dependencies)       │
│                                                         │
│  ┌─────────────────┐  ┌────────────┐  ┌─────────────┐  │
│  │ InputController │→ │ GameEngine │→ │ GridManager │  │
│  │ Swipe→Direction │  │Orchestrator│  │N×M operations│ │
│  └─────────────────┘  └─────┬──────┘  └─────────────┘  │
│                             │                           │
│                    ┌────────▼────────┐                  │
│                    │ HistoryManager  │                  │
│                    │   Undo stack    │                  │
│                    └─────────────────┘                  │
└──────────────────────┬──────────────────────────────────┘
                       │  Read immutable models
┌──────────────────────▼──────────────────────────────────┐
│                     DATA LAYER                          │
│            (Immutable C# models — no side effects)      │
│                                                         │
│   GridState   │   TileModel   │  MoveRecord  │GameConfig│
└─────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer | Responsibility | Key Rule |
|-------|---------------|----------|
| **UI Layer** | Render state visually | No business logic |
| **Game Logic Layer** | All game rules & transitions | No UnityEngine.UI imports |
| **Data Layer** | Immutable data models | No side effects |

---

## 🔄 Functional Code Flow

Complete data pipeline from user input to UI update:

```
User Gesture (Swipe)
        │
        ▼  Input.touch / pointer events
┌───────────────────┐
│  InputController  │  ← Validates gesture, emits Direction enum
└────────┬──────────┘
         │  Direction event
         ▼
┌───────────────────┐         ┌─────────────────┐
│    GameEngine     │────────▶│  HistoryManager │
│   Orchestrator    │  push   │  Undo stack     │
└────────┬──────────┘         └─────────────────┘
         │  move(direction)
         ▼
┌───────────────────┐
│   GridManager     │  ← Computes new GridState (pure function)
│  N×M operations   │
└────────┬──────────┘
         │  emit GridState
         ▼
┌───────────────────┐
│   EventBus /      │  ← Broadcasts immutable state to all listeners
│   ScriptableObj   │
└────────┬──────────┘
         │  OnStateChanged()
         ▼
┌───────────────────┐
│  UI Rendering     │  ← GridView, HUDBar, Overlay re-render
│  (Unity uGUI)     │
└───────────────────┘

        ↑
        │  Undo: pop stack → restore previous GridState
        └──────── HistoryManager ──────────
```

### Flow Description

1. **User Gesture** — Player swipes on screen; Unity's touch/pointer system captures the raw input.
2. **InputController** — Translates gesture into a typed `Direction` enum; filters invalid inputs.
3. **GameEngine** — Central orchestrator; dispatches the move to `GridManager` and records it in `HistoryManager`.
4. **GridManager** — Executes the grid transition algorithm, returning a new immutable `GridState`.
5. **EventBus / ScriptableObject** — Receives the new state and broadcasts it to all subscribed UI listeners.
6. **UI Rendering** — Unity uGUI components rebuild reactively; `GridView`, `HUDBar`, and Overlay update independently.

---

## 📂 Project Structure

```
Assets/
├── Scripts/
│   ├── Data/
│   │   ├── GridState.cs          # Immutable board state
│   │   ├── TileModel.cs          # Single tile data
│   │   ├── MoveRecord.cs         # Undo history entry
│   │   └── GameConfig.cs         # Grid size, rules config
│   ├── Logic/
│   │   ├── InputController.cs    # Gesture → Direction
│   │   ├── GameEngine.cs         # Orchestration layer
│   │   ├── GridManager.cs        # N×M grid algorithms
│   │   └── HistoryManager.cs     # Undo/redo stack
│   ├── UI/
│   │   ├── GameScreen.cs         # Root canvas controller
│   │   ├── GridView.cs           # Tile grid renderer
│   │   ├── HUDBar.cs             # Score / moves display
│   │   └── OverlayManager.cs     # Win / lose overlays
│   └── Core/
│       └── EventBus.cs           # Decoupled state broadcaster
├── Prefabs/
├── Scenes/
└── Resources/
```

---

## ⚙️ Tech Stack

| Concern | Technology |
|---------|-----------|
| Engine | Unity (2022 LTS) |
| Language | C# |
| Architecture | Clean Architecture (3-layer) |
| UI System | Unity uGUI / UI Toolkit |
| Version Control | Git (semantic commits) |

---

## 🚀 Getting Started

```bash
# Clone the repository
git clone https://github.com/rohit892004/Capsitech_Task.git

# Open in Unity Hub
# Select Unity 2022 LTS or compatible version

# Build APK
# File → Build Settings → Android → Build
```

Or download the prebuilt APK directly from the [Google Drive link](https://drive.google.com/drive/folders/1eSFtPwtauxYj6KeF_l0MymUUzjE9doAr?usp=sharing).

---

## 📝 Commit Convention

This project follows semantic commit prefixes:

```
feat:     New feature
fix:      Bug fix
refactor: Code restructure (no behaviour change)
docs:     Documentation update
test:     Test additions or changes
chore:    Build / config changes
```

---

## 📄 License

This project was developed as part of a technical assessment for Capsitech IT Services.
