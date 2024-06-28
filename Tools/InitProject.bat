set WORKSPACE=..\Client
set ConfigDataPath=%WORKSPACE%\Assets\StreamingAssets
set ConfigCodePath=%WORKSPACE%\Assets\Scripts\Generated
set Config_WorkSpace=..\Config\DataTables\Output


cd %ConfigCodePath%
rmdir ConfigCode
mklink /D ConfigCode ..\..\..\%Config_WorkSpace%\ConfigCode

cd ../../StreamingAssets
rmdir ConfigData
mklink /D ConfigData ..\..\%Config_WorkSpace%\ConfigData

pause