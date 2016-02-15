
#r "System.Configuration.dll"

open System
open System.Configuration

let appName = "test.exe"

let testReadAppSettings() =

    let config = ConfigurationManager.OpenExeConfiguration(appName)
    let appSettings = config.AppSettings.Settings
    
    let counter = ref 0
    for o in appSettings do
        let kv = o :?> KeyValueConfigurationElement
        incr counter
        printfn "\n[%d] '%s': \t'%s'" !counter kv.Key kv.Value

let testWriteAppSettings() =
    let config = ConfigurationManager.OpenExeConfiguration(appName)
    
    let key = "lastModified"
    let value = DateTime.Now.ToString()

    let setting = config.AppSettings.Settings.[key]
    if setting = null then
        config.AppSettings.Settings.Add(key,value)
    else
        setting.Value <- value

    config.Save()

// testReadAppSettings()
testWriteAppSettings()