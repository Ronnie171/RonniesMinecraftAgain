using UnityEngine;

namespace TestInterfaces {
    public static class Assets {
        public readonly struct SceneInfo {
            public readonly string name;
            public readonly string path;
        }

        public const string emailPattern = @"^[\w@.]+uni-bayreuth\.de$";
        public const string elearningPattern = @"^(s\d+\w+)|(bt\d+)@uni-bayreuth\.de$";

        public const string avatarPrefab = "Assets/Prefabs/Avatar.prefab";
        public const string floorPrefab = "Assets/Prefabs/Floor.prefab";
        public const string blockPrefab = "Assets/Prefabs/Block.prefab";
        public const string userInterfacePrefab = "Assets/Prefabs/UserInterface.prefab";
        public const string levelPrefab = "Assets/Prefabs/Level.prefab";
        public const string gameManagerPrefab = "Assets/Prefabs/GameManager.prefab";

        public const float avatarHeight = 1.8f;
        public const float avatarRadius = 0.3f;
        public const float avatarEyeHeight = 1.6f;
        public const float avatarSpeed = 5f;
        public static readonly Vector3 avatarSpawnPoint = new Vector3(8, 2, 8);

        public const string stoneBlockPrefab = "Assets/Prefabs/Blocks/Block_Stone.prefab";
        public const string sandBlockPrefab = "Assets/Prefabs/Blocks/Block_Sand.prefab";
        public const string dirtBlockPrefab = "Assets/Prefabs/Blocks/Block_Dirt.prefab";
        public const string grassBlockPrefab = "Assets/Prefabs/Blocks/Block_Grass.prefab";
        public const string glassBlockPrefab = "Assets/Prefabs/Blocks/Block_Glass.prefab";
        public const string leavesBlockPrefab = "Assets/Prefabs/Blocks/Block_Leaves.prefab";
        public const string gravelBlockPrefab = "Assets/Prefabs/Blocks/Block_Gravel.prefab";
        public const string logBlockPrefab = "Assets/Prefabs/Blocks/Block_Log.prefab";
        public const string woodenPlankBlockPrefab = "Assets/Prefabs/Blocks/Block_WoodenPlank.prefab";
        public const string cobblestoneBlockPrefab = "Assets/Prefabs/Blocks/Block_Cobblestone.prefab";

        public const string playerControlsAsset = "Assets/Scripts/PlayerControls.inputactions";
        public const string playerControlsScript = "Assets/Scripts/PlayerControls.cs";

        public const string playerActionMap = "Avatar";

        public const string playerMoveAction = "Move";
        public const string playerJumpAction = "Jump";
        public const string playerLookAction = "Look";
        public const string playerBuildBlockAction = "BuildBlock";
        public const string playerDestroyBlockAction = "DestroyBlock";

        public const string uiActionMap = "UI";

        public const string uiHotkey1Action = "SelectHotkey1";
        public const string uiHotkey2Action = "SelectHotkey2";
        public const string uiHotkey3Action = "SelectHotkey3";
        public const string uiHotkey4Action = "SelectHotkey4";
        public const string uiHotkey5Action = "SelectHotkey5";
        public const string uiHotkey6Action = "SelectHotkey6";
        public const string uiHotkey7Action = "SelectHotkey7";
        public const string uiHotkey8Action = "SelectHotkey8";
        public const string uiHotkey9Action = "SelectHotkey9";
        public const string uiHotkeyMouseAction = "SelectHotkeyMouse";
        public const string uiTogglePauseAction = "TogglePause";

        public const string emptyTag = "Untagged";
        public const string avatarTag = "Player";
        public const string cameraTag = "MainCamera";
        public const string blockTag = "Block";
        public const string hudTag = "HUD";

        public const string restartButtonText = "Restart";

        public const string scoreDisplayName = "ScoreDisplay";
        public const string gameOverName = "GameOver(Clone)";
        public const string generatorName = "LevelGenerator";

        public const int userInterfaceLayer = 5;

        public const string mainSceneName = "Game";
        public const string mainScenePath = "Assets/Scenes/Game.unity";

        public static readonly (string name, string path) mainScene = (mainSceneName, mainScenePath);

        public const string terrainMaterial = "Assets/Art/Materials/Terrain.mat";
        public const string terrainTexture = "Assets/Art/Textures/terrain.png";
        public const string terrainShader = "Assets/Art/Shaders/BlockTile.shadergraph";

        public const string guiSprites = "Assets/Art/Textures/gui.png";
        public const string terrainSprites = "Assets/Art/Textures/terrain_sprites.png";

        public const string defaultCubeCreator = "Assets/Art/Models/DefaultCubeCreator.asset";
        public const string defaultCubeMesh = "Assets/Art/Models/DefaultCube.asset";
        public const string topSideBottomCubeCreator = "Assets/Art/Models/TopSideBottomCubeCreator.asset";
        public const string topSideBottomCubeMesh = "Assets/Art/Models/TopSideBottomCube.asset";

        public const string alphaClipProperty = "_AlphaClip";
        public const string textureIdProperty = "_Texture_ID";
    }
}
