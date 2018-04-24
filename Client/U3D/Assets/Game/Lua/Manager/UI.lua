local UI = {}

local assetManager = GStd.Asset.AssetManager
local GameObject = UnityEngine.GameObject

local opened = {}
local cached = {}

local root = GameObject.Find("Main/UI/2D/Canvas")

function UI.Open(name)
    if opened[name] ~= nil then
        print_log(string.format("ui %s already opened", name))
        return
    end

    if cached[name] ~= nil then
        cached[name].gameObject:SetActive(true)
        return
    end

    local prefab = assetManager.LoadObject(string.format("uis/views/%s", name), name, typeof(GameObject))
    assert(prefab, string.format("load object %s failed", name or "nil"))

    local inst = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity)
    inst.transform:SetParent(root.transform, false)
    
    opened = {
        inst = inst
    }    
end

function UI.Close(name)
    if opened[name] == nil then
        print_log(string.format("ui %s not opened", name))
        return
    end

    local data = opened[name]
    data.inst:SetActive(false)
end

function UI.IsOpen()
    return opened[name] ~= nil
end

return UI