local scene = require("Manager.Scene")

local Game = {}

local loginSceneId = 1001
local function EnterLogin()
    scene.Change(loginSceneId)
end

function Game.Enter()
    EnterLogin()
end

return Game