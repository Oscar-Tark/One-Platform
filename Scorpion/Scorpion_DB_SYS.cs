﻿using System;
using System.Collections;
using System.IO;
using System.Security;

//DEPRECIATED
namespace Dumper
{
    public class Virtual_Dumper_System
    {
        private const int DEFAULT_SLOT_SIZE = 2048;
        private const int DEFAULT_STRT_SIZE = 0;
        Scorpion.Scorp Do_on;
        private string node_file = ".stg.nde";
        /*private string db_file = ".stg";
        private string current_path = "";
        private string db_folder = "vds";
        private string db_folder_full_path = null;
        private string db_config_path = null;
        private string db_config_file = "scorpion.config";
        private string db_config = null;*/

        public Virtual_Dumper_System(Scorpion.Scorp fm1)
        {
            Do_on = fm1;
            /*get_paths();
            create_FileDir();
            get_config();*/
            return;
        }

        /*private void get_config()
        {
            db_config = File.ReadAllText(db_config_path);
        }

        private void get_paths()
        {
            current_path = Environment.CurrentDirectory;
            db_folder_full_path = current_path + db_folder;
            db_config_path = current_path + db_config_file;
            return;
        }

        private void create_FileDir()
        {
            if (!Directory.Exists(db_folder_full_path))
                Directory.CreateDirectory(db_folder_full_path);

            if (!File.Exists(db_config_path))
                File.Create(db_config_path);
            return;
        }*/

        private void NULLIFY(ref string[] reference_, ref string[] data)
        {
            for(int i = 0; i < reference_.Length; i++)
            {
                reference_[i] = null;
                data[i] = null;
            }
            return;
        }

        public void Create_DB(string path)
        {
            object[] s_data = new object[DEFAULT_SLOT_SIZE];
            string[] s_tag = new string[DEFAULT_SLOT_SIZE];
            string[] s_meta = new string[DEFAULT_SLOT_SIZE];
            //Create SHA seed
            SecureString s_seed = Do_on.crypto.Create_Seed();
            //Create SHA out of seed
            string sha_ = Do_on.crypto.SHA_SS(s_seed);
            ArrayList al = new ArrayList (4) { s_data, s_tag, s_meta, sha_, DEFAULT_STRT_SIZE };

            //Encrypt s_dat, s_tag, s_meta with the seed

            byte[] bte = Do_on.crypto.To_Byte(al);
            File.WriteAllBytes(path, bte);
            bte = null;
            path = null;
            return;
        }

        public ArrayList Load_DB(string path)
        {
            //File.Decrypt(path);
            byte[] b = File.ReadAllBytes(path);
            //Get pwd as securestring
            SecureString scr = new SecureString();
            object o = Do_on.crypto.To_Object(b);
            return (ArrayList)o;
        }

        public void Save_DB(string path, string pwd)
        {
            //Save in segments of 0x3a each
            //File.Encrypt(path);
            byte[] b = Do_on.crypto.To_Byte(Do_on.AL_TBLE[Do_on.AL_TBLE_REF.IndexOf(path)]);
            File.WriteAllBytes(path, b);
            path = null;
            pwd = null;
            return;
        }

        public string Data_getDB(string db, string reference)
        {
            //
            ArrayList al_tmp = (ArrayList)Do_on.AL_TBLE[Do_on.AL_TBLE_REF.IndexOf(db)];
            int ndx = Array.IndexOf((string[])al_tmp[0], reference);

            if (ndx == -1)
                return "";
            return ((string[])al_tmp[1])[ndx];
        }

        public void Data_setDB(string path, object data, string meta, string tag)
        {
            lock (Do_on.AL_TBLE)
            {
                ArrayList al_tmp = (ArrayList)Do_on.AL_TBLE[Do_on.AL_TBLE_REF.IndexOf(path)];
                int size = (int)al_tmp[4];
                ((object[])al_tmp[0])[size] = data;
                ((string[])al_tmp[1])[size] = meta;
                ((string[])al_tmp[2])[size] = tag;
                al_tmp[4] = size++;
            }
            return;
        }

        private int get_position(ref string[] tbl)
        {
            for (int i_pos = 0; i_pos < tbl.Length; i_pos++)
            {
                if (tbl[i_pos] == null)
                    return i_pos;

                //if (i_pos == tbl.Length - 1 && position == 0)
                //    Segment_DB(ref db);
            }


            return -1;
        }

        private string[] get_query()
        {
            //'value@joe dawson'
            //'ref@12342'
            //'meta@city=DENVER'
            //'tag@TEACHERS'

            return new string[0];
        }


        /*
        //DUMP SYSTEM DB's
        public void Dump_main_db()
        {
            /*var Serializer = new BinaryFormatter();
            
            ArrayList al_rec_tmp = new ArrayList();
            foreach (ArrayList al in Do_on.AL_REC)
            {
                al_rec_tmp.Add(((System.Windows.Forms.Timer)al[0]).Interval);
            }
            byte[] b = Do_on.crypto.encrypt(new ArrayList() { Do_on.AL_CURR_VAR, Do_on.AL_CURR_VAR_REF, Do_on.AL_EVT, Do_on.AL_Ref_EVT, al_rec_tmp, Do_on.AL_REC_REF, Do_on.AL_SHS, Do_on.AL_SHS_REF, Do_on.AL_AUTH, Do_on.AL_AUTH_REF }, Do_on.SHA);
            File.WriteAllBytes(Do_on.AL_HIB_FILES[0].ToString(), b);
            
            Serializer = null;*/

        /*  return;
      }
        /*
        public void Verify_Directory_DB()
        {
            if (!Directory.Exists(Do_on.AL_DIRECTORIES[0].ToString()))
            {
                Directory.CreateDirectory(Do_on.AL_DIRECTORIES[0].ToString());
            }
            return;
        }

        public void Verify_File_DB(string Name)
        {
            if (!Directory.Exists(Do_on.AL_DIRECTORIES[0].ToString()))
            {
                Directory.CreateDirectory(Do_on.AL_DIRECTORIES[0].ToString());
            }

            if (!File.Exists(Do_on.AL_DIRECTORIES[0].ToString() + Name + Do_on.AL_EXTENSNS[1]))
            {
                FileStream fs = new FileStream(Do_on.AL_DIRECTORIES[0].ToString() + Name + Do_on.AL_EXTENSNS[1], FileMode.Create, FileAccess.Write);
                fs.Flush();
                fs.Close();
                fs.Dispose();
            }

            Name = null;
            return;
        }

        public void Un_Dump(string pwd)
        {
            ArrayList al_tmp = new ArrayList();
            /*if (Do_on.readr.lib_SCR.var_get("*secs").ToString() == Do_on.types.S_Yes)
            {*/
        /* byte[] b = File.ReadAllBytes(Do_on.AL_HIB_FILES[0].ToString());
         byte[] read_bytes = Do_on.crypto.decrypt(b, pwd);

         al_tmp = (ArrayList)Do_on.crypto.To_Object(new MemoryStream(read_bytes));
     /*}
     else
     {
         var Serializer = new BinaryFormatter();
         using (var stream = File.OpenRead(Do_on.AL_HIB_FILES[0].ToString()))
         {
             al_tmp = (ArrayList)Serializer.Deserialize(stream);
         }

         Serializer = null;
     }*/


        /* Do_on.AL_CURR_VAR = (ArrayList)al_tmp[0];
         Do_on.AL_CURR_VAR_REF = (ArrayList)al_tmp[1];
         //Do_on.AL_EVT = (ArrayList)al_tmp[2];
         //Do_on.AL_Ref_EVT = (ArrayList)al_tmp[3];

         foreach (int i in (ArrayList)al_tmp[4])
         {
             System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
             t.Interval = i;
             //Do_on.AL_REC.Add(t);
         }

         //Do_on.AL_REC_REF = (ArrayList)al_tmp[5];
         //Do_on.AL_SHS = (ArrayList)al_tmp[6];
         //Do_on.AL_SHS_REF = (ArrayList)al_tmp[7];
         //Do_on.AL_AUTH = (ArrayList)al_tmp[8];
         //Do_on.AL_AUTH_REF = (ArrayList)al_tmp[9];
         //Do_on.AL_OBJ_3D = (ArrayList)al_tmp[8];
         //Do_on.AL_OBJ_3D_REF = (ArrayList)al_tmp[9];

         Do_on.readr.lib_SCR.var_arraylist_dispose(ref al_tmp);

         //OLD
         /*
         var Serializer = new BinaryFormatter();
         using (var stream = File.OpenRead(Do_on.AL_HIB_FILES[0].ToString()))
         {
             try
             {
                 ArrayList al_tmp = (ArrayList)Serializer.Deserialize(stream);
                 Do_on.AL_CURR_VAR = (ArrayList)al_tmp[0];
                 Do_on.AL_CURR_VAR_REF = (ArrayList)al_tmp[1];
                 Do_on.AL_EVT = (ArrayList)al_tmp[2];
                 Do_on.AL_Ref_EVT = (ArrayList)al_tmp[3];

                 foreach (int i in (ArrayList)al_tmp[4])
                 {
                     System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                     t.Interval = i;
                     Do_on.AL_REC.Add(t);
                 }

                 Do_on.AL_REC_REF = (ArrayList)al_tmp[5];
                 Do_on.AL_SHS = (ArrayList)al_tmp[6];
                 Do_on.AL_SHS_REF = (ArrayList)al_tmp[7];
                 //Do_on.AL_OBJ_3D = (ArrayList)al_tmp[8];
                 //Do_on.AL_OBJ_3D_REF = (ArrayList)al_tmp[9];

                 Do_on.readr.lib_SCR.var_arraylist_dispose(ref al_tmp);
             }
             catch { }
         }*/

        //Serializer = null;
        /* return;
     }*/

    }
}