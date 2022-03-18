using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

namespace CommandTerminal
{
    public enum TerminalState
    {
        Close,
        ReadOnly,
        Write
    }

    public class Terminal : MonoBehaviour
    {
        float ToggleSpeed        = 1200;
        
        int BufferSize           = 512;

        Font ConsoleFont;
        string InputCaret        = ">";
        bool showGUIButtons;
        bool RightAlignButtons;

        float InputContrast;

        Color BackgroundColour  = Color.black;
        Color ForegroundColour  = Color.white;
        Color ShellColour       = Color.white;
        Color InputColour       = Color.green;
        Color HeaderColour      = Color.cyan;
        Color ControlColour     = new Color(0.2F, 0.3F, 0.4F); //orange
        Color ActionColour      = Color.red;
        Color ErrorColour       = Color.red;

        TerminalState state;
        TextEditor editorState;
        bool inputFix;
        bool moveCursor;
        bool firstOpen; // Used to focus on TextField when console opens
        Rect window;
        RectTransform terminalRect;
        Vector2 terminalRectCorner;
        float currentOpenT;
        float openTarget;
        string commandText;
        string cachedCommandText;
        static Vector2 scrollPosition;
        GUIStyle window_style;
        GUIStyle labelStyle;
        GUIStyle inputStyle;

        GameScript gameScript;
        bool terminalLoaded;

        public static CommandLog Buffer { get; private set; }
        public static CommandShell Shell { get; private set; }
        public static CommandHistory History { get; private set; }
        public static CommandAutocomplete Autocomplete { get; private set; }

        public static bool issuedError {
            get { return Shell.getIssuedErrorMessage() != null; }
        }

        public bool isClosed {
            get { return state == TerminalState.Close && Mathf.Approximately(currentOpenT, openTarget); }
        }

        public static void log(string format, params object[] message) {
            log(TerminalLogType.ShellMessage, format, message);
        }

        public static void log(TerminalLogType type, string format, params object[] message) {
            Buffer.handleLog(string.Format(format, message), type);
            scrollPosition.y=int.MaxValue;
        }

        public void setState(TerminalState newState) {
            inputFix = true;
            
            
            switch (newState) {
                case TerminalState.Close: {
                    openTarget = 0;
                    commandText = "";
                    break;
                }
                case TerminalState.ReadOnly:
                    openTarget = terminalRect.sizeDelta.y;
                    cachedCommandText = commandText;
                    commandText = "";
                    break;
                case TerminalState.Write:
                default: {
                    firstOpen = true;
                    openTarget = terminalRect.sizeDelta.y;
                    break;
                }
            }
            state = newState;
        }

        public TerminalState getState(){return state;}

        void OnEnable() {
            Buffer = new CommandLog(BufferSize);
            Shell = new CommandShell();
            History = new CommandHistory();
            Autocomplete = new CommandAutocomplete();
        }

        public void initShell(GameScript gameScript){
            Shell = new CommandShell();
            Autocomplete = new CommandAutocomplete();
            this.gameScript=gameScript;
            
            Shell.registerBuiltInCommands();

            Shell.registerPlayer(gameScript.getPlayer());
            Shell.registerOpponent(gameScript.getOpponent());

            foreach (var command in Shell.getBuiltInCommands()){
                Autocomplete.register(command.Key);
            }           
        }

        public void registerCharacterCommands(List<GameObject> methodBlockObjects){

            Shell.registerPlayerCommands(methodBlockObjects);

            Autocomplete = new CommandAutocomplete();
            foreach(var command in Shell.getBuiltInCommands()) {
                Autocomplete.register(command.Key);
            }

            foreach(var command in Shell.getPlayerCommands()) {
                Autocomplete.register(command.Key);
            }

        }

        void Start() {
            ConsoleFont = (Font)Resources.Load("Fonts/ThaleahFat_TTF");

            terminalRect=GameObject.Find("Terminal Rect").GetComponent<RectTransform>();

            Vector3[] fourCornerArray = { new Vector3(), new Vector3(), new Vector3(), new Vector3() };

            terminalRect.GetWorldCorners(fourCornerArray);
            terminalRectCorner = fourCornerArray[0];

            commandText = "";
            cachedCommandText = commandText;

            setupWindow();
            setupInput();
            setupLabels();

            terminalLoaded=true;
        }

        public bool getTerminalLoaded(){return terminalLoaded;}

        void OnGUI() {
            handleOpenness();
            window = GUILayout.Window(88, window, drawConsole, "", window_style);
        }

        void setupWindow() {        
            // Set background color
            Texture2D background_texture = new Texture2D(1, 1);
            Color dark_background = new Color();
            dark_background.r = BackgroundColour.r;
            dark_background.g = BackgroundColour.g;
            dark_background.b = BackgroundColour.b;
            dark_background.a = 0.8f;
            background_texture.SetPixel(0, 0, dark_background);
            background_texture.Apply();

            window_style = new GUIStyle();
            window_style.normal.background = background_texture;
            window_style.padding = new RectOffset(4, 4, 4, 4);
            window_style.normal.textColor = ForegroundColour;
            window_style.font = ConsoleFont;
            window_style.fontSize=45;
        }

        void setupLabels() {
            labelStyle = new GUIStyle();
            labelStyle.font = ConsoleFont;
            labelStyle.fontSize=45;
            labelStyle.normal.textColor = ForegroundColour;
            labelStyle.wordWrap = true;
        }

        void setupInput() {
            inputStyle = new GUIStyle();
            inputStyle.padding = new RectOffset(4, 4, 4, 4);
            inputStyle.font = ConsoleFont;
            inputStyle.fontSize=45;
            inputStyle.fixedHeight = ConsoleFont.fontSize * 4f;
            inputStyle.normal.textColor = InputColour;
        }

        void drawConsole(int Window2D) {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            drawLogs();
            GUILayout.EndScrollView();

            if (moveCursor) {
                CursorToEnd();
                moveCursor = false;
            }

            if(state==TerminalState.Write){
                if (Event.current.Equals(Event.KeyboardEvent("return"))) {
                    EnterCommand();
                } else if (Event.current.Equals(Event.KeyboardEvent("up"))) {
                    commandText = History.Previous();
                    moveCursor = true;
                } else if (Event.current.Equals(Event.KeyboardEvent("down"))) {
                    commandText = History.Next();
                } else if (Event.current.Equals(Event.KeyboardEvent("tab"))) {
                    CompleteCommand();
                    moveCursor = true; // Wait till next draw call
                }
            }


            GUILayout.BeginHorizontal();

            if (InputCaret != "") {
                GUILayout.Label(InputCaret, inputStyle, GUILayout.Width(ConsoleFont.fontSize));
            }

            if(state==TerminalState.Write){
                GUI.SetNextControlName("commandText_field");
                commandText = GUILayout.TextField(commandText, inputStyle);

                if (firstOpen) {
                    GUI.FocusControl("commandText_field");
                    firstOpen = false;
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        void drawLogs() {
            foreach (var log in Buffer.getLogs()) {
                labelStyle.normal.textColor = getLogColour(log.type);
                GUILayout.Label(log.message, labelStyle);
            }
        }

        void handleOpenness() {
            //handles smooth opening of terminal
            float dt = ToggleSpeed * Time.deltaTime;

            if (currentOpenT < openTarget) {
                currentOpenT += dt;
                if (currentOpenT > openTarget) currentOpenT = openTarget;
            } else if (currentOpenT > openTarget) {
                currentOpenT -= dt;
                if (currentOpenT < openTarget) currentOpenT = openTarget;
            } else {
                return; // Already at target openness
            }

            window = new Rect(terminalRectCorner.x,terminalRectCorner.y,terminalRect.sizeDelta.x, currentOpenT);
        }

        void EnterCommand() {
            log(TerminalLogType.Input, "{0}\n", commandText);
            CommandWrapper commandWrapper=Shell.RunCommand(commandText);
            History.Push(commandText);
            if(commandWrapper.getIsGameAction()&&commandWrapper.getIsValid()){
                gameScript.setPlayerMethod(commandWrapper.getMethodBlockObject());
            }else if(!commandWrapper.getIsValid()){
                log(TerminalLogType.Error, "Error: {0}", Shell.getIssuedErrorMessage());
            }
            commandText = "";
            scrollPosition.y = int.MaxValue;
        }



        void CompleteCommand() {
            string headText = commandText;
            headText=headText.ToUpper();
            string[] completionBuffer = Autocomplete.complete(ref headText);
            int completionLength = completionBuffer.Length;

            if (completionLength == 1) {
                commandText = headText + completionBuffer[0]+"()";
            } else if (completionLength > 1) {
                // Print possible completions
                log(TerminalLogType.ShellMessage,
                    "{0}\n",
                    string.Join("    ", completionBuffer));
                scrollPosition.y = int.MaxValue;
            }
        }

        void CursorToEnd() {
            if (editorState==null) {
                editorState=(TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            }
            editorState.MoveCursorToPosition(new Vector2(999, 999));
        }

        Color getLogColour(TerminalLogType type) {
            switch (type) {
                case TerminalLogType.Message: return ForegroundColour;
                case TerminalLogType.Control:  return ControlColour;
                case TerminalLogType.Action: return ActionColour;
                case TerminalLogType.Header: return HeaderColour;
                case TerminalLogType.Input: return InputColour;
                case TerminalLogType.ShellMessage: return ShellColour;
                default: return ErrorColour;
            }
        }
    }
}
