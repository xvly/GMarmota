local game = require("Manager.Game")

function GameUpdate()
end

function GameStop()
end

-- init
    -- check udpate
    -- device
-- enter game
game.Enter()

local function _TestCoroutine()
    print("!! 1")

    for i=1, 10 do
        print_log(Time.time, i)
        coroutine.wait(0.1)
    end

    print("current frameCount: "..Time.frameCount)  
    coroutine.step()  
    print("yield frameCount: "..Time.frameCount)  

    local www = UnityEngine.WWW("http://www.baidu.com")
    coroutine.www(www)
    print("!! 2")
end

coroutine.start(_TestCoroutine)