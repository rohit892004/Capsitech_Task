# 🧩 Grid Puzzle Challenge

A polished, grid-based mobile puzzle game built with Flutter — featuring swipe-based controls, deterministic undo/redo history, and clean separation between game logic and UI rendering.

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
│         (Flutter widgets — visual rendering only)       │
│                                                         │
│  ┌─────────────┐  ┌────────────┐  ┌────────┐  ┌──────┐ │
│  │ GameScreen  │→ │  GridView  │  │ HUDBar │  │Overlay│ │
│  │ Root widget │  │Tile render │  │Score/  │  │Win/  │ │
│  └─────────────┘  └────────────┘  │Moves   │  │Lose  │ │
│                                   └────────┘  └──────┘ │
└──────────────────────┬──────────────────────────────────┘
                       │ State stream (no direct calls)
┌──────────────────────▼──────────────────────────────────┐
│                   GAME LOGIC LAYER                      │
│          (Pure Dart — zero Flutter imports)             │
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
                       │ Read immutable models
┌──────────────────────▼──────────────────────────────────┐
│                     DATA LAYER                          │
│            (Immutable models — no side effects)         │
│                                                         │
│   GridState   │   TileModel   │  MoveRecord  │GameConfig│
└─────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer | Responsibility | Key Rule |
|-------|---------------|----------|
| **UI Layer** | Render state visually | No business logic |
| **Game Logic Layer** | All game rules & transitions | No Flutter/UI imports |
| **Data Layer** | Immutable data models | No side effects |

---

## 🔄 Functional Code Flow

Complete data pipeline from user input to UI update:

```
User Gesture (Swipe)
        │
        ▼  GestureDetector callback
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
│  StateNotifier /  │  ← Broadcasts immutable state to all listeners
│      Bloc         │
└────────┬──────────┘
         │  rebuild()
         ▼
┌───────────────────┐
│  UI Rendering     │  ← GridView, HUDBar, Overlay re-render
│  Framework        │
└───────────────────┘

        ↑
        │  Undo: pop stack → replay state
        └──────── HistoryManager ──────────
```

### Flow Description

1. **User Gesture** — Player swipes on the grid; `GestureDetector` captures the raw pointer event.
2. **InputController** — Translates raw gesture into a typed `Direction` enum; filters invalid or duplicate inputs.
3. **GameEngine** — Central orchestrator; dispatches the move to `GridManager` and records it in `HistoryManager`.
4. **GridManager** — Executes the grid transition algorithm, returning a new immutable `GridState`.
5. **StateNotifier / Bloc** — Receives the new state and broadcasts it to all subscribed UI listeners.
6. **UI Rendering** — Widgets rebuild reactively; `GridView`, `HUDBar`, and `OverlayManager` update independently.

---

## 📂 Project Structure

```
lib/
├── data/
│   ├── models/
│   │   ├── grid_state.dart       # Immutable board state
│   │   ├── tile_model.dart       # Single tile data
│   │   ├── move_record.dart      # Undo history entry
│   │   └── game_config.dart      # Grid size, rules config
├── logic/
│   ├── input_controller.dart     # Gesture → Direction
│   ├── game_engine.dart          # Orchestration layer
│   ├── grid_manager.dart         # N×M grid algorithms
│   └── history_manager.dart      # Undo/redo stack
├── ui/
│   ├── screens/
│   │   └── game_screen.dart      # Root screen
│   ├── widgets/
│   │   ├── grid_view.dart        # Tile grid renderer
│   │   ├── hud_bar.dart          # Score / moves display
│   │   └── overlay_manager.dart  # Win / lose overlays
└── main.dart
```

---

## ⚙️ Tech Stack

| Concern | Technology |
|---------|-----------|
| Framework | Flutter (Dart) |
| State Management | Riverpod / Bloc |
| Architecture | Clean Architecture (3-layer) |
| Version Control | Git (semantic commits) |

---

## 🚀 Getting Started

```bash
# Clone the repository
git clone https://github.com/rohit892004/Capsitech_Task.git

# Install dependencies
flutter pub get

# Run on connected device / emulator
flutter run

# Build APK
flutter build apk --release
```

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
