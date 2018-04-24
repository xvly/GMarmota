local Config = {}

local function _GetData(id)
    print_log("!! get data ", id)
end

local mt = { __index = _GetData }
setmetatable(Config, mt)

return Config