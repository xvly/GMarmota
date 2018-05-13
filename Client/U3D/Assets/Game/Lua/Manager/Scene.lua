local UI = require("Manager.UI")
local Scene = {}

local assetManager = GStd.Asset.AssetManager
local loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode

local function OnLoadLevel()
end

function Scene.Change(id)
    print_log("start chagne scene ", id)

    UI.Open("loading")

    -- change
    assetManager.LoadLevel("scenes/map/linshan01", "LinShan01_Main", loadSceneMode.Single, function()
        UI.Close("loading")
    end ,
    function(progress)
        print_log("!! progress ", progress)
    end)

    -- coroutine.start(assetManager.LoadLevel, "scenes/map/linshan01_main", "LinShan01_Main", loadSceneMode.Single)

    -- 
end

function Scene.LoadLevel()

end

return Scene