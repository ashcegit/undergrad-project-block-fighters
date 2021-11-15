using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CommandTerminal
{
    public struct CommandWrapper{
        List<GameAction> gameActions;
        bool isGameAction;
        bool isValid;

        public List<GameAction> getGameActions(){return gameActions;}
        public bool getIsGameAction(){return isGameAction;}
        public bool getIsValid(){return isValid;}

        public void setGameActions(List<GameAction> gameActions){this.gameActions=gameActions;}
        public void setIsGameAction(bool isGameAction){this.isGameAction=isGameAction;}
        public void setIsValid(bool isValid){this.isValid=isValid;}
    }

    public class CommandShell
    {
        Regex commandRegex;

        Player player;
        Opponent opponent;

        private Dictionary<string,MethodInfo> playerCommands=new Dictionary<string,MethodInfo>();
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

        public void registerOpponent(Opponent opponent){this.opponent=opponent;}
        public void registerPlayer(Player player){this.player=player;}

        public Dictionary<string,MethodInfo> getPlayerCommands(){return playerCommands;}
        public Dictionary<string,MethodInfo> getBuiltInCommands(){return builtInCommands;}
        public Dictionary<string,string> getBuiltInCommandHelp(){return builtInCommandHelp;}

        public void registerPlayerCommands(){
            MethodInfo[] playerMethodInfos=player.GetType().GetMethods(BindingFlags.DeclaredOnly|
                                                                       BindingFlags.Public|
                                                                       BindingFlags.Instance);
            foreach(MethodInfo playerMethodInfo in playerMethodInfos){
                playerCommands.Add(playerMethodInfo.Name,playerMethodInfo);
            }
        }
        public void registerBuiltInCommands(){
            builtInCommands.Add("help",this.GetType().GetMethod("help"));
            builtInCommandHelp.Add("help","prints this help screen");

            builtInCommands.Add("clear",this.GetType().GetMethod("clear"));
            builtInCommandHelp.Add("clear","clears terminal");

            builtInCommands.Add("listPlayerCommands",this.GetType().GetMethod("listPlayerCommands"));
            builtInCommandHelp.Add("listPlayerCommands","lists player's current battle commands");

            builtInCommands.Add("quit",this.GetType().GetMethod("quit"));
            builtInCommandHelp.Add("quit","quits game");
        }

        public bool clear() {
            Terminal.Buffer.Clear();
            return true;
        }

        public bool help(string[] commandNames) {
            if(commandNames.Length>0){
                if(!builtInCommandHelp.ContainsKey(commandNames[0])) {
                    IssueErrorMessage("Built in command {0} could not be found.", commandNames[0].ToLower());
                    return false;
                }else{
                    string commandName=commandNames[0];
                    Terminal.log(TerminalLogType.Message,"{0}: {1}",commandName,builtInCommandHelp[commandName]);
                    return true;
                }
            }else{
                foreach(KeyValuePair<string,string> command in builtInCommandHelp){
                    Terminal.log(TerminalLogType.Message,"{0}: {1}",command.Key,command.Value);
                }
                return true;
            }
        }

        public bool listPlayerCommands(){
            foreach(KeyValuePair<string,MethodInfo> command in playerCommands){Terminal.log(TerminalLogType.Message,"{0}('target')",command.Key);}
            return true;
        }

        public void quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
        
        public Tuple<string,string[]> parseCommandText(string commandText){
            string[] commandArray=commandText.Split('(',')');
            if(commandArray[1]==""){return Tuple.Create(commandArray[0],new string[0]{});
            }else{return Tuple.Create(commandArray[0],commandArray[1].Split(','));}
        }

        public CommandWrapper RunCommand(string commandText){
            CommandWrapper commandWrapper=new CommandWrapper();
            commandWrapper.setIsGameAction(false);
            if(commandRegex.IsMatch(commandText)){
                Tuple<string,string[]> parsedCommand=parseCommandText(commandText);
                string commandName=parsedCommand.Item1;
                string[] argStringArray=parsedCommand.Item2;
                if(playerCommands.ContainsKey(commandName)){
                    if(argStringArray.Length!=playerCommands[commandName].GetParameters().Length){
                        commandWrapper.setIsValid(false);
                        IssueErrorMessage("Command {0} takes {1} parameter(s)",commandName,
                                                                            playerCommands[commandName].GetParameters().Length);
                        return commandWrapper;
                    }
                    commandWrapper.setIsGameAction(true);
                    List<Character> targets=new List<Character>();
                    foreach(string arg in argStringArray){
                        if(arg.Equals(player.getCharacterName())){targets.Add(player);}
                        else if(arg.Equals(player.getCharacterName())){targets.Add(opponent);}
                        else{
                            commandWrapper.setIsValid(false);
                            IssueErrorMessage("Parameter {0} not recognisable target",arg);
                            return commandWrapper;
                        }
                    }
                    MethodInfo selectedMethod=playerCommands[commandName];
                    commandWrapper.setGameActions((List<GameAction>)selectedMethod.Invoke((Player)player,targets.ToArray()));
                    commandWrapper.setIsValid(true);
                    return commandWrapper;
                }else if(builtInCommands.ContainsKey(commandName)){
                    if(commandName=="help"){
                        if(argStringArray.Length>1||argStringArray.Length < 0){
                            commandWrapper.setIsValid(false);
                            IssueErrorMessage("Command {0} takes 0 or 1 parameters","help");
                            return commandWrapper;
                        }else{
                            commandWrapper.setIsValid((bool)builtInCommands["help"].Invoke(this,argStringArray));
                            return commandWrapper;
                        }
                    }
                    else if(argStringArray.Length!=builtInCommands[commandName].GetParameters().Length){
                        commandWrapper.setIsValid(false);
                        IssueErrorMessage("Command {0} takes {1} parameter(s)",commandName,builtInCommands[commandName].GetParameters().Length);
                        return commandWrapper;
                    }else{
                        commandWrapper.setIsValid((bool)builtInCommands[commandName].Invoke(this,argStringArray));
                        return commandWrapper;
                    }
                    
                }else{
                    commandWrapper.setIsValid(false);
                    IssueErrorMessage("Command {0} not recognised",commandName);
                    return commandWrapper;
                }
            }else{
                commandWrapper.setIsValid(false);
                IssueErrorMessage("Command must be invoked in the form command(parameter)");
                return commandWrapper;
            }
        }
    }
}
