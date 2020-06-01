﻿/*  <Scorpion IEE(Intelligent Execution Environment). Kernel To Run Scorpion Built Applications Using the Scorpion Language>
    Copyright (C) <2014>  <Oscar Arjun Singh Tark>

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

using System.Collections;
using System.Windows.Forms;

//Static Library
namespace Scorpion
{
    partial class Librarian
    {
        public void exit(string Scorp_Line_Exec, ArrayList Objects)
        {
            Application.Exit();
        }

        public void restart(string Scorp_Line_Exec, ArrayList Objects)
        {
            Application.Restart();
        }

        public void wiki(ref string Scorp_Line_Exec, ArrayList Objects)
        {
            for (int i = 0; i < Objects.Count; i++)
                Do_on.write_to_cui(Do_on.AL_WIKI[Do_on.AL_TERMS_WIKI_REF.IndexOf(var_get(Objects[i].ToString()))].ToString());
            var_arraylist_dispose(ref Objects);
            Scorp_Line_Exec = null;
            return;
        }

        public object test_return(ref string Scorp_Line_Exec, ArrayList Objects)
        {
            bool elem = true;
            if ((string)var_get(Objects[0].ToString()) == "false")
                elem = false;
            string return__ = "one_root";
            return (object)var_create_return(ref return__, elem);
        }

        private void write_to_console(ref string STR_)
        {
            Do_on.write_to_cui(STR_);
            return;
        }

        private void write_to_cui(string Scorp_Line_Exec)
        {
            ArrayList al = cut_variables(ref Scorp_Line_Exec);
            foreach (object o in al)
            {
                Do_on.write_to_cui(var_get(o.ToString()).ToString() + "\n");
            }

            var_arraylist_dispose(ref al);
            Scorp_Line_Exec = null;
            return;
        }
    }
}