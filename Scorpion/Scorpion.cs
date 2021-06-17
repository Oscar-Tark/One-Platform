﻿/*  <Scorpion IEE(Intelligent Execution Environment). Server To Run Scorpion Built Applications Using the Scorpion Language>
    Copyright (C) <2020+>  <Oscar Arjun Singh Tark>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Reflection;
using System.Threading;

namespace Scorpion
{
    public partial class Librarian
    {
        System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
    }

    public partial class Librarian
    {
        public Librarian(Scorp Form_Handle)
        {
            Do_on = Form_Handle;
            return;
        }

        public void scorpioniee(object Scorp_Line, Scorp Handle)
        {
            if(Handle != null)
                Do_on = Handle;
            try
            {
                Thread ths = new Thread(new ParameterizedThreadStart(scorpion_exec));
                ths.IsBackground = true;
                ths.Start(Scorp_Line);
            }
            catch { Do_on.write_to_cui("FATAL: COULD NOT START ENGINE THREAD"); }
            return;
        }

        private void scorpion_exec(object Scorp_Line)
        {
            //*return<<function::*vars
            //Add: @@@networkresponse{networkname}@@@
            sp.Start();
            Enginefunctions ef__ = new Enginefunctions();
            string Scorp_Line_Exec = (string)Scorp_Line;

            string[] functions = null;
            try
            {
                //Gets and removes the return variable
                string[] final = ef__.get_return(ref Scorp_Line_Exec);
                if (final.Length > 1)
                    Scorp_Line_Exec = final[1];
                functions = ef__.get_function(ref Scorp_Line_Exec);

                //Check permission
                if (!Do_on.mmsec.authenticate_execution(ref functions[0]))
                {
                    Do_on.write_error("This user does not have enough privileges to execute this function");
                    return;
                }

                object[] paramse = new object[2] { Scorp_Line_Exec, cut_variables(ref Scorp_Line_Exec) };
                object retfun = this.GetType().GetMethod(functions[0], BindingFlags.Public | BindingFlags.Instance).Invoke(this, paramse);

                if (retfun != null)
                    ef__.process_return(ref retfun, ref final[0], this);

                functions = null;
                paramse = null;
                retfun = null;
            }
            catch (Exception erty)
            {
                Do_on.write_error("------------------------------------------------------\nThere was an error while processing your function call [Command that caused the error: " + Scorp_Line_Exec + "]\n[Stack trace: " + erty.StackTrace + "]\n[System message: " + erty.Message + "]");
                showman(functions[0]);
            }

            sp.Stop();
            Do_on.mem.engine_ndx++;
            Do_on.write_to_cui("LIBRARIAN: DOING for: " + Do_on.instance);
            Do_on.write_success("Executed [Call: " + Do_on.mem.engine_ndx + "] >> " + Scorp_Line_Exec + " in " + (sp.ElapsedMilliseconds / 1000) + "s/" + sp.ElapsedMilliseconds + "ms");
            sp.Reset();

            //Make sure objects are set to null and disposed
            ef__ = null;
            var_dispose_internal(ref Scorp_Line_Exec);
            var_dispose_internal(ref Scorp_Line);
            GC.Collect();
            return;
        }
    }

    public class Enginefunctions
    {
        public string replace_fakes(string Scorp_Line)
        {
            return Scorp_Line.Replace("{&var}", "*").Replace("{&quot}", "'");
        }

        public string replace_telnet(string Scorp_Line)
        {
            return Scorp_Line.Replace("\r\n", "").Replace("959;1R", "");
        }

        public string replace_phpapi(string Scorp_Line)
        {
            Scorp_Line = Scorp_Line.Remove(Scorp_Line.IndexOf("{&scorpion_end}", StringComparison.CurrentCulture));
            return Scorp_Line.Remove(0, (Scorp_Line.IndexOf("{&scorpion}", StringComparison.CurrentCulture) + 11));
        }

        public string[] get_function(ref string Scorp)
        {
            string[] delimiterChars = { "::" };
            return Scorp.ToLower().Replace(" ", "").Split(delimiterChars, StringSplitOptions.None);
        }

        public string[] get_return(ref string Scorp)
        {
            string[] delimiterChars = { "<<" };
            return Scorp.Split(delimiterChars, StringSplitOptions.None);
        }

        public string line_check(ref Scorp fm1, ref string Scorp)
        {
            return fm1.san.sanitize(ref Scorp);
        }

        public bool process_return(ref object o, ref string var, Librarian lib)
        {
            lib.varset("", new System.Collections.ArrayList() { var.Replace("*", "") , o });
            return true;
        }
    }
}