local UI = require("Manager.UI")
local Scene = {}

function Scene.Change(id)
    print_log("start chagne scene ", id)

    UI.Open("loading")

    -- change

    -- 
    UI.Close("loading")

    

    print_log("change scene end ", id)
end

return Scene