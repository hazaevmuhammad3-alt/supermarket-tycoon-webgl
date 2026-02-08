# Supermarket Tycoon MVP

A Unity 6000.0.23f1 WebGL game project built with the Built-in Render Pipeline. This is a minimum viable product (MVP) for a supermarket management tycoon game.

## Project Overview

**Unity Version:** 6000.0.23f1  
**Render Pipeline:** Built-in  
**Target Platform:** WebGL  
**Project Type:** 3D Game

## Features

### 1. Grid-Based Placement System
- 20x15 grid for placing objects
- Visual grid overlay (togglable in editor)
- Placement preview with valid/invalid indicators
- Support for multiple object types:
  - Shelves (stores products)
  - Fridges (stores cold products)
  - Checkout counters (customer payment)

### 2. Inventory & Stock Management
- Product database with purchase/sell prices
- Stock purchasing system
- Shelf stocking mechanics
- Visual stock indicators (color-coded)
- Products included:
  - Bread, Milk, Cheese, Juice, Soda

### 3. Customer AI System
- Customer spawning at configurable intervals
- State machine-based behavior:
  - Shopping (browsing shelves)
  - Product selection
  - Moving to checkout
  - Queue management
  - Payment processing
  - Leaving store
- Configurable shopping patterns
- Automatic pathfinding to shelves and checkout

### 4. Finance System
- Starting capital: $5,000
- Revenue tracking from customer purchases
- Expense tracking for purchases and construction
- Real-time money display
- Profit/loss calculation

### 5. Expansion System
- Unlockable expansion tiles
- Cost-based expansion mechanics
- Grid size increases with expansions

### 6. User Interface
- Money display
- Customer count tracker
- Game time display
- Build menu (Press 'B')
- Shop menu (Press 'S')
- Click-to-place system

### 7. Camera Controls
- WASD/Arrow Keys: Pan camera
- Mouse Scroll: Zoom in/out
- Middle Mouse Drag: Pan camera
- Configurable boundaries

## Project Structure

```
Assets/
├── Scenes/
│   └── MainScene.unity          # Main game scene
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs       # Core game loop and state
│   │   └── CameraController.cs  # Camera movement
│   ├── Grid/
│   │   ├── GridManager.cs       # Grid system and placement
│   │   └── ExpansionTile.cs     # Expansion mechanics
│   ├── Inventory/
│   │   ├── InventoryManager.cs  # Stock management
│   │   └── ShelfController.cs   # Shelf behavior
│   ├── Customer/
│   │   ├── CustomerAI.cs        # Customer behavior
│   │   ├── CustomerSpawner.cs   # Customer spawning
│   │   └── CheckoutController.cs # Checkout queue system
│   ├── Finance/
│   │   └── FinanceManager.cs    # Money management
│   └── UI/
│       └── UIManager.cs         # UI control
├── Prefabs/
│   ├── Shelves/
│   │   ├── ShelfPrefab.prefab
│   │   └── FridgePrefab.prefab
│   ├── Checkout/
│   │   └── CheckoutPrefab.prefab
│   └── Customer/
│       └── CustomerPrefab.prefab
└── Materials/
    └── GroundMaterial.mat

ProjectSettings/
├── ProjectSettings.asset        # Build target: WebGL
├── EditorBuildSettings.asset
├── TagManager.asset
├── QualitySettings.asset
└── GraphicsSettings.asset
```

## Setup Instructions

### Prerequisites
- Unity Hub installed
- Unity 6000.0.23f1 (or compatible Unity 6 version)
- WebGL Build Support module installed

### Opening the Project

1. Clone or download this repository
2. Open Unity Hub
3. Click "Add" and select the project folder
4. Open the project with Unity 6000.0.23f1 or later

### First-Time Setup

1. Open the project in Unity
2. Navigate to `Assets/Scenes/MainScene.unity`
3. Ensure the scene hierarchy contains:
   - Main Camera (with CameraController)
   - Directional Light
   - GameManager
   - GridManager
   - CustomerSpawner
   - Ground
   - Canvas (with UIManager)
   - EventSystem

4. **Important:** Assign prefab references in the Inspector:
   - Select `GridManager` in Hierarchy
   - Assign `ShelfPrefab`, `FridgePrefab`, `CheckoutPrefab` from `Assets/Prefabs/`
   - Select `CustomerSpawner` in Hierarchy
   - Assign `CustomerPrefab` from `Assets/Prefabs/Customer/`

### Testing in Play Mode

1. Click the Play button in Unity Editor
2. Use the following controls:
   - **B Key:** Open build menu
   - **S Key:** Open shop menu
   - **WASD/Arrows:** Move camera
   - **Mouse Scroll:** Zoom camera
   - **Left Click:** Place selected object (after selecting from build menu)
   - **Right Click:** Cancel placement
   - **ESC:** Close menus

### Expected Behavior in Play Mode

- Customers should spawn every 5 seconds
- Customers move to shelves, take products, go to checkout
- Money should increase when customers complete purchases
- Grid should be visible (green lines on ground)
- Camera should respond to controls

## WebGL Build Instructions

### Configure Build Settings

1. Go to `File > Build Settings`
2. Select `WebGL` platform
3. Click `Switch Platform` if not already selected
4. Click `Add Open Scenes` to add MainScene
5. Click `Player Settings` and verify:
   - Company Name: SupermarketTycoonStudio
   - Product Name: Supermarket Tycoon MVP
   - WebGL Template: Default
   - Memory Size: 256 MB (minimum)
   - Enable Exceptions: Explicitly Thrown Exceptions Only
   - Compression Format: Gzip (recommended)

### Build Process

1. In Build Settings, click `Build`
2. Choose output folder (e.g., `Build/WebGL`)
3. Wait for build to complete (5-15 minutes depending on hardware)
4. Output will contain:
   - `index.html` - Main entry point
   - `Build/` folder - WebGL assets
   - `TemplateData/` folder - UI assets

### Testing WebGL Build Locally

**Important:** WebGL builds must be served via HTTP server, not opened directly as files.

#### Option 1: Python Simple Server
```bash
cd Build/WebGL
python -m http.server 8000
# Open browser to http://localhost:8000
```

#### Option 2: Node.js http-server
```bash
npm install -g http-server
cd Build/WebGL
http-server
# Open browser to http://localhost:8080
```

#### Option 3: Unity Editor Test
- After building, click "Build and Run" to test in browser

### WebGL Deployment

Upload the entire build folder to a web hosting service:
- **GitHub Pages:** Free, good for demos
- **itch.io:** Game-focused platform
- **Netlify/Vercel:** Easy deployment, free tier
- **Custom hosting:** Any web server with HTTP support

### WebGL Performance Notes

- First load may take 30-60 seconds
- Performance is lower than native builds
- Mobile WebGL support varies by browser
- Recommended browsers: Chrome, Firefox, Edge (latest versions)

## Development Notes

### Adding New Products

Edit `InventoryManager.InitializeProducts()`:
```csharp
availableProducts.Add(new ProductData("NewProduct", purchasePrice, sellPrice));
```

### Adjusting Grid Size

Select `GridManager` in Hierarchy, modify:
- `gridWidth` - Horizontal cells
- `gridHeight` - Vertical cells  
- `cellSize` - Size of each cell

### Changing Customer Behavior

Select `CustomerSpawner`, adjust:
- `spawnInterval` - Time between spawns
- `maxCustomers` - Maximum concurrent customers

Edit `CustomerAI.cs` to modify:
- `moveSpeed` - Movement speed
- `itemsToPurchase` - Number of items per customer

### Modifying Starting Money

Select scene object with `FinanceManager` component, change:
- `currentMoney` - Starting capital

## Known Limitations (MVP)

- No persistent save/load system
- No advanced pathfinding (uses direct movement)
- No employee/staff management
- Limited product variety
- Basic placeholder graphics (primitives)
- No sound effects or music
- No tutorial or onboarding
- No progression/unlocks system
- Single store location only

## Future Enhancement Ideas

- Save/Load system using PlayerPrefs or JSON
- Advanced pathfinding with NavMesh
- Staff hiring and management
- More product categories
- Store upgrades and decorations
- Marketing and advertising system
- Customer satisfaction mechanics
- Competitor stores
- Day/night cycle
- Random events (sales, inspections)
- Achievement system

## Controls Reference

| Input | Action |
|-------|--------|
| B | Toggle Build Menu |
| S | Toggle Shop Menu |
| ESC | Close Menus |
| WASD / Arrow Keys | Pan Camera |
| Mouse Scroll Wheel | Zoom Camera |
| Middle Mouse Drag | Pan Camera (alternate) |
| Left Click | Place Object / Interact |
| Right Click | Cancel Placement |

## Troubleshooting

### Issue: Customers not spawning
**Solution:** Ensure `CustomerPrefab` is assigned in `CustomerSpawner` inspector

### Issue: Can't place objects
**Solution:** 
1. Check prefabs are assigned in `GridManager`
2. Ensure you have enough money
3. Press 'B' to open build menu first
4. Check ground has "Ground" layer assigned

### Issue: WebGL build fails
**Solution:**
1. Ensure WebGL Build Support is installed in Unity Hub
2. Check sufficient disk space (2-3 GB needed)
3. Try building to a different folder
4. Restart Unity and try again

### Issue: Grid not visible in editor
**Solution:** Select `GridManager`, enable `showGrid` in inspector

### Issue: Customers walk through walls
**Solution:** This is expected in MVP - no NavMesh implemented yet

## License

This is a sample/educational project. Feel free to use and modify as needed.

## Credits

**Developed with Unity 6000.0.23f1**  
Built-in Render Pipeline  
WebGL Target Platform
