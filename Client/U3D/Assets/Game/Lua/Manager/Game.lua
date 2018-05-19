require("Init.RequireList")

local scene = require("Manager.Scene")

local Game = {}

local function EnterLogin()
    scene.ChangeLogin()
end

function Game.Enter()
    EnterLogin()
end

return Game