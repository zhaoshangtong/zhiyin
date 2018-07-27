using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

/// <summary>
/// TLS_SIG 的摘要说明
/// </summary>
public class TLS_SIG
{
    public TLS_SIG()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    string pri_key_path = AppDomain.CurrentDomain.BaseDirectory + (@"tls_sig\private_key");
    string pub_key_path = AppDomain.CurrentDomain.BaseDirectory + (@"tls_sig\public_key");

    public string CreateSig(string identifier)
    {
        UInt32 sdkappid = Convert.ToUInt32(GloabManager.SDKAPPID);
        // 生成 sig 文件
        FileStream f = new FileStream(pri_key_path, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(f);
        byte[] b = new byte[f.Length];
        reader.Read(b, 0, b.Length);
        string pri_key = Encoding.Default.GetString(b);

        StringBuilder sig = new StringBuilder(4096);
        StringBuilder err_msg = new StringBuilder(4096);

        int ret = sigcheck.tls_gen_sig_ex(
            sdkappid,
            identifier,
            sig,
            4096,
            pri_key,
            (UInt32)pri_key.Length,
            err_msg,
            4096);
        if (0 != ret)
        {
            Console.WriteLine("err_msg: " + err_msg);
            return null;
        }

        if (CheckSig(sdkappid, identifier, sig.ToString()))
        {
            return sig.ToString();
        }
        else
        {
            Console.WriteLine("err_msg: sig已生成，但校验 sig 时失败。");
            return null;
        }
    }

    // 校验 sig
    private bool CheckSig(UInt32 sdkappid, string identifier, string sig)
    {
        FileStream f = new FileStream(pub_key_path, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(f);
        byte[] b = new byte[f.Length];
        reader.Read(b, 0, b.Length);
        string pub_key = Encoding.Default.GetString(b);

        StringBuilder err_msg = new StringBuilder(4096);

        UInt32 expire_time = 0;
        UInt32 init_time = 0;
        int ret = sigcheck.tls_vri_sig_ex(
            sig,
            pub_key,
            (UInt32)pub_key.Length,
            sdkappid,
            identifier,
            ref expire_time,
            ref init_time,
            err_msg,
            4096);

        if (0 != ret)
        {
            Console.WriteLine("err_msg: " + err_msg);
            return false;
        }
        return true;
    }
}



class dllpath
{
    // 开发者调用 dll 时请注意项目的平台属性，下面的路径是 demo 创建时使用的，请自己使用予以修改
    // 请使用适当的平台 dll
    //public const string DllPath = System.Web.HttpContext.Current.Server.MapPath(@"~\App_Code\tls_sig\sigcheck.dll");       // 64 位

    //发布

    public const string DllPath = @"C:\Windows\SysWOW64\sigcheck64.dll";

    //#if DEBUG
    //本地调试
    // public const string DllPath = @"C:\Work\Zhiyin\Zhiyin\tls_sig\sigcheck32.dll";//C:\Windows\SysWOW64\
                                                                                        //#endif

    //public const string DllPath = @"C:\Work\services.hifun.me\App_Code\tls_sig\sigcheck64.dll";
    // 如果选择 Any CPU 平台，默认加载 32 位 dll
    //public const string DllPath = @"D:\src\oicq64\tinyid\tls_sig_api\windows\32\lib\libsigcheck\sigcheck.dll";     // 32 位
}

class sigcheck
{
    [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public extern static int tls_gen_sig(
        UInt32 expire,
        string appid3rd,
        UInt32 sdkappid,
        string identifier,
        UInt32 acctype,
        StringBuilder sig,
        UInt32 sig_buff_len,
        string pri_key,
        UInt32 pri_key_len,
        StringBuilder err_msg,
        UInt32 err_msg_buff_len
    );

    [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public extern static int tls_vri_sig(
        string sig,
        string pub_key,
        UInt32 pub_key_len,
        UInt32 acctype,
        string appid3rd,
        UInt32 sdkappid,
        string identifier,
        StringBuilder err_msg,
        UInt32 err_msg_buff_len
    );

    [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public extern static int tls_gen_sig_ex(
        UInt32 sdkappid,
        string identifier,
        StringBuilder sig,
        UInt32 sig_buff_len,
        string pri_key,
        UInt32 pri_key_len,
        StringBuilder err_msg,
        UInt32 err_msg_buff_len
    );

    [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public extern static int tls_vri_sig_ex(
        string sig,
        string pub_key,
        UInt32 pub_key_len,
        UInt32 sdkappid,
        string identifier,
        ref UInt32 expire_time,
        ref UInt32 init_time,
        StringBuilder err_msg,
        UInt32 err_msg_buff_len
    );
}

//class SignCheck
//{
//    //申明外部API
//    [DllImport("kernel32.dll")]
//    static extern IntPtr LoadLibrary(string lpFileName);

//    [DllImport("kernel32.dll")]
//    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

//    [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
//    static extern bool FreeLibrary(IntPtr hModule);

//    //申明委托
//    private delegate IntPtr tls_gen_sig_ex_Proc(IntPtr aAppHandle, IntPtr aCallWinHandle, IntPtr aPluginHandle,
//          UInt32 sdkappid,
//        string identifier,
//        StringBuilder sig,
//        UInt32 sig_buff_len,
//        string pri_key,
//        UInt32 pri_key_len,
//        StringBuilder err_msg,
//        UInt32 err_msg_buff_len);
//    //获取函数地址
//    private Delegate GetFunctionAddress(IntPtr dllModule, string functionName, Type t)
//    {
//        IntPtr address = GetProcAddress(dllModule, functionName);
//        if (address == IntPtr.Zero)
//            return null;
//        else
//            return Marshal.GetDelegateForFunctionPointer(address, t);
//    }
//    private IntPtr hModule = IntPtr.Zero;
//    tls_gen_sig_ex_Proc farProc;
//    public SignCheck(string dllFunctionName)
//    {
//        string strDLLPath = System.Web.HttpContext.Current.Server.MapPath(@"~\App_Code\tls_sig\sigcheck32.dll");
//        hModule = LoadLibrary(strDLLPath);
//        if (hModule.Equals(IntPtr.Zero))
//        {
//            return;
//        }

//        //将要调用的方法转换为委托：hModule为DLL的句柄，"tls_gen_sig_ex"为DLL中方法的名称
//        farProc = (tls_gen_sig_ex_Proc)this.GetFunctionAddress(hModule, dllFunctionName, typeof(tls_gen_sig_ex_Proc));
//        if (farProc == null)
//        {
//            FreeLibrary(hModule);
//            hModule = IntPtr.Zero;
//            return;
//        }
//    }

//    public int Run_tls_gen_sig_ex(
//        UInt32 sdkappid,
//        string identifier,
//        StringBuilder sig,
//        UInt32 sig_buff_len,
//        string pri_key,
//        UInt32 pri_key_len,
//        StringBuilder err_msg,
//        UInt32 err_msg_buff_len)
//    {
//        //利用委托执行DLL文件中的接口方法
//        farProc(hModule, IntPtr.Zero, IntPtr.Zero, sdkappid,
//                identifier,
//                sig,
//                sig_buff_len,
//                pri_key,
//                pri_key_len,
//                err_msg,
//                err_msg_buff_len);

//        FreeLibrary(hModule);
//        hModule = IntPtr.Zero;
//        return 0;
//    }

//      //调用：
////int ret = new SignCheck("tls_gen_sig_ex").Run_tls_gen_sig_ex(
////            sdkappid,
////            identifier,
////            sig,
////            4096,
////            pri_key,
////            (UInt32)pri_key.Length,
////            err_msg,
////            4096
////            );
//}
