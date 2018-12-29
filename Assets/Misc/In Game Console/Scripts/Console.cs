using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Kpable.Utilities;
using System.Text.RegularExpressions;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class Console : SingletonBase<Console>, IConsole
    {
        public TMP_Text textBox;
        public TMP_InputField inputBox;
        public Scrollbar scrollbar;

        bool IsConsoleShown = false;

        Log log;
        RegExLib regEx;
        Commands commands;

        //Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");
        //Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");

        public Commands Commands { get { return commands; } set { commands = value; } }
        public Log Log { get { return log; } set { log = value; } }
        public BaseCommands BaseCommands { get; set; }
        public RegExLib RegEx { get { return regEx; } set { regEx = value; } }
        public History History { get; set; }

        public bool Register(string alias, Dictionary<string, object[]> arguments)
        {
            return Commands.Register(alias, arguments);      
        }

        public void ToggleConsole()
        {
            throw new System.NotImplementedException();
        }

        public void Write(string message)
        {
            textBox.text += message;
        }

        public void WriteLine(string message = "")
        {
            textBox.text += message + "\n";
        }

        // Use this for initialization
        void Start()
        {
            Commands = new Commands();
            Log = new Log();
            RegEx = new RegExLib();
            History = new History();
            BaseCommands = new BaseCommands();
            
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void HandleEnteredCommand(string command)
        {
            History.Reset();
            Commands.Autocomplete.Reset();
            //command = eraseTrash.Replace(command, "");
            if (string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command)) return;

            string[] cmdsplit = command.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
            string cmdName = cmdsplit[0];

            Command Command = Commands.Get(cmdName);
            if (Command == null)
            {
                Log.Warn("No such command");
                ClearInput();
                return;
            }

            string cmdArgs = string.Join(" ", cmdsplit.Skip(1).ToArray());
            if (Command.RequireArgs())
            {
                if (cmdArgs.Length != Command.RequiredArgumentsCount && Command.RequireStrings())
                {

                }
            }

            History.Push(Command.Alias + " " + cmdArgs);
            WriteLine("<#00ff00>$</color> " + Command.Alias + " " + cmdArgs);
            Command.Run(cmdArgs.Split(' '));

            ClearInput();
        }

        private void ClearInput()
        {
            inputBox.text = string.Empty;
            inputBox.ActivateInputField();
            scrollbar.value = 0;
        }
    }
}

