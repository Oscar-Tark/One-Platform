
using System;
using System.Collections;

//Static Library
namespace Scorpion
{
    public static class ParsingCore
    {
        static T DirectCast<T>(object o, Type type) where T : class
        {
            if (!type.IsInstanceOfType(o))
                throw new ArgumentException();
            T value = o as T;
            if (value == null && o != null)
                throw new InvalidCastException();
            return value;
        }

        public static ArrayList cut_variables(string Scorp_Line_Exec) =>
            split_vars(ref Scorp_Line_Exec);

        public static ArrayList cut_variables(ref string Scorp_Line_Exec) =>
            split_vars(ref Scorp_Line_Exec);

        private static ArrayList split_vars(ref string Scorp)
        {
            string[] vars = Scorp.Split('*', ',');
            //Allow value variables with ubearables
            ArrayList vars_ = new ArrayList();
            int ndx = 0;
            foreach (string s in vars)
            {
                if (s != "" && s != " " && ndx != 0 && s.EndsWith(">>", StringComparison.CurrentCulture) != true)
                    vars_.Add(s);
                ndx++;
            }
            return vars_;
        }

        public static ArrayList cut_commands(ref string Scorp_Line_Exec, ref ArrayList append, int index)
        {
            //(''),(,''),[''],['',],['' ]
            int ndx = 0; int ndx2 = 0;// ArrayList al = new ArrayList();
            append.RemoveRange(index, append.Count - (index + 1));
            foreach (char c in Scorp_Line_Exec)
            {
                ndx = Scorp_Line_Exec.IndexOf("'", ndx);
                ndx2 = Scorp_Line_Exec.IndexOf("'", ndx + 1);
                if (ndx == -1 || ndx2 == -1) 
                    break;

                append.Add(Scorp_Line_Exec.Remove(ndx2).Remove(0, ndx + 1));

                ndx++;
            }
            return append;
        }

        public static ArrayList cut_symbol(ref ArrayList al)
        {
            for (int i = 0; i < al.Count; i++)
                al[i] = MemoryCore.varGet(al[i].ToString());
            return al;
        }

        public static string cut_square_parenth_LEADING_TRACE(ref string Scorp_Line_Exec, int Trail, char Last_Del)
        {
            int index = -1;
            for (int i = 0; i < Trail; i++)
            {
                if (i == Trail - 1)
                    index = Scorp_Line_Exec.IndexOf("*", index + 1);
                else
                    index = Scorp_Line_Exec.IndexOf("*", index + 1);
            }

            Scorp_Line_Exec = Scorp_Line_Exec.Remove(Scorp_Line_Exec.IndexOf(Last_Del));
            return Scorp_Line_Exec = Scorp_Line_Exec.Remove(0, index);
        }

        public static int[] toIntArray(object[] array)
        {
            int hold = -1, ndx = 0;
            int[] returnable = new int[array.Length];
            foreach(object o in array)
            {
                if(Int32.TryParse(o.ToString(), out hold))
                {
                    returnable[ndx] = hold;
                    ndx++;
                }
            }

            //Resize the array to eliminate empty entries
            Array.Resize<int>(ref returnable, ndx);
            return returnable;
        }
    }
}