using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlaxVM
{

    public class TSysOps
    {
        public const int OUT_C = 4;
        public const int OUT_I = 3;
        public const int IMP_I = 2;
        public const int IMP_C = 1;
    }

    public class TInsturctions
    {
        public const int NEG = 106;
        public const int NOP = 107;
        public const int HLT = 108;
        public const int PUSH = 109;
        public const int POP = 110;
        //
        public const int ADD = 111;
        public const int SUB = 112;
        public const int MUL = 113;
        public const int DIV = 114;
        //
        public const int JMP = 115;
        public const int JT = 116;
        public const int JZ = 117;
        public const int JNZ = 118;
        public const int ISEQ = 119;
        public const int ISGT = 120;
        public const int ISLT = 121;
        public const int LEQ = 122;
        public const int GEQ = 123;
        //
        public const int SYS = 124;
        //
        public const int LDA = 125;
        public const int STA = 126;
        //
        public const int DUP = 127;
        public const int AND = 128;
        public const int OR = 129;
        public const int XOR = 130;
        public const int NOT = 131;
        public const int REM = 132;
        public const int INC = 133;
        public const int DEC = 134;
        public const int SHL = 135;
        public const int SHR = 136;
        public const int SWAP = 137;

        public const int CALL = 138;
        public const int RET = 139;

        private Dictionary<string, int> OpNames = new Dictionary<string, int>()
        {
            {"NEG", TInsturctions.NEG},
            {"NOP", TInsturctions.NOP},
            {"HLT", TInsturctions.HLT},
//
            {"PUSH", TInsturctions.PUSH},
            {"POP", TInsturctions.POP},
            {"ADD", TInsturctions.ADD},
            {"SUB", TInsturctions.SUB},
            {"MUL", TInsturctions.MUL},
            {"DIV", TInsturctions.DIV},
//
            {"JMP", TInsturctions.JMP},
            {"JT", TInsturctions.JT},
            {"JZ", TInsturctions.JZ},
            {"JNZ", TInsturctions.JNZ},
//
            {"ISLT", TInsturctions.ISLT},
            {"ISGT", TInsturctions.ISGT},
            {"ISEQ", TInsturctions.ISEQ},
            {"LEQ", TInsturctions.LEQ},
            {"GEQ", TInsturctions.GEQ},
//
            {"SYS", TInsturctions.SYS},
            {"LDA", TInsturctions.LDA},
            {"STA", TInsturctions.STA},

            {"DUP", TInsturctions.DUP},
            {"AND", TInsturctions.AND},
            {"OR", TInsturctions.OR},
            {"XOR", TInsturctions.XOR},
            {"NOT", TInsturctions.NOT},
            {"INC", TInsturctions.INC},
            {"DEC", TInsturctions.DEC},
            {"REM", TInsturctions.REM},
//
            {"SHL", TInsturctions.SHL},
            {"SHR", TInsturctions.SHR},
            {"SWAP", TInsturctions.SWAP},
            {"CALL", TInsturctions.CALL},
            {"RET", TInsturctions.RET}
        };

        public bool KeyExsits(string Insturction)
        {
            return OpNames.ContainsKey(Insturction);
        }

        public int GetOpCode(String Insturction)
        {
            return OpNames[Insturction];
        }
    }

    public class TErrors
    {

        private const string AppTitle = "Flare VM Version 1.5\n";

        public void CompileError(Exception ex)
        {
            Console.WriteLine(AppTitle);
            Console.WriteLine("Compile Error:");
            Console.WriteLine(ex.Message);
        }
        public void RunTimeError_Exception(Exception ex)
        {
            Console.WriteLine(AppTitle);
            Console.WriteLine("RunTime Error:");
            Console.WriteLine(ex.Message);
        }

        public void RunTimeErrorStackErr()
        {
            Console.WriteLine(AppTitle);
            Console.WriteLine("RunTime Error:");
            Console.WriteLine("Stack UnderFlow.");
        }

        public void ArgsMissing()
        {
            Console.WriteLine(AppTitle);
            Console.WriteLine("Not Enough Arguments.");
            Console.WriteLine("Usage FlaxVM Filename.asm");
        }

        public void MissingVariable()
        {
            Console.WriteLine(AppTitle);
            Console.WriteLine("RunTime Error:");
            Console.WriteLine("Variable Index Out Of Range.");
        }
    }

    class VM
    {
        //Program Counter
        public int pc { get; set; }
        //Insturction
        public int op { get; set; }

        public bool running { get; set; }
        //Main program data storage
        public Stack<int> Data;
        //VM Stack
        public Stack<int> CallRet;
        //Program code
        public List<int> pCode;
    }

    public class TFrame
    {
        private Hashtable variables = new Hashtable();

        public int GetVar(int id)
        {
            return (int)variables[id];
        }

        public void SetVar(int id, int value)
        {
            if (!variables.ContainsKey(id))
            {
                variables.Add(id, value);
            }
            else
            {
                variables[id] = value;
            }
        }

        public bool VarFound(int id)
        {
            return variables.Contains(id);
        }

        public void Clear()
        {
            variables.Clear();
        }
    }

    public class TUtils
    {
        public bool IsCharConst(string S)
        {
            return (S[0] == '\'' && S[S.Length - 1] == '\'');
        }

        public bool IsHexStr(string s)
        {
            return (s.ToUpper().StartsWith("0X"));
        }

        public bool IsDeclare(string S)
        {
            return (S[0] == '(' && S[S.Length - 1] == ')');
        }

        public string StripDeclare(string S)
        {
            //Remove ( and ) from front and back of string
            return S.Substring(1, S.Length - 2);
        }

        public bool IsBinStr(string S)
        {
            bool flag = true;
            string tmp = S;

            try
            {
                if (tmp.Length > 0 && tmp[tmp.Length - 1] == 'B' ||
                    tmp[tmp.Length - 1] == 'b')
                {
                    //Remove B
                    tmp = tmp.Trim('b', 'B');

                    if (tmp.Length == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        for (int x = 0; x < tmp.Length; x++)
                        {
                            char c = tmp[x];

                            if (c != '0' && c != '1')
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch
            {
                flag = false;
            }

            tmp = string.Empty;

            return flag;
        }
    }

    public class FlaxVM
    {
        
        private VM vm = new VM();
        private TFrame frame = new TFrame();
        private TErrors ErrEvent = new TErrors();

        private Dictionary<string, int> temp_var = new Dictionary<string, int>();

        private bool CheckStack(int size)
        {
            if (vm.Data.Count() < size)
            {
                ErrEvent.RunTimeErrorStackErr();
                return false;
            }
            return true;
        }

        public void FreeVM()
        {
            vm.Data.Clear();
            vm.CallRet.Clear();
            vm.pCode.Clear();
            frame.Clear();
        }

        public FlaxVM()
        {
            vm.Data = new Stack<int>();
            vm.CallRet = new Stack<int>();
            vm.pCode = new List<int>();
        }

        public bool AssambleFile(string Filename)
        {
            string[] lines = { "" };
            string[] parts = { "" };
            string[] vIdents = { "" };

            Hashtable lables = new Hashtable();
            List<string> tokens = new List<string>();
            TInsturctions Insturctions = new TInsturctions();
            TUtils Utils = new TUtils();

            string Inst = string.Empty;
            string sLine = string.Empty;
            string lb = string.Empty;

            string vTemp = string.Empty;
            int ins_idx = 0;
            int X = 0;
            int vIndex = 0;
            bool flag = true;

            try
            {
                //Get all lines
                lines = File.ReadAllLines(Filename);

                foreach (string line in lines)
                {
                    sLine = line.Trim();

                    if (sLine.Length > 1 && sLine[0] != '#')
                    {
                        if (sLine.StartsWith(".GLOBAL",StringComparison.OrdinalIgnoreCase))
                        {
                            int spos = sLine.IndexOf(" ");
                            if (spos > 0)
                            {
                                sLine = sLine.Substring(spos).Trim();

                                if (!Utils.IsDeclare(sLine))
                                {
                                    //Error
                                    ErrEvent.CompileError(new Exception("Illegal Variable Declaration."));
                                    flag = false;
                                    break;
                                }
                                //Remove the ()
                                sLine = Utils.StripDeclare(sLine);
                                //split the string
                                vIdents = sLine.Split(',');

                                foreach (string vIdent in vIdents)
                                {
                                    vTemp = vIdent.ToUpper().Trim();
                                    string[] vLine = vTemp.Split(' ', '\t');

                                    string s0 = vLine[0].TrimStart('[').TrimEnd(']').Trim();

                                    try
                                    {
                                        vIndex = int.Parse(s0);
                                        Inst = vLine[1].Trim();
                                        temp_var.Add(Inst, vIndex);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrEvent.CompileError(ex);
                                        flag = false;
                                        break;
                                    }
                                }
                                Array.Clear(vIdents, 0, vIdents.Length);
                            }
                        }
                        else
                        {
                            //Split the line in two parts
                            parts = sLine.Split(' ', '\t');

                            foreach (string part in parts)
                            {
                                if (part.Trim().Length > 0)
                                {
                                    //Check fr lables.
                                    if (part[part.Length - 1] == ':')
                                    {
                                        lb = part.ToUpper().TrimEnd(':', ' ', '\t');

                                        if (lables.ContainsKey(lb))
                                        {
                                            ErrEvent.CompileError(new Exception("Label Already Exists '" + lb + "'"));
                                            flag = false;
                                            break;
                                        }
                                        else
                                        {
                                            //Add label address
                                            lables.Add(lb, tokens.Count() - 1);
                                        }
                                    }
                                    else
                                    {
                                        //Add token
                                        tokens.Add(part);
                                    }
                                }
                            }
                        }
                    }
                }
                //Destory scanner arrays
                Array.Clear(lines, 0, lines.Length);
                Array.Clear(parts, 0, parts.Length);

                //Parser / code generator
                foreach (string tok in tokens)
                {
                    lb = Inst = tok.ToUpper().Trim();

                    if (Insturctions.KeyExsits(Inst))
                    {
                        ins_idx = Insturctions.GetOpCode(Inst);
                        vm.pCode.Add(ins_idx);
                    }
                    else if (int.TryParse(tok, out X))
                    {
                        vm.pCode.Add(X);
                    }
                    else if (Utils.IsHexStr(tok))
                    {
                        vm.pCode.Add(Convert.ToInt32(tok, 16));
                    }
                    else if (Utils.IsBinStr(tok))
                    {
                        string sBin = tok.Trim('B','b');
                        vm.pCode.Add(Convert.ToInt32(sBin, 2));
                    }
                    else if (tok.Length == 1 && char.IsLetter(tok[0]))
                    {
                        vm.pCode.Add((int)tok[0]);
                    }
                    else if (Utils.IsCharConst(tok))
                    {
                        string ch = tok.Trim('\'', ' ', '\t');
                        int chAsc = 0;

                        if (ch.Length > 0 && ch[0] == '\\')
                        {
                            switch (ch[1])
                            {
                                case 'N':
                                case 'n':
                                    chAsc = 10;
                                    break;
                                case 'R':
                                case 'r':
                                    chAsc = 13;
                                    break;
                                case 'T':
                                case 't':
                                    chAsc = 9;
                                    break;
                                case 'B':
                                case 'b':
                                    chAsc = 8;
                                    break;
                                default:
                                    ErrEvent.CompileError(new Exception("Illegal Escape Character '\\" + ch[1] + "'"));
                                    flag = false;
                                    break;
                            }
                        }
                        else
                        {
                            chAsc = (int)ch[0];
                        }

                        vm.pCode.Add(chAsc);
                    }
                    else if (lables.ContainsKey(lb))
                    {
                        vm.pCode.Add((int)lables[lb]);
                    }
                    else if (temp_var.ContainsKey(Inst))
                    {
                        vm.pCode.Add(temp_var[Inst]);
                    }
                    else
                    {
                        ErrEvent.CompileError(new Exception("illegal Instruction Found Or Missing Label '" + tok + "'"));
                        flag = false;
                        break;
                    }
                }
                //Destroy tokens
                tokens.Clear();
            }
            catch(Exception ex)
            {
                ErrEvent.CompileError(ex);
                flag = false;
            }
            return flag;
        }

        private bool DoBinOp(int _op)
        {
            if (vm.Data.Count() < 2)
            {
                ErrEvent.RunTimeErrorStackErr();
                return false;
            }

            int a = vm.Data.Pop();
            int b = vm.Data.Pop();

            switch (_op)
            {
                case TInsturctions.ADD:
                    vm.Data.Push(a + b);
                    break;
                case TInsturctions.SUB:
                    vm.Data.Push(b - a);
                    break;
                case TInsturctions.MUL:
                    vm.Data.Push(a * b);
                    break;
                case TInsturctions.DIV:
                    vm.Data.Push(b / a);
                    break;
                case TInsturctions.AND:
                    vm.Data.Push(a & b);
                    break;
                case TInsturctions.REM:
                    vm.Data.Push(a % b);
                    break;
                case TInsturctions.OR:
                    vm.Data.Push(a | b);
                    break;
                case TInsturctions.XOR:
                    vm.Data.Push(a ^ b);
                    break;
                case TInsturctions.ISEQ:
                    vm.Data.Push(Convert.ToInt32(a == b));
                    break;
                case TInsturctions.ISLT:
                    vm.Data.Push(Convert.ToInt32(a < b));
                    break;
                case TInsturctions.ISGT:
                    vm.Data.Push(Convert.ToInt32(a > b));
                    break;
                case TInsturctions.LEQ:
                    vm.Data.Push(Convert.ToInt32(a <= b));
                    break;
                case TInsturctions.GEQ:
                    vm.Data.Push(Convert.ToInt32(a >= b));
                    break;
                case TInsturctions.SHL:
                    vm.Data.Push(Convert.ToInt32(a << b));
                    break;
                case TInsturctions.SHR:
                    vm.Data.Push(Convert.ToInt32(a >> b));
                    break;
                case TInsturctions.SWAP:
                    vm.Data.Push(b);
                    vm.Data.Push(a);
                    break;
            }
            return true;
        }

        private bool DoNoArgOps(int _op)
        {
            if (vm.Data.Count() < 1)
            {
                ErrEvent.RunTimeErrorStackErr();
                return false;
            }
            else
            {
                switch (_op)
                {
                    case TInsturctions.NEG:
                        vm.Data.Push(-System.Math.Abs(vm.Data.Pop()));
                        break;
                    case TInsturctions.POP:
                        vm.Data.Pop();
                        break;
                    case TInsturctions.INC:
                        vm.Data.Push(vm.Data.Pop() + 1);
                        break;
                    case TInsturctions.DEC:
                        vm.Data.Push(vm.Data.Pop() - 1);
                        break;
                    case TInsturctions.DUP:
                        vm.Data.Push(vm.Data.Peek());
                        break;
                    case TInsturctions.NOT:
                        vm.Data.Push(~vm.Data.Pop());
                        break;
                }
                return true;
            }
        }

        public bool Execute()
        {
            int addr = 0;
            vm.running = true;

            while (vm.running && vm.pc < vm.pCode.Count())
            {
                try
                {
                    //Get op
                    vm.op = (int)vm.pCode[vm.pc];

                    switch (vm.op)
                    {
                        case TInsturctions.NEG:
                        case TInsturctions.POP:
                        case TInsturctions.INC:
                        case TInsturctions.DEC:
                        case TInsturctions.DUP:
                        case TInsturctions.NOT:
                            if (!DoNoArgOps(vm.op))
                            {
                                vm.running = false;
                            }
                            break;
                        case TInsturctions.PUSH:
                            vm.pc++;
                            vm.Data.Push(vm.pCode[vm.pc]);
                            break;
                        case TInsturctions.LDA:
                            vm.pc++;
                            addr = vm.pCode[vm.pc];
                            //Find Variable index
                            if (!frame.VarFound(addr))
                            {
                                ErrEvent.MissingVariable();
                                vm.running = false;
                                break;
                            }

                            vm.Data.Push(frame.GetVar(addr));
                            break;
                        case TInsturctions.STA:
                            vm.pc++;
                            addr = vm.pCode[vm.pc];
                            //Check for data been added to the variable
                            if (vm.Data.Count < 1)
                            {
                                ErrEvent.RunTimeError_Exception(new Exception("Missing Data For Variable."));
                                vm.running = false;
                                break;
                            }

                            frame.SetVar(addr, vm.Data.Pop());
                            break;
                        case TInsturctions.JMP:
                            vm.pc++;
                            vm.pc = vm.pCode[vm.pc];
                            break;
                        case TInsturctions.CALL:
                            vm.CallRet.Push(vm.pc);
                            //INC PC
                            vm.pc++;
                            //Move to label address
                            vm.pc = vm.pCode[vm.pc];
                            break;
                        case TInsturctions.RET:
                            if (vm.CallRet.Count() < 1)
                            {
                                ErrEvent.RunTimeError_Exception(new Exception("No Return Address For RET"));
                                vm.running = false;
                                break;
                            }
                            //Set pc to the last return address.
                            vm.pc = vm.CallRet.Pop();
                            break;
                        case TInsturctions.JT:
                            vm.pc++;

                            if (vm.Data.Count() < 1)
                            {
                                ErrEvent.RunTimeErrorStackErr();
                                vm.running = false;
                                break;
                            }

                            if (vm.Data.Pop() == 1)
                            {
                                addr = vm.pCode[vm.pc];
                                vm.pc = addr;
                                break;
                            }
                            break;
                        case TInsturctions.JNZ:
                            vm.pc++;

                            if (vm.Data.Count() > 0 && vm.Data.Peek() != 0)
                            {
                                addr = vm.pCode[vm.pc];
                                vm.pc = addr;
                                break;
                            }
                            break;
                        case TInsturctions.ADD:
                        case TInsturctions.SUB:
                        case TInsturctions.MUL:
                        case TInsturctions.DIV:
                        case TInsturctions.AND:
                        case TInsturctions.OR:
                        case TInsturctions.XOR:
                        case TInsturctions.ISEQ:
                        case TInsturctions.ISLT:
                        case TInsturctions.ISGT:
                        case TInsturctions.LEQ:
                        case TInsturctions.GEQ:
                        case TInsturctions.SHL:
                        case TInsturctions.SHR:
                        case TInsturctions.SWAP:
                        case TInsturctions.REM:
                            if (!DoBinOp(vm.op))
                            {
                                vm.running = false;
                            }
                            break;
                        case TInsturctions.SYS:
                            vm.pc++;
                            switch (vm.pCode[vm.pc])
                            {
                                case TSysOps.OUT_I:
                                    Console.Write(vm.Data.Pop());
                                    break;
                                case TSysOps.OUT_C:
                                    Console.Write((char)vm.Data.Peek());
                                    break;
                                case TSysOps.IMP_C:
                                    string s = Console.ReadLine();
                                    vm.Data.Push((int)s[0]);
                                    break;
                                case TSysOps.IMP_I:
                                    vm.Data.Push(Convert.ToInt32(Console.ReadLine()));
                                    break;
                                default:
                                    vm.running = false;
                                    ErrEvent.RunTimeError_Exception(new Exception("Illegal System IO Call."));
                                    break;
                            }
                            break;
                        case TInsturctions.HLT:
                            vm.running = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrEvent.RunTimeError_Exception(ex);
                    vm.running = false;
                }
                vm.pc++;
            }
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            FlaxVM vm = new FlaxVM();
            TErrors Err = new TErrors();

            if (args.Length < 1)
            {
                Err.ArgsMissing();
                return;
            }
            else
            {
                if (vm.AssambleFile(args[0]))
                {
                    vm.Execute();
                }
                vm.FreeVM();
            }
        }
    }
}
