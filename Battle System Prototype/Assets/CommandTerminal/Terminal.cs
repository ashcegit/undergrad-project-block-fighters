using UnityEngine;
using System.Text;
using System.Collections;
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
        float ToggleSpeed = 360;

        int BufferSize           = 512;

        Font ConsoleFont;
        string InputCaret        = ">";
        bool showGUIButtons;
        bool RightAlignButtons;

        float InputContrast;

        Color BackgroundColor    = Color.black;
        Color ForegroundColor    = Color.white;
        Color ShellColor         = Color.white;
        Color InputColor         = Color.cyan;
        Color WarningColor       = Color.yellow;
        Color ErrorColor         = Color.red;

        TerminalState state;
        TextEditor editorState;
        bool inputFix;
        bool moveCursor;
        bool initialOpen; // Used to focus on TextField when console opens
        Rect window;
        float currentOpenT;
        float openTarget;
        string commandText;
        string cachedCommandText;
        Vector2 scrollPosition;
        GUIStyle window_style;
        GUIStyle labelStyle;
        GUIStyle input_style;

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
        }

        public void setState(TerminalState newState) {
            inputFix = true;
            cachedCommandText = commandText;
            commandText = "";

            switch (newState) {
                case TerminalState.Close: {
                    openTarget = 0;
                    break;
                }
                case TerminalState.ReadOnly:
                case TerminalState.Write:
                default: {
                    openTarget = Screen.height;
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

            // Hook Unity log events
            Application.logMessageReceived += handUnityLog;
        }

        void OnDisable() {
            Application.logMessageReceived -= handUnityLog;
        }

        public void initShell(Player player,Opponent opponent,GameScript gameScript){
            this.gameScript=gameScript;
            Shell.registerPlayer(player);
            Shell.registerOpponent(opponent);
            Shell.registerPlayerCommands();
            Shell.registerBuiltInCommands();

            if (issuedError) {
                log(TerminalLogType.Error, "Error: {0}", Shell.getIssuedErrorMessage());
            }

            Autocomplete.register(player.getCharacterName());
            Autocomplete.register(opponent.getCharacterName());

            foreach(var command in Shell.getPlayerCommands()) {
                Autocomplete.register(command.Key);
            }
            foreach(var command in Shell.getBuiltInCommands()){
                Autocomplete.register(command.Key);
            }
        }

        void Start() {
            ConsoleFont = Font.CreateDynamicFontFromOSFont("Courier New", 25);

            commandText = "";
            cachedCommandText = commandText;

            setupWindow();
            setupInput();
            setupLabels();

            terminalLoaded=true;
            setState(TerminalState.Write);
        }

        public bool getTerminalLoaded(){return terminalLoaded;}

        void OnGUI() {
            initialOpen = true;

            if (showGUIButtons) {
                drawGUIButtons();
            }

            if (isClosed) {
                return;
            }

            handleOpenness();
            window = GUILayout.Window(88, window, drawConsole, "", window_style);
        }

        void setupWindow() {
            window = new Rect(Screen.width/2, 0, Screen.width/2, Screen.height);

            // Set background color
            Texture2D background_texture = new Texture2D(1, 1);
            background_texture.SetPixel(0, 0, BackgroundColor);
            background_texture.Apply();

            window_style = new GUIStyle();
            window_style.normal.background = background_texture;
            window_style.padding = new RectOffset(4, 4, 4, 4);
            window_style.normal.textColor = ForegroundColor;
            window_style.font = ConsoleFont;
        }

        void setupLabels() {
            labelStyle = new GUIStyle();
            labelStyle.font = ConsoleFont;
            labelStyle.normal.textColor = ForegroundColor;
            labelStyle.wordWrap = true;
        }

        void setupInput() {
            input_style = new GUIStyle();
            input_style.padding = new RectOffset(4, 4, 4, 4);
            input_style.font = ConsoleFont;
            input_style.fixedHeight = ConsoleFont.fontSize * 1.6f;
            input_style.normal.textColor = InputColor;

            var dark_background = new Color();
            dark_background.r = BackgroundColor.r - InputContrast;
            dark_background.g = BackgroundColor.g - InputContrast;
            dark_background.b = BackgroundColor.b - InputContrast;
            dark_background.a = 0.5f;

            Texture2D input_background_texture = new Texture2D(1, 1);
            input_background_texture.SetPixel(0, 0, dark_background);
            input_background_texture.Apply();
            input_style.normal.background = input_background_texture;
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
                GUILayout.Label(InputCaret, input_style, GUILayout.Width(ConsoleFont.fontSize));
            }

            if(state==TerminalState.Write){
                GUI.SetNextControlName("commandText_field");
                commandText = GUILayout.TextField(commandText, input_style);

                if (inputFix && commandText.Length > 0) {
                    commandText = cachedCommandText; // Otherwise the TextField picks up the ToggleHotkey character event
                    inputFix = false;                  // Prevents checking string Length every draw call
                }

                if (initialOpen) {
                    GUI.FocusControl("commandText_field");
                    initialOpen = false;
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

        void drawGUIButtons() {
            int size = ConsoleFont.fontSize;
            float x_position = RightAlignButtons ? Screen.width - 7 * size : 0;

            // 7 is the number of chars in the button plus some padding, 2 is the line height.
            // The layout will resize according to the font size.
            GUILayout.BeginArea(new Rect(x_position, currentOpenT, 7 * size, size * 2));
            GUILayout.BeginHorizontal();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void handleOpenness() {
            float dt = ToggleSpeed * Time.deltaTime;

            if (currentOpenT < openTarget) {
                currentOpenT += dt;
                if (currentOpenT > openTarget) currentOpenT = openTarget;
            } else if (currentOpenT > openTarget) {
                currentOpenT -= dt;
                if (currentOpenT < openTarget) currentOpenT = openTarget;
            } else {
                return; // Already at target
            }

            window = new Rect(Screen.width/2, 0, Screen.width/2,Screen.height);
        }

        void EnterCommand() {
            log(TerminalLogType.Input, "{0}", commandText);
            CommandWrapper commandWrapper=Shell.RunCommand(commandText);
            History.Push(commandText);
            if(commandWrapper.getIsGameAction()&&commandWrapper.getIsValid()){
                gameScript.setPlayerGameActions(commandWrapper.getGameActions());
            }else if(!commandWrapper.getIsValid()){
                log(TerminalLogType.Error, "Error: {0}", Shell.getIssuedErrorMessage());
            }
            commandText = "";
            scrollPosition.y = int.MaxValue;
        }



        void CompleteCommand() {
            string headText = commandText;
            string[] completionBuffer = Autocomplete.complete(ref headText);
            int completionLength = completionBuffer.Length;

            if (completionLength == 1) {
                commandText = headText + completionBuffer[0];
            } else if (completionLength > 1) {
                // Print possible completions
                log(string.Join("    ", completionBuffer));
                scrollPosition.y = int.MaxValue;
            }
        }

        void CursorToEnd() {
            if (editorState==null) {
                editorState=(TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            }
            editorState.MoveCursorToPosition(new Vector2(999, 999));
        }

        void handUnityLog(string message, string stack_trace, LogType type) {
            Buffer.handleLog(message, stack_trace, (TerminalLogType)type);
            scrollPosition.y = int.MaxValue;
        }

        Color getLogColour(TerminalLogType type) {
            switch (type) {
                case TerminalLogType.Message: return ForegroundColor;
                case TerminalLogType.Warning: return WarningColor;
                case TerminalLogType.Input: return InputColor;
                case TerminalLogType.ShellMessage: return ShellColor;
                default: return ErrorColor;
            }
        }
    }
}
