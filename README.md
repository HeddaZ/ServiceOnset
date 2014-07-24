ServiceOnset - A light and FREE service wrapper
===============================================

ServiceOnset is an utility to help you to run one or more programs as a **windows service**. Is it cool?
The most typical usage is for [Node.js], [COW], regular jobs and so on.
ServiceOnset is a wrapper program implemented as a windows service. So it enables some of foreground applications started before login.

Prerequisites
-------------
Windows operation with [Microsoft .NET Framework 4.0]

Installation
------------
* Clone and build the solution with `VisualStudio` or download the binary package directly.
> InstallUtil.exe  
> log4net.config  
> log4net.dll  
> ServiceOnset.exe  
> ServiceOnset.exe.json  

* Start a command line with Administrator privilege.
* Navigate to the directory of the binary package.
* Run `C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ServiceOnset.exe` to install the service.
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
			"allowWindow": false,
			"enableLog": true
		},
		{
			"name": "mywinTesting",
			"command": "mywin"
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
|command			|string		|Yes		|			|Command (with full path or not). eg.: `ping`|
|arguments			|string		|			|""			|Command arguments. eg.: `www.baidu.com`|
|workingDirectory	|string		|			|Command path, or ServiceOnset path			|Working directory. eg.: `D:\\ServiceOnset\\`|
|runMode			|enum 		|			|"daemon"	|`"daemon"`: Auto-restart the program if it exited<br/>`"launch"`: Launch the program once and let it be<br/>`"interval"`: Restart the program termly by force kill the running process|
|intervalInSeconds	|int		|			|30			|Detecting interval in seconds for current run mode|
|useShellExecute	|bool		|			|false		|Start a process by [UseShellExecute]. Will omit the standard output of a console when the value is `true`|
|allowWindow		|bool		|			|false		|If `true`, will not restrict the wrapped program showing an UI; otherwise, will try to do that by setting "CreateNoWindow=true" or "WindowStyle=Hidden"|

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
[log4net Config]: http://logging.apache.org/log4net/release/config-examples.html
[UseShellExecute]: http://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo.useshellexecute.aspx
