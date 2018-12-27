using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Kpable.Utilities;
using System.Text.RegularExpressions;

namespace Kpable.InGameConsole
{
    public class Console : SingletonBase<Console>, IConsole
    {
        public TextMeshProUGUI textBox;
        public TMP_InputField inputBox;

        bool IsConsoleShown = false;

        History history;
        Log log;
        RegExLib regEx;
        Commands commands;

        //Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");
        Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");

        public Commands Commands { get { return commands; } }
        public Log Log { get { return log; } }
        
        public BaseCommands BaseCommands
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public int Register(string alias, Dictionary<string, object> arguments)
        {
            throw new System.NotImplementedException();           
        }

        public void ToggleConsole()
        {
            throw new System.NotImplementedException();
        }

        public void Write(string message)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(string message = "")
        {
            throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }


        void HandleEnteredCommand(string command)
        {
            history.Reset();
            commands.Autocomplete.Reset();
            command = eraseTrash.Replace(command, "");

            string cmdName = command.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries)[0];

            Command Command = Commands.Get(cmdName);

        }

    }
}

