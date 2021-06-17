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

using System;
using System.Collections;

namespace Scorpion
{
    public class Types
    {
        public bool B_Yes = true;
        public bool B_No = false;
        public string S_Yes = "true";
        public string S_No = "false";
        public int INDEX_NOT_FOUND = -1;
        public char C_Delim_Start = '[';
        public char C_Delim_Stop = ']';
        public string S_NULL = "";

        //COMPARERS
        public string S_Equal = "=";
        public string S_Less = "<";
        public string S_Greater = ">";
        public string S_Not_Equal = "!=";
        public string S_And = "&";
        public string S_Or = "|";

        //DB
        public const string DB_TYPE_STRING = "text";
        public const string DB_TYPE_EXECUTABLE = "bytes";
        public const string DB_TYPE_MEDIA = "media";


        Scorp fm_1_ref;
        public Types(Scorp fm_1)
        {
            fm_1_ref = fm_1;
            return;
        }

        public void load_system_vars()
        {
            fm_1_ref.readr.lib_SCR.var("", new ArrayList(5) { "true", "false", "temp" });
            fm_1_ref.readr.lib_SCR.var("", new ArrayList(5) { "userfolder", "userfolder" });
            fm_1_ref.readr.lib_SCR.varset("", new ArrayList(5) { "true", "'true'" });
            fm_1_ref.readr.lib_SCR.varset("", new ArrayList(5) { "false", "'false'" });
            fm_1_ref.readr.lib_SCR.varset("", new ArrayList(5) { "userfolder", "'" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "'" });
            return;
        }

        //Conversions
        public bool Convert_String_To_Bool(string YN)
        {
            if (YN.ToLower() == "yes" || YN.ToLower() == "true")
                return true;
            //CLEAN
            YN = null;
            return false;
        }

        public string Convert_booltostring(bool bool_)
        {
            switch(bool_)
            {
                case true:
                    return S_Yes;
                default:
                    return S_No;
            }
        }
    }
}