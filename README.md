ServiceOnset - A light and FREE service wrapper
===============================================

ServiceOnset is an exciting utility to help you to run **one or more programs** as **single windows service**. Is it cool?
The most typical usage is for [Node.js], [COW], regular jobs and so on.
ServiceOnset is a wrapper program implemented as a windows service. So it enables some of foreground applications started before login.

Prerequisites
-------------
Windows operation with [Microsoft .NET Framework 4.0]

Installation
------------
* Clone and build the solution with `VisualStudio`, or **[DOWNLOAD]** the binary package directly.
> log4net.config  
> log4net.dll  
> ServiceOnset.exe  
> ServiceOnset.exe.json  

* Start a command line with Administrator privilege.
* Navigate to the directory of the binary package.
* Run `C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ServiceOnset.exe` to install the service.
* `Optional.` If you got an error like below ([Issue #3]), please `right-click` ServiceOnset.exe file and click `Unblock` button in the program's property window.
> Exception occurred while initializing the installation:  
> System.IO.FileLoadException: Could not load file or assembly '...\ServiceOnset.exe' or one of its dependencies. Operation is not supported. (Exception from HRESULT: 0x80131515).

* Change the config of ServiceOnset as you want. Refer to [ServiceOnset.exe.json](#config)
* `Optional.` Change the config of log4net if you want assign a dedicated logger for a service. Refer to [log4net Config]

```xml
<log4net>
  <root>
    <appender-ref ref="Default" />
  </root>
  <appender name="Default" type="log4net.Appender.RollingFileAppender">
    <file value="logs/log" />
    <appendToFile value="true" />
    <rollingStyle value="Date" />
    <datePattern value="yyyyMMdd&quot;.log&quot;" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
  </appender>
</log4net>
```
* Open windows services manager and start **ServiceOnset** service. Or directly execute `net start ServiceOnset`.
* Known [Issue #7]. ServiceOnset currently CANNOT support an UI-based program because of the *session 0 isolation* problem on Win2008 or above. Only added `Interop.cs` as a preparation.
* Enjoy ~~

Uninstallation
--------------
1. Stop the service `net stop ServiceOnset` if it is running.
2. Start a command line with Administrator privilege.
3. Navigate to the directory of the binary package.
4. Run `C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u ServiceOnset.exe` to remove the service.
5. Clean up the directory.

<a name="config">ServiceOnset.exe.json</a>
------------------------------------------
#### Sample
```json
{
	"enableLog": true,
	"services": [
		{
			"name": "PingBaidu",
			"command": "ping",
			"arguments": "www.baidu.com",
			"workingDirectory": "",
			"runMode": "interval",
			"intervalInSeconds": 10,
			"useShellExecute": false,
			"killExistingProcess": false,
			"enableLog": true
		},
        {
            "name": "mywin_AppPath",
            "command": "mywin",
            "killExistingProcess": true
        },
        {
			"name": "mywin_RelativePath",
            "command": "test\\mywin.exe",
            "runMode": "interval",
			"killExistingProcess": true
		}
	]
}
```
#### References
|Property			|Value type	|Required	|Default	|Description|
|--------			|------		|-------	|-------	|-----------|
|enableLog			|bool		|			|false		|Determinate if generate logs by `log4net`|
|services			|array		|Yes		|(empty)	|Program definitions hosted by `ServiceOnset`|
|name				|string		|Yes		|			|Program identifier, must be same to the corresponding logger name|
|command			|string		|Yes		|			|Command (with full path, relative path ([Issue #5]) or Windows ENV path). eg.: `ping`|
|arguments			|string		|			|""			|Command arguments. eg.: `www.baidu.com`|
|workingDirectory	|string		|			|Command path, or ServiceOnset path			|Working directory. eg.: `D:\\ServiceOnset\\`|
|runMode			|enum 		|			|"daemon"	|`"daemon"`: Auto-restart the program if it exited<br/>`"launch"`: Launch the program once and let it be<br/>`"interval"`: Restart the program termly by force kill the running process|
|intervalInSeconds	|int		|			|30			|Detecting interval in seconds for current run mode|
|useShellExecute	|bool		|			|false		|Start a process by [UseShellExecute]. Will omit the standard output of a console when the value is `true`|
|killExistingProcess|bool		|			|false		|If `true`, will try to kill the existing process whose file name equals [Command] when initializing the service entry. Here any error will be ignored except logging|

Case sample for [COW]
---------------------
1. Make sure `cow.exe` or `cow-hide.exe` or `cow-taskbar.exe` with `rc.txt` can work.
2. Extract ServiceOnset binary package to COW directory. And config like this:
```json
{
	"enableLog": true,
	"services": [
		{
			"name": "COW",
			"command": "cow.exe"
		}
	]
}
```

-------------------------------
**Contact QQ: 9812152 `@Hedda`**

[Node.js]: http://nodejs.org/
[COW]: https://github.com/cyfdecyf/cow
[Microsoft .NET Framework 4.0]: http://www.microsoft.com/zh-cn/download/details.aspx?id=17718
[DOWNLOAD]: https://github.com/HeddaZ/ServiceOnset/releases
[log4net Config]: http://logging.apache.org/log4net/release/config-examples.html
[UseShellExecute]: http://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo.useshellexecute.aspx
[Issue #3]: https://github.com/HeddaZ/ServiceOnset/issues/3
[Issue #5]: https://github.com/HeddaZ/ServiceOnset/issues/5
[Issue #7]: https://github.com/HeddaZ/ServiceOnset/issues/7