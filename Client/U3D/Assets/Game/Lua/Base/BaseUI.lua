--local ui = require("Manager/UI")

local BaseUI = {}

BaseUI.name = nil
BaseUI.ab = nil
BaseUI.asset = nil

function BaseUI:Listen(msgKey)

end

-- local function Register(self)
--     --ui.Register(self)
-- end

function BaseUI:ctor()
    print_log("!! baseui ctor")
    Register()
end

return BaseUI