using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpCodes = dnlib.DotNet.Emit.OpCodes;

namespace GunapPatcher
{
    class Program
    {
        private static ModuleDefMD Module = null;
        private static string NewLocation = string.Empty;
        static void Main(string[] args)
        {
            Module = ModuleDefMD.Load(args[0]);

            NewLocation = Module.Location.Replace(Module.Name, $"Cracked");
            
            if (Module.Name != "Guna.UI2" && Module.Assembly.Name != "Guna.UI2")
            {
                Console.WriteLine("Are you doing this on purpose ?");
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
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "https://t.me/TheHellTower"));
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, Module.Import(typeof(System.Windows.Forms.MessageBox).GetMethod("Show", new Type[] { typeof(string), typeof(string) }))));
                            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

                            Console.WriteLine("Cracked !");
                        }
            }

            if (!Directory.Exists(NewLocation))
                Directory.CreateDirectory(NewLocation);

            Module.Write($"{NewLocation}\\{Module.Name}");
        }
    }
}