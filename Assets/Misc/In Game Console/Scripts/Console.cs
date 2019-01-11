using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Kpable.Utilities;
using System.Text.RegularExpressions;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class Console : SingletonBase<Console>, IConsole, IPointerClickHandler
    {
        public TMP_Text textBox;
        public TMP_InputField inputBox;
        public Scrollbar scrollbar;
        public GameObject container;

        bool IsConsoleShown = true;
        string currentHistoryCommand = "";
        string currentText = "";

        //Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");
        //Regex eraseTrash = new Regex("\\[[\\/]?[a-z\\=\\#0-9\\ \\_\\-]+\\]");

        public Commands Commands { get; set; }
        public Log Log { get; set; }
        public BaseCommands BaseCommands { get; set; }
        public RegExLib RegEx { get; set; }
        public History History { get; set; }

        public bool Register(string alias, Dictionary<string, object[]> arguments)
        {
            return Commands.Register(alias, arguments);      
        }

        public void ToggleConsole()
        {
            if (!IsConsoleShown)
            {
                container.SetActive(true);
                ClearInput();
                inputBox.onSubmit.AddListener(HandleEnteredCommand);

            }
            else
            {
                container.SetActive(false);
                inputBox.onSubmit.RemoveListener(HandleEnteredCommand);

            }

            IsConsoleShown = !IsConsoleShown;
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

            if (IsConsoleShown)
                inputBox.onSubmit.AddListener(HandleEnteredCommand);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleConsole();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentHistoryCommand = History.Previous();
                SetInputAutoText();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentHistoryCommand = History.Next();
                SetInputAutoText();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {

                if (!Commands.Autocomplete.Filtered.Contains(inputBox.text))
                {
                    currentText = inputBox.text;
                    Commands.Autocomplete.Reset();
                }

                Commands.Autocomplete.Filter(currentText);
                currentHistoryCommand = Commands.Autocomplete.Next();

                SetInputAutoText();
            }
        }

        private void SetInputAutoText()
        {
            if (!string.IsNullOrEmpty(currentHistoryCommand))
            {
                inputBox.text = currentHistoryCommand;
                inputBox.stringPosition = currentHistoryCommand.Length + 1;
                currentHistoryCommand = "";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //int linkIndex = TMP_TextUtilities.FindIntersectingLink(textBox, Input.mousePosition, null);

            //TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkIndex];

            //Debug.Log(linkInfo.GetLinkID());
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

            History.Push(Command.Alias + (string.IsNullOrEmpty(cmdArgs) ? " " : "") + cmdArgs);
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

