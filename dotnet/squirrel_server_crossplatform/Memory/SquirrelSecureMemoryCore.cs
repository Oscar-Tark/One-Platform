using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Security;
using System.Runtime.InteropServices;

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
            if(string_.Length > Types.HANDLE.librarian_instance.librarian.get_limit())
                return false;
            return true;
        }
    }

    public class Secure_Memory
    {
        private string uname = null;
        private SecureString password = null;
        private SecureString password_secured = null;

        public void set_uname(ref string username)
        {
            uname = username;
            return;
        }

        public string get_uname()
        {
            return uname;
        }

        public SecureString get_pwd()
        {
            return password;
        }

        //AUTHENTICATE USER FOR EXECUTION OF A FUNCTION, DEFAULT = NO PERMISSION
        public bool authenticate_execution(ref string function)
        {
            //Scorpion database is used for this
            Scorpion_Authenticator.ExecutionPersmissions ep = new Scorpion_Authenticator.ExecutionPersmissions(ref Types.HANDLE.mmsec.uname);
            return ep.check_authentication(ref Types.HANDLE.mmsec.uname, ref function);
        }

        public void write_permissions()
        {
            //Scorpion database is used for this
            Scorpion_Authenticator.ExecutionPersmissions ep = new Scorpion_Authenticator.ExecutionPersmissions(ref Types.HANDLE.mmsec.uname);
            ep.write_permissions();
            return;
        }

        //make private
        public void set_pass(SecureString pass)
        {
            password = pass;
            password_secured = password;
            return;
        }

        private SecureString set_secure(ref string element)
        {
            SecureStringVar ssv = new SecureStringVar();
            return ssv.create_secure_string(ref element);
        }

        private void var_set_encrypted(string Reference, byte[] block_)
        {
            ((ArrayList)Types.HANDLE.mem.AL_CURR_VAR[Types.HANDLE.mem.AL_CURR_VAR_REF.IndexOf(Reference)])[2] = block_;
            return;
        }

        private byte[] var_get_encrypted(ref string Reference)
        {
            return (byte[])((ArrayList)Types.HANDLE.mem.AL_CURR_VAR[Types.HANDLE.mem.AL_CURR_VAR_REF.IndexOf(Reference)])[2]; ;
        }
    }

    //Creates a secure string pool
    public class SecureStringVar
    {
        public SecureString create_secure_string(ref string element)
        {
            SecureString sec = new SecureString();
            foreach (char c_ in element)
                sec.AppendChar(c_);
            return sec;
        }

        public string convert_to_string(SecureString sec)
        {
            IntPtr pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.SecureStringToGlobalAllocUnicode(sec);
                return Marshal.PtrToStringUni(pointer);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }
    }
}
