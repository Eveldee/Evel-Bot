using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Evel_Bot.Util
{
    public class ConfigurationFile
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Boolean Equals(Object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override String ToString()
        {
            return base.ToString();
        }

        private string Name { get; }
        private string Path { get => Name; }
        /// <summary>
        /// Get the number of <see cref="Setting"/> in the file
        /// </summary>
        public int Count { get { return Ressources.Count; } }
        /// <summary>
        /// Check if the file is empty or not
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (Count == 0)
                    return true;
                else
                    return false;
            }
        }
        private bool IsLoading { get; set; } = false;

        /// <summary>
        /// The char to delimite each variables (ex : Name=Eveldee)
        /// </summary>
        public char Separator { get; set; } = '=';                 //The separator for the key and value
        /// <summary>
        /// Get all the properties related with the File
        /// </summary>
        public _File Properties { get { return new _File(Name); } } //Get the file informations

        private List<Setting> Ressources = new List<Setting>();

        public enum SaveType { Auto, Manual }

        public struct _File //The properties of the File
        {
            public string Name { get; }
            public string Path { get { return Name; } }

            public _File(string n)
            {
                Name = n;
            }
        }

        /// <summary>
        /// Return a <see cref="Setting"/> by his line
        /// </summary>
        /// <param name="index">The line of the <see cref="Setting"/></param>
        /// <returns></returns>
        public Setting this[int index]      //Return the Setting by index
        {
            get
            {
                try
                {
                    return Ressources[index];
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Return a <see cref="Setting"/> by his name
        /// </summary>
        /// <param name="name">The name(Key) of the <see cref="Setting"/></param>
        /// <returns></returns>
        public Setting this[string name]    //Return the Setting by name
        {
            get
            {
                foreach (Setting e in Ressources)
                {
                    if (e.Key == name)
                        return e;
                }
                return null;
            }
        }

        /// <summary>
        /// Create a File at the specfied path or Open it if already exist
        /// </summary>
        /// <param name="path">The directory of the file (ex: C:\\Windows\\Calc.config)</param>
        public ConfigurationFile(string path)    //Constructor
        {
            string name = null;

            name = path;


            if (!name.Contains("."))                //Check if there is an extensions , add it if not
                name += ".cfg";
            this.Name = name;

            if (File.Exists(Path))                  //Check if the file already exist
            {
                ForceLoad();
            }
            else if (!ChkDir(name))
                Environment.Exit(0);
        }

        private bool ChkDir(string name) //Check if the file can be created , throw an Exception if not
        {
            bool chk = true;
            try
            {
                File.Create(name).Dispose();
            }
            catch (UnauthorizedAccessException)
            {
                chk = false;
                Console.WriteLine("Can't acces to " + name + " , try to run in Administrator", "Permissions error");
            }
            catch (Exception e)
            {
                chk = false;
                Console.WriteLine(e.Message, "An error occured");
            }

            return chk;
        }

        /// <summary>
        /// Return a <see cref="Setting"/> by his name
        /// </summary>
        /// <param name="key">The name(Key) of the setting</param>
        /// <returns></returns>
        public Setting Read(string key) => this[key]; //Do the same as the Index

        /// <summary>
        /// Return a <see cref="Setting"/> by his line
        /// </summary>
        /// <param name="index">The line of the setting</param>
        /// <returns></returns>
        public Setting Read(int index) => this[index]; //Do the same as the Index

        /// <summary>
        /// Add a <see cref="Setting"/> to the file , if the <see cref="Setting"/> already exist the Set() method is called
        /// </summary>
        /// <param name="key">The name(key) of the setting</param>
        /// <param name="value">The value of the setting</param>
        public void Add(string key, string value) //Add a value , if already exist just set it
        {
            if (this[key] != null)
            {
                Set(key, value);
            }
            else
            {
                Ressources.Add(new Setting(key, value, Count));
            }
        }

        /// <summary>
        /// Set the value of a <see cref="Setting"/>
        /// </summary>
        /// <param name="key">The name(key) of the setting</param>
        /// <param name="value">The value to set</param>
        public void Set(string key, string value) //Set a value , if not exist add it
        {
            int line = GetLine(Ressources, key);

            if (line != -1)
            {
                Ressources[line].Value = value;
            }
            else
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Set the value of a <see cref="Setting"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="Setting"/></param>
        /// <param name="value">The value to set</param>
        public void Set(int index, string value) //Set a value , if not exist do nothing
        {
            string key = this[index].Key;

            if (key != null)
            {
                Ressources[index].Value = value;
            }
        }

        /// <summary>
        /// Remove a <see cref="Setting"/>
        /// </summary>
        /// <param name="key">The name(key) of the setting</param>
        public void Remove(string key) //Remove a value
        {
            int line = GetLine(Ressources, key);

            if (line != -1)
            {
                Ressources.RemoveAt(line);
                ReloadIndex();
            }
        }

        /// <summary>
        /// Save all the element in the List to the File , only use it if <see cref="SaveType"/> is set to manual
        /// </summary>
        public void Save() //Save the Ressources to the File
        {
            int i = 0;
            string[] text = new string[Ressources.Count];
            foreach (Setting s in Ressources)
            {
                string line = s.Key + Separator + s.Value;
                text[i] = line;
                i++;
            }
            File.WriteAllLines(Path, text);
        }

        /// <summary>
        /// Delete all the element of the file and the list but don't delete it
        /// </summary>
        public void RemoveAll() //Remove all the element of the List
        {
            Ressources.RemoveRange(0, Ressources.Count);
            //File.Delete(Path);
        }

        /// <summary>
        /// Copy and return the list where all the <see cref="Setting"/> are.
        /// </summary>
        /// <returns></returns>
        public List<Setting> GetAll() => Ressources; //Get the list where settings are stored

        /// <summary>
        /// Replace the current list with another one.
        /// </summary>
        /// <param name="list">A list with <see cref="Setting"/></param>
        public void SetAll(List<Setting> list) //Set all the settings from a list
        {
            Ressources = list;
        }

        private int GetLine(string[] Text, string str) //Return the line where a word is
        {
            int i = 0;
            bool changed = false;
            foreach (string line in Text) //Read the file Line-by-Line
            {
                string Key = line.Split(Separator)[0]; //Only read the first word
                if (Key == str)
                {
                    changed = true;
                    break;
                }
                i++;
            }
            if (changed)
                return i;
            return -1;
        }

        private int GetLine(List<Setting> List, string str) //Return the index where a Key is
        {
            int i = 0;
            bool changed = false;
            foreach (Setting s in List) //Read the list Line-by-Line
            {
                if (s.Key == str)
                {
                    changed = true;
                    break;
                }
                i++;
            }
            if (changed)
                return i;
            return -1;
        }

        private void ForceLoad() //Load the value in a file
        {
            IsLoading = true;

            string[] text = File.ReadAllLines(Path);

            foreach (string str in text)
            {
                try
                {
                    string[] prop = str.Split(Separator);
                    Ressources.Add(new Setting(prop[0], prop[1], Count));
                }
                catch (Exception)
                {

                }
            }

            IsLoading = false;
        }

        private void ReloadIndex() //Reload all the index
        {
            for (int i = 0; i < Ressources.Count; i++)
            {
                Ressources[i] = new Setting(this[i].Key, this[i].Value, i);
            }
        }
    }

    public class Setting   //A propertie with a Key and a Value
    {
        public string Key { get; private set; }
        private string value;
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
            }
        }
        public int Index { get; private set; }

        public Setting(string key, string value, int index)
        {
            Key = key;
            this.value = value;
            Index = index;
        }

        /// <summary>
        /// Return the raw string representation of the <see cref="Setting"/> (ex: "Name=Eveldee")
        /// </summary>
        /// <returns></returns>
        public override string ToString() //Return the raw string representation
        {
            return Key + "=" + Value;
        }
    }

}
