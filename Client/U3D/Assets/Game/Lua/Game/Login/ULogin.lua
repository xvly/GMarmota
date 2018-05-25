local scene = require("Manager.Scene")

local ui = require("Manager.UI")
local ULogin = ui.Register("login")

ULogin.name = "login"
ULogin.ab = "uis/views/login"
ULogin.asset = "Login"

function ULogin.OnOpen(...)
    ULogin:ListenButton("Enter", ULogin.OnEnter)
end

function ULogin.OnEnter()
    scene.Change(1001)
end

return ULogin