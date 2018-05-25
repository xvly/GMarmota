local Config = {}

local function _GetData(id)
    print("!! get data ", id)
end

local mt = { __index = _GetData }
setmetatable(Config, mt)

return Config