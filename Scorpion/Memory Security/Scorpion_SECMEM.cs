﻿using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;

namespace Scorpion.Memory_Security
{
    public class REGEX
    {
        public bool regex_match(ref string string_)
        {
            bool res = false;
            MatchCollection mc;
            string[] s_chars =
            {
                //STARTING SCRIPT
                /*"[<]{1}[s]{1}[c]{1}[r]{1}[i]{1}[p]{1}[t]{1}[>]{1}",
                "(<|>)[s]{1}[c]{1}[r]{1}[i]{1}[p]{1}[t]{1}(<|>)",
                //ENDING SCRIPT
                "[<]{1}[/]{1}[s]{1}[c]{1}[r]{1}[i]{1}[p]{1}[t]{1}[>]{1}",
                "(<|/>)[s]{1}[c]{1}[r]{1}[i]{1}[p]{1}[t]{1}(<|/>)",*/
                //AVOID HEX PAYLOADS
                @"[\]{1}[x]{1}[a-fA-F0-9]{1,2}"
            };

            foreach (string s in s_chars)
            {
                Regex r = new Regex(@"^"+s+"$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                mc = r.Matches(string_.Replace(" ", ""));
                if (mc.Count > 0)
                    res = true;
                r = null;
            }

            mc = null;

            return res;
        }
    }

    public class Sanitizer
    {
        Form1 Do_on;
        public Sanitizer(Form1 fm1)
        {
            Do_on = fm1;
            return;
        }

        public string sanitize(ref string string_)
        {
            if (check_len(ref string_))
                return sanitize_string(ref string_);
            else
                return "\0";
        }

        private string sanitize_string(ref string string_)
        {
            //MATCHES UNSAFE PATTERNS, IF FOUND RETURNS A NULL TERMINATION CHARACTER
            REGEX rg = new REGEX();
            if (rg.regex_match(ref string_))
            {
                rg = null;
                return "\0";
            }
            else
            {
                rg = null;
                return string_;
            }
        }

        private bool check_len(ref string string_)
        {
            if(string_.Length > Do_on.readr.lib_SCR.get_limit())
                return false;
            return true;
        }
    }

    public class Secure_Memory
    {
        Form1 Do_on;
        public Secure_Memory(Form1 fm1)
        {
            Do_on = fm1;
            return;
        }

        /*public void Encrypt_Block(ref string Scorp_Line)
        {
            ArrayList al = Do_on.readr.lib_SCR.cut_variables(Scorp_Line);
            byte[] tmpbyte;

            foreach (string s in al)
            {
                Do_on.write_to_cui("Encrypting Variable: '" + s + "'");
                tmpbyte = Do_on.crypto.encrypt(Do_on.readr.lib_SCR.var_get(s), "anus");
                Do_on.readr.lib_SCR.var_set_encrypted(s, tmpbyte);
            }

            tmpbyte = null;
            Do_on.readr.lib_SCR.var_arraylist_dispose(ref al);
            return;
        }

        public void Decrypt_Block(ref string Scorp_Line)
        {
            ArrayList al = Do_on.readr.lib_SCR.cut_variables(Scorp_Line);
            byte[] tmpbyte;

            foreach (string s in al)
            {
                Do_on.write_to_cui("Decrypting Variable: '" + s + "'");
                tmpbyte = Do_on.crypto.decrypt((byte[])Do_on.readr.lib_SCR.var_get(s), "anus");
                //Do_on.readr.lib_SCR.var_set_decrypted(s, Do_on.crypto.To_Object(new System.IO.MemoryStream(tmpbyte)));
            }

            tmpbyte = null;
            Do_on.readr.lib_SCR.var_arraylist_dispose(ref al);
            return;
        }*/
        private string password = null;
        private byte[] pin_cde_hx = new byte[4];
        private byte pin_cde = 0x00;

        public void set_pass(ref string pass, ref byte[] pin)
        {
            password = pass;
            for (int i = 0; i <= 3; i++)
            {
                pin_cde_hx[i] = pin[i];
                pin_cde += pin_cde_hx[i];
            }
            return;
        }

        public void secure(ref string reference)
        {
            //GETVAR
            string block = (string)Do_on.readr.lib_SCR.var_get(ref reference);

            //TO_BYTE
            byte[] b_raw = Do_on.crypto.To_Byte(block);
            //REVERSE
            byte[] b_raw_rev = b_raw.Reverse().ToArray();
            //AES
            Do_on.crypto.encrypt(block, password);
            //SET
            Do_on.readr.lib_SCR.varset("", new ArrayList() { reference, b_raw_rev });

            reference = null;
            return;
        }

        public void revsecure(ref string reference)
        {
            //GETVAR
            try
            {
                //GET BLOCK
                object block = Do_on.readr.lib_SCR.var_get(ref reference);
                //TO BYTE
                byte[] block_ = Do_on.crypto.To_Byte(block);
                //DECRYPT AES
                byte[] rev = Do_on.crypto.decrypt(block_, password);
                //REVERSE
                block_ = block_.Reverse().ToArray();
                Console.WriteLine("{0:X}", block_);
                //TO OBJ
                object b_obj = Do_on.crypto.To_Object(rev);
                //SET
                Do_on.readr.lib_SCR.varset("", new ArrayList() { reference, b_obj });
            }
            catch(Exception ery) { Console.WriteLine(ery.Message + " | " + ery.StackTrace); }
            reference = null;
            return;
        }
    }
}
