set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-lazyload-bin ^
    -d bin bin-offsetlength ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=Output\ConfigCode\GenLazyBinCode ^
    -x bin.outputDataDir=Output\ConfigData\LazyBin ^
    -x bin-offsetlength.outputDataDir=Output\ConfigData\LazyBinOffset

pause