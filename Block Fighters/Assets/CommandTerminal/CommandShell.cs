using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandTerminal
{
    public struct CommandWrapper{
        GameObject methodBlockObject;
        bool isGameAction;
        bool isValid;

        public GameObject getMethodBlockObject(){return methodBlockObject;}
        public bool getIsGameAction(){return isGameAction;}
        public bool getIsValid(){return isValid;}

        public void setMethodBlockObject(GameObject methodBlockObject){this.methodBlockObject=methodBlockObject;}
        public void setIsGameAction(bool isGameAction){this.isGameAction=isGameAction;}
        public void setIsValid(bool isValid){this.isValid=isValid;}
    }

    public class CommandShell
    {
        Regex commandRegex;

        Character player;
        Character opponent;

        private Dictionary<string,GameObject> playerCommands=new Dictionary<string,GameObject>();
        private Dictionary<string,MethodInfo> builtInCommands=new Dictionary<string,MethodInfo>();
        private Dictionary<string,string> builtInCommandHelp=new Dictionary<string,string>();

        string IssuedErrorMessage;

        public CommandShell(){
            commandRegex=new Regex(@"^\w*(\w*||\d*)?\({1}\w*(\w?||\d?)*\){1}$");
        }

        public void IssueErrorMessage(string format, params object[] message) {
            IssuedErrorMessage = string.Format(format, message);
        }

        public string getIssuedErrorMessage(){return IssuedErrorMessage;}

        public void registerOpponent(Character opponent){this.opponent=opponent;}
        public void registerPlayer(Character player){this.player=player;}

        public Dictionary<string,GameObject> getPlayerCommands(){return playerCommands;}
        public Dictionary<string,MethodInfo> getBuiltInCommands(){return builtInCommands;}
        public Dictionary<string,string> getBuiltInCommandHelp(){return builtInCommandHelp;}

        public void registerPlayerCommands(List<GameObject> methodBlockObjects){
            foreach(GameObject methodBlockObject in methodBlockObjects){
                Block methodBlock=methodBlockObject.GetComponent<Block>();
                playerCommands.Add(methodBlock.getMethodName().ToUpper(),methodBlockObject);
            }

            builtInCommands.Add("LISTPLAYERCOMMANDS", this.GetType().GetMethod("listPlayerCommands"));
            builtInCommandHelp.Add("LISTPLAYERCOMMANDS", "lists player's current battle commands");
        }

        public void registerBuiltInCommands(){
            builtInCommands.Add("HELP",this.GetType().GetMethod("help"));
            builtInCommandHelp.Add("HELP","prints this help screen.\nAdding a command in the parameter displays help for that specific command");

            builtInCommands.Add("CLEAR",this.GetType().GetMethod("clear"));
            builtInCommandHelp.Add("CLEAR","clears terminal");

            builtInCommands.Add("FINISH",this.GetType().GetMethod("finishProgramming"));
            builtInCommandHelp.Add("FINISH","finishes programming your character's methods");

            builtInCommands.Add("PLAYERSTATS", this.GetType().GetMethod("playerStats"));
            builtInCommandHelp.Add("PLAYERSTATS", "check your current stats");

            builtInCommands.Add("OPPONENTSTATS", this.GetType().GetMethod("opponentStats"));
            builtInCommandHelp.Add("OPPONENTSTATS", "check your current stats");

            builtInCommands.Add("CHANGENAME", this.GetType().GetMethod("changeName"));
            builtInCommandHelp.Add("CHANGENAME", "changes your character's name");

            builtInCommands.Add("QUIT",this.GetType().GetMethod("quit"));
            builtInCommandHelp.Add("QUIT","quits game");
        }

        public bool clear(string[] argStringArray) {
            if (argStringArray.Length != 0) {
                IssueErrorMessage("Clear takes 1 parameter");
                return false;
            } else {
                Terminal.Buffer.Clear();
                return true;
            }
        }

        public bool help(string[] commandNames) {
            if (commandNames.Length == 0) {
                foreach (KeyValuePair<string, string> command in builtInCommandHelp) {
                    Terminal.log(TerminalLogType.Message, "{0}: {1}\n", command.Key, command.Value);
                }
                return true;
            }else if (!builtInCommandHelp.ContainsKey(commandNames[0])) {
                IssueErrorMessage("Built in command {0} could not be found.\n", commandNames[0].ToLower());
                return false;                
            }else{
                string commandName=commandNames[0];
                Terminal.log(TerminalLogType.Message,"{0}: {1}\n",commandName,builtInCommandHelp[commandName]);
                return true;
            }
        }

        public bool listPlayerCommands(string[] argStringArray) {
            if (argStringArray.Length != 0) {
                IssueErrorMessage("listplayercommands takes no parameters");
                return false;
            } else {
                foreach (KeyValuePair<string, GameObject> command in playerCommands) { Terminal.log(TerminalLogType.Message, "{0}()", command.Key); }
                return true;
            }
        }

        public bool finishProgramming(string[] argStringArray) {
            BlockProgrammerScript blockProgrammerScript = GameObject.FindGameObjectWithTag("BlockProgrammer")
                                                                    .GetComponent<BlockProgrammerScript>();
            blockProgrammerScript.applyMethodNames();
            if (!blockProgrammerScript.enabled) {
                IssueErrorMessage("Command {0} cannot be called when in battle", "finish");
                return false;
            } else if (!blockProgrammerScript.checkMinAmountOfMethods()) {
                IssueErrorMessage("Amount of methods must be above 0");
                return false;
            } else if (!blockProgrammerScript.checkMaxAmountOfMethods()) {
                IssueErrorMessage("Maximum of 4 methods allowed");
                return false;
            } else if (!blockProgrammerScript.checkForNonEmptyMethods()) {
                IssueErrorMessage("Methods must not be empty");
                return false;
            } else if (!blockProgrammerScript.finishProgrammingCheck()) {
                IssueErrorMessage("Not all methods have been named");
                return false;
            } else if (blockProgrammerScript.areInputFieldHandlersEmpty()) {
                IssueErrorMessage("Block inputs cannot be empty");
                return false;
            }else if (blockProgrammerScript.areMethodNamesLegal()) {
                IssueErrorMessage("Method names should start with a letter and contain no special characters");
                return false;
            }else{
                GameObject.FindGameObjectWithTag("Main").GetComponent<Main>().finishProgramming();
                return true;
            }
        }

        public bool quit(string[] argStringArray) {
            if (argStringArray.Length != 0) {
                IssueErrorMessage("Quit takes no parameters");
                return false;
            } else {
                SceneManager.LoadScene("Main Menu");
                return true;
            }
        }

        public bool changeName(string[] argStringArray) {
            if (argStringArray.Length != 1) {
                IssueErrorMessage("changeName takes 1 parameter");
                return false;
            } else {
                player.setCharacterName(argStringArray[0]);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>().updatePlayerHUDName();
                return true;
            }
        }

        public bool playerStats(string[] argStringArray) {
            if (argStringArray.Length != 0) {
                IssueErrorMessage("playerStats takes no parameters");
                return false;
            } else {
                Terminal.log(TerminalLogType.Message, "Character Name: {0}", player.getCharacterName());
                Terminal.log(TerminalLogType.Message,
                            "Max Health: {0} (Base Max Health: {1})",
                            player.getMaxHealth(),
                            player.getBaseMaxHealth());
                Terminal.log(TerminalLogType.Message,
                            "Attack: {0} (Base Attack: {1})",
                            player.getAttack(),
                            player.getBaseAttack());
                Terminal.log(TerminalLogType.Message,
                            "Defence: {0} (Base Defence: {1})",
                            player.getDefence(),
                            player.getBaseDefence());
                Terminal.log(TerminalLogType.Message,
                            "Speed: {0} (Base Speed: {1})",
                            player.getSpeed(),
                            player.getBaseSpeed());
                Terminal.log(TerminalLogType.Message,
                            "Stamina: {0} (Base Stamina: {1})\n",
                            player.getStamina(),
                            player.getBaseStamina());
                return true;
            }
        }

        public bool opponentStats(string[] argStringArray) {
            if (argStringArray.Length != 0) {
                IssueErrorMessage("opponentStats takes no parameters");
                return false;
            } else {
                Terminal.log(TerminalLogType.Message, "Character Name: {0}", opponent.getCharacterName());
                Terminal.log(TerminalLogType.Message,
                            "Max Health: {0} (Base Max Health: {1})",
                            opponent.getMaxHealth(),
                            opponent.getBaseMaxHealth());
                Terminal.log(TerminalLogType.Message,
                            "Attack: {0} (Base Attack: {1})",
                            opponent.getAttack(),
                            opponent.getBaseAttack());
                Terminal.log(TerminalLogType.Message,
                            "Defence: {0} (Base Defence: {1})",
                            opponent.getDefence(),
                            opponent.getBaseDefence());
                Terminal.log(TerminalLogType.Message,
                            "Speed: {0} (Base Speed: {1})",
                            opponent.getSpeed(),
                            opponent.getBaseSpeed());
                Terminal.log(TerminalLogType.Message,
                            "Stamina: {0} (Base Stamina: {1})\n",
                            opponent.getStamina(),
                            opponent.getBaseStamina());
                return true;
            }
        }

        public Tuple<string,string[]> parseCommandText(string commandText){
            string[] commandArray=commandText.Split('(',')');
            if(commandArray[1]==""){return Tuple.Create(commandArray[0],new string[0]{});
            }else{return Tuple.Create(commandArray[0],commandArray[1].Split(','));}
        }

        public CommandWrapper RunCommand(string commandText){
            commandText=commandText.ToUpper();
            CommandWrapper commandWrapper=new CommandWrapper();
            commandWrapper.setIsGameAction(false);
            if(commandRegex.IsMatch(commandText)){
                Tuple<string,string[]> parsedCommand=parseCommandText(commandText);
                string commandName=parsedCommand.Item1;
                string[] argStringArray=parsedCommand.Item2;
                if(playerCommands.ContainsKey(commandName)){
                    commandWrapper.setIsGameAction(true);
                    if (argStringArray.Length > 0) {
                        commandWrapper.setIsValid(false);
                        IssueErrorMessage("Methods can take no parameters");
                        return commandWrapper;
                    } else {
                        commandWrapper.setIsValid(true);
                        GameObject selectedMethodBlockObject = playerCommands[commandName];
                        commandWrapper.setMethodBlockObject(selectedMethodBlockObject);
                        return commandWrapper;
                    }
                }else if(builtInCommands.ContainsKey(commandName)){
                    commandWrapper.setIsValid((bool)builtInCommands[commandName].Invoke(this, new object[] { argStringArray }));
                    return commandWrapper;
                }else{
                    commandWrapper.setIsValid(false);
                    IssueErrorMessage("Command {0} not recognised",commandName);
                    return commandWrapper;
                }
            }else{
                commandWrapper.setIsValid(false);
                IssueErrorMessage("Command must be invoked in the form \"command(parameter)\" and start with a letter");
                return commandWrapper;
            }
        }
    }
}
