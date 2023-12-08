using dnlib.DotNet;
using dnlib.DotNet.Emit;
using GunaPatcher;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GunapPatcher
{
    class Program
    {
        private static ModuleDefMD Module = null;
        private static string NewLocation = string.Empty;
        static string[] ModuleNames = new string[] { "Guna.UI2", "Guna.Charts.WinForms.dll" }, AssemblyNames = new string[] { "Guna.UI2", "Guna.Charts.WinForms" };
        static Status Status = Status.NotCracked;
        static void Main(string[] args)
        {
            Module = ModuleDefMD.Load(args[0]);

            NewLocation = Module.Location.Replace(Module.Name, $"Cracked");

            Console.Title = "GunaPatcher | https://t.me/TheHellTower_Group";

            if (!ModuleNames.Contains(Module.Name.ToString()) && !AssemblyNames.Contains(Module.Assembly.Name.ToString()))
            {
                Console.WriteLine("Are you doing this on purpose ?\nContact https://t.me/TheHellTower if this is an error.");
                Process.Start("https://t.me/TheHellTower_Group");
            }
            else
            {
                Console.WriteLine($"[Loaded] {Module.Assembly.Name} | {Module.Assembly.Version}");
                foreach (TypeDef Type in Module.Types)
                    foreach (TypeDef NT in Type.NestedTypes.Where(NT => NT.HasMethods && NT.Methods.Count() == 5 && NT.Fields.Count() == 5))
                        foreach (MethodDef Method in NT.Methods.Where(M => M.HasBody && M.Body.HasInstructions && M.Body.Instructions.Count() > 300))
                        {
                            Method.Body.Variables.Clear();
                            Method.Body.Instructions.Clear();
                            Method.Body.ExceptionHandlers.Clear();

                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "Cracked by TheHellTower"));
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "https://t.me/TheHellTower_Group"));
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, Module.Import(typeof(System.Windows.Forms.MessageBox).GetMethod("Show", new Type[] { typeof(string), typeof(string) }))));
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

                            Status = Status.Cracked;
                        }

                if (Status == Status.Cracked)
                {
                    Console.WriteLine("Cracked with success !");
                    if (!Directory.Exists(NewLocation))
                        Directory.CreateDirectory(NewLocation);


                    Module.Write($"{NewLocation}\\{Module.Name}");
                } else
                {
                    Console.WriteLine("Unfortunately, this version is not supported..\n\nContact https://t.me/TheHellTower for the patcher to get updated !");
                    Process.Start("https://t.me/TheHellTower_Group");
                }
            }

            Console.ReadLine();
        }
    }
}