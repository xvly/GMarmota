local game = require("Manager.Game")

game.Enter()

-- local function _TestCoroutine(arg1, arg2)
--     print_log("!! 1 2", arg1, arg2)

--     for i=1, 10 do
--         print_log(Time.time, i)
--         coroutine.wait(0.1)
--     end

--     print("current frameCount: "..Time.frameCount)
--     coroutine.step()  
--     print("yield frameCount: "..Time.frameCount)

--     local www = UnityEngine.WWW("http://www.baidu.com")
--     coroutine.www(www)
--     print("!! 2")
-- end

-- coroutine.start(_TestCoroutine, "xxx", "zzz")

-- local UI = require("Manager/UI")
-- local isShow = true

-- local scenes = {{"scenes/map/linshan01", "LinShan01_Main"}, {"scenes/map/"}}

-- local function loop()
--     while true do
--         if isShow then
--             UI.Open("loading")
--         else
--             UI.Close("loading")
--         end

--         isShow = not isShow
--         coroutine.wait(5)
--     end
-- end

-- coroutine.start(loop)