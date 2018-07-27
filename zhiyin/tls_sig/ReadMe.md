tls_sig文件夹为腾讯IM聊天相关文件

64位服务器发布：

	sigcheck64.dll 复制到服务器的 C:\Windows\SysWOW64
	连接池的【高级设置】》【启用32位应用程序】的值必须为False

32位服务器发布：

	sigcheck32.dll 复制到服务器的 C:\Windows\System32
	修改代码：~/App_Code/tls_sig/TLS_SIG.cs 106行 public const string DllPath = @"C:\Windows\System32\sigcheck32.dll";
	连接池的【高级设置】》【启用32位应用程序】的值必须为True