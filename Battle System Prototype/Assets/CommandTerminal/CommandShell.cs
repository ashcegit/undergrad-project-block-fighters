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
        }

        public void registerBuiltInCommands(){
            builtInCommands.Add("HELP",this.GetType().GetMethod("help"));
            builtInCommandHelp.Add("HELP","prints this help screen");

            builtInCommands.Add("CLEAR",this.GetType().GetMethod("clear"));
            builtInCommandHelp.Add("CLEAR","clears terminal");

            builtInCommands.Add("LISTPLAYERCOMMANDS",this.GetType().GetMethod("listPlayerCommands"));
            builtInCommandHelp.Add("LISTPLAYERCOMMANDS","lists player's current battle commands");

            builtInCommands.Add("FINISH",this.GetType().GetMethod("finishProgramming"));
            builtInCommandHelp.Add("FINISH","finishes programming your character's methods");

            builtInCommands.Add("QUIT",this.GetType().GetMethod("quit"));
            builtInCommandHelp.Add("QUIT","quits game");
        }

        public bool clear() {
            Terminal.Buffer.Clear();
            return true;
        }

        public bool help(string[] commandNames) {
            if(commandNames.Length>0){
                if(!builtInCommandHelp.ContainsKey(commandNames[0])) {
                    IssueErrorMessage("Built in command {0} could not be found.\n", commandNames[0].ToLower());
                    return false;
                }else{
                    string commandName=commandNames[0];
                    Terminal.log(TerminalLogType.Message,"{0}: {1}\n",commandName,builtInCommandHelp[commandName]);
                    return true;
                }
            }else{
                foreach(KeyValuePair<string,string> command in builtInCommandHelp){
                    Terminal.log(TerminalLogType.Message,"{0}: {1}\n",command.Key,command.Value);
                }
                return true;
            }
        }

        public bool listPlayerCommands(){
            foreach(KeyValuePair<string,GameObject> command in playerCommands){Terminal.log(TerminalLogType.Message,"{0}()",command.Key);}
            return true;
        }

        public bool finishProgramming(){
            BlockProgrammerScript blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer")
                                                                    .GetComponent<BlockProgrammerScript>();
            if(!blockProgrammerScript.enabled){
                IssueErrorMessage("Command {0} cannot be called when in battle","finish");
                return false;
            }else if(!blockProgrammerScript.checkMinAmountOfMethods()){
                IssueErrorMessage("Amount of methods must be above 0");
                return false;
            }else if(!blockProgrammerScript.checkMaxAmountOfMethods()){
                IssueErrorMessage("Maximum of 4 methods allowed");
                return false;
            }else if(!blockProgrammerScript.checkForNonEmptyMethods()){
                IssueErrorMessage("Methods must not be empty");
                return false;
            }else if(!blockProgrammerScript.finishProgrammingCheck()){
                IssueErrorMessage("Not all methods have been named");
                return false;
            }else{
                GameObject.FindGameObjectWithTag("Main").GetComponent<Main>().finishProgramming();
                return true;
            }
        }

        public void quit() {
            SceneManager.LoadScene("Main Menu");
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
                    GameObject selectedMethodBlockObject=playerCommands[commandName];
                    commandWrapper.setMethodBlockObject(selectedMethodBlockObject);
                    commandWrapper.setIsValid(true);
                    return commandWrapper;
                }else if(builtInCommands.ContainsKey(commandName)){
                    if(commandName=="help"){
                        if(argStringArray.Length>1||argStringArray.Length < 0){
                            commandWrapper.setIsValid(false);
                            IssueErrorMessage("Command {0} takes 0 or 1 parameters","help");
                            return commandWrapper;
                        }else{
                            commandWrapper.setIsValid((bool)builtInCommands["help"].Invoke(this,new object[]{argStringArray}));
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
