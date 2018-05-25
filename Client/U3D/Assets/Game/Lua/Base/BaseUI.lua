--local ui = require("Manager/UI")

local BaseUI = {}

BaseUI.name = nil
BaseUI.ab = nil
BaseUI.asset = nil

local function FindTransform(transform, node)
end

function BaseUI.ListenButton(self, node, event)
    print("!! baseui inst ", self.inst, node, event)

    assert(node, "node can not be nil")
    assert(event, "event can not be nil")

    if not self.rootTransform then
        self.rootTransform = self.inst.transform
    end

    local nodeTransform = self.rootTransform:Find(node)
    print("!! baseui find node ", node, nodeTransform)

    if not nodeTransform then
        error("not find transform ", node)
        return
    end

    print("!! type ", typeof(UnityEngine.UI.Button))

    local button = nodeTransform:GetComponent(typeof(UnityEngine.UI.Button))
    print("!! base ui find button ", button)

    button.onClick:AddListener(event)
end

return BaseUI