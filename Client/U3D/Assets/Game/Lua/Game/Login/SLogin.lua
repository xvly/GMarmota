local UI = require("Manager.UI")
local SLogin = {}

function SLogin.OnEnter()
    UI.Open("login")
end

return SLogin