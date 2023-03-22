using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using static IronPython.Modules._ast;

namespace pythonExecute
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 法1：執行緒 (Success!)
                string pythonExe = @"C:\ProgramData\Anaconda3\python.exe";
                string path = Directory.GetCurrentDirectory() + @"\pythonfile\params.py";

                string outputText = "";
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = pythonExe;
                    p.StartInfo.Arguments = path + " 3 3";

                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;// 接受來自呼叫程式的輸入資訊
                    p.StartInfo.RedirectStandardOutput = true;// 由呼叫程式獲取輸出資訊
                    p.StartInfo.RedirectStandardError = true;// 重定向標準錯誤輸出
                    p.StartInfo.CreateNoWindow = false; // 跳出cmd視窗
                    
                    p.Start(); // 啟動程式：輸出 output.txt

                    outputText = p.StandardOutput.ReadToEnd();
                    outputText = outputText.Replace(Environment.NewLine, "");

                    p.Close(); // 關閉程式
                    
                }

                Console.WriteLine(outputText); // 印出
                Console.ReadKey();

                // 法2：IronPython 套件 (Success!)
                var engine = IronPython.Hosting.Python.CreateEngine();
                var scope = engine.CreateScope();
                var source = engine.CreateScriptSourceFromFile(path);
                source.Execute(scope);

                var say_hello = scope.GetVariable<Func<object>>("say_hello");
                say_hello(); // hello!

                var get_text = scope.GetVariable<Func<object>>("get_text");
                var text = get_text().ToString();
                Console.WriteLine(text); // text from hello.py

                var add = scope.GetVariable<Func<object, object, object>>("add");
                var result1 = add(1, 2);
                Console.WriteLine(result1); // 3

                var result2 = add("hello ", "world");
                Console.WriteLine(result2); // hello world
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
        }
    }
}