local UI = {}

local AssetManager = GStd.Asset.AssetManager
local GameObject = UnityEngine.GameObject

local opened = {}
local cached = {}

local root = GameObject.Find("Main/UI/2D/Canvas")

local uis = {}

local baseui = require("Base.BaseUI")

function UI.Register(name)
    print_log("ui register ", name)
    local ui = {}
    uis[name] = ui
    setmetatable(ui, {__index=baseui})
    return ui
end

function UI.Open(name)
    local ui = uis[name]
    if ui == nil then
        print_error(string.format("ui %s not register ", name or "nil"))
        return
    end

    if opened[name] ~= nil then
        print_log(string.format("ui %s already opened", name))
        return
    end

    if cached[name] ~= nil then
        local data = cached[name]
        cached[name] = nil
        opened[name] = data
        data.inst:SetActive(true)
        return
    end

    local prefab = AssetManager.LoadAsset(ui.ab, ui.asset, typeof(GameObject))
    assert(prefab, string.format("load object %s failed", name or "nil"))

    local inst = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity)
    inst.transform:SetParent(root.transform, false)
    
    opened = {
        [name] = {
            inst=inst,
        }
    }    
end

function UI.Close(name)
    if opened[name] == nil then
        print_log(string.format("ui %s not opened", name))
        return
    end

    local data = opened[name]
    data.inst:SetActive(false)
    data.time2destroy = Time.realtimeSinceStartup + 10

    opened[name] = nil
    cached[name] = data
end

function UI.IsOpen()
    return opened[name] ~= nil
end

local Time = Time

local function Update()
    local toRemove = {}
    for name, uiData in pairs(cached) do
        if uiData.time2destroy <= Time.realtimeSinceStartup then
            table.insert(toRemove, name)
            GameObject.Destroy(uiData.inst)
        end
    end

    for _, name in ipairs(toRemove) do
        cached[name] = nil
    end
end

local UpdateBeat = UpdateBeat
UpdateBeat:AddListener(UpdateBeat:CreateListener(Update))


return UI