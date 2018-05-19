local UI = require("Manager.UI")
local Scene = {}

local assetManager = GStd.Asset.AssetManager
local loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode

local loadingData = nil

local function OnLoadLevel()
    UI.Close("loading")

    if loadingData.OnEnter then
        loadingData.OnEnter()
    end

    loadingData = nil
end

local function OnLoadLevelProgress(progress)
    print_log("!! progress ", progress)

    if loadingData.OnProgress then
        loadingData.OnProgress()
    end
end

local function LoadLevel(ab, asset)
    UI.Open("loading")

    if loadingData.OnLoadLevel then
        loadingData.OnLoadLevel()
    end

    -- change
    assetManager.LoadLevel(
        ab, 
        asset, 
        loadSceneMode.Single, 
        OnLoadLevel,
        OnLoadLevelProgress)
end

local mapConfigs = {}

function Scene.Change(id)
    print_log("start chagne scene ", id)

    local config = require("Config/Scene/"..id)
    mapConfigs[id] = config

    LoadLevel(config.ab, config.asset)
end

function Scene.ChangeLogin()
    loadingData = require("Game/Login/SLogin")
    LoadLevel("scenes/map/empty", "Empty")
end

return Scene