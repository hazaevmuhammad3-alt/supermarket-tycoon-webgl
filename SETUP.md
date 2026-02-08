# Quick Setup Guide - Supermarket Tycoon MVP

## Essential Setup Steps (Do This First!)

### 1. Open Project in Unity
- Open Unity Hub
- Click "Add" > Select this project folder
- Open with Unity 6000.3.2f1 or later

### 2. Assign Prefab References (REQUIRED!)

**GridManager:**
1. Select `GridManager` in Hierarchy
2. In Inspector, assign:
   - Shelf Prefab → `Assets/Prefabs/Shelves/ShelfPrefab`
   - Fridge Prefab → `Assets/Prefabs/Shelves/FridgePrefab`
   - Checkout Prefab → `Assets/Prefabs/Checkout/CheckoutPrefab`

**CustomerSpawner:**
1. Select `CustomerSpawner` in Hierarchy
2. In Inspector, assign:
   - Customer Prefab → `Assets/Prefabs/Customer/CustomerPrefab`

### 3. Test in Play Mode
- Press Play button
- Customers should spawn every 5 seconds
- Press 'B' to open build menu
- Camera controls: WASD to move, Scroll to zoom

## WebGL Build Steps

### Configure Build Settings
1. `File > Build Settings`
2. Select `WebGL` platform
3. Click `Switch Platform` (wait for processing)
4. `Add Open Scenes` (MainScene should be added)
5. Click `Build` and choose output folder

### Test Build Locally
```bash
# Using Python
cd Build/WebGL
python -m http.server 8000
# Open: http://localhost:8000

# Using Node.js
npm install -g http-server
cd Build/WebGL
http-server
# Open: http://localhost:8080
```

## Quick Reference

### Controls
- **B** - Build Menu
- **S** - Shop Menu  
- **ESC** - Close Menus
- **WASD** - Pan Camera
- **Scroll** - Zoom
- **Left Click** - Place/Interact
- **Right Click** - Cancel

### Starting Values
- Money: $5,000
- Shelf Cost: $100
- Fridge Cost: $200
- Checkout Cost: $150

### Scene Objects
All managers are in the scene:
- GameManager
- GridManager
- FinanceManager
- InventoryManager
- CustomerSpawner
- UIManager

### Products Available
- Bread ($2.50 / $5.00)
- Milk ($3.00 / $6.00)
- Cheese ($4.00 / $8.00)
- Juice ($2.00 / $4.50)
- Soda ($1.50 / $3.00)

Format: (Purchase Price / Sell Price)

## Troubleshooting

**No customers spawning?**
- Check CustomerPrefab is assigned in CustomerSpawner

**Can't place objects?**
- Check prefabs are assigned in GridManager
- Ensure you have enough money
- Press 'B' to open build menu first

**Grid not visible?**
- Select GridManager
- Enable "Show Grid" in Inspector

## Next Steps
1. Assign all prefab references (see step 2 above)
2. Press Play to test
3. Try placing shelves with 'B' key
4. Watch customers shop and checkout
5. Build for WebGL when ready
