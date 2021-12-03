using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[CustomEditor(typeof(BE2_Inspector))]
public class BE2_Editor_Inspector : Editor
{
    BE2_Inspector inspector;

    void OnEnable()
    {
        inspector = (BE2_Inspector)target;

        if (inspector.inputValues == null)
            inspector.inputValues = new string[0];
    }

    bool tryAddInstruction = false;
    string instructionCompleteName = "";
    Transform newBlockTransform = null;
    public override void OnInspectorGUI()
    {
        DrawSeparator();
        EditorGUILayout.LabelField("Block Builder", EditorStyles.boldLabel);

        inspector.instructionName = EditorGUILayout.TextField("Instruction Name", inspector.instructionName);
        inspector.blockType = (BlockTypeEnum)EditorGUILayout.EnumPopup("Block Type", inspector.blockType);
        inspector.blockColor = EditorGUILayout.ColorField("Blcok Color", inspector.blockColor);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Block Header Markup");
        EditorGUILayout.HelpBox(
            "Write the header text and inputs in a single line. Additional headers in new lines. \n" +
            "possible input types are $text or $dropdown"
            , MessageType.Info);

        inspector.blockHeaderMarkup = EditorGUILayout.TextArea(inspector.blockHeaderMarkup, GUILayout.MaxHeight(75));

        if (inspector.blockHeaderMarkup != "")
        {
            EditorGUILayout.HelpBox(
            "Below you can define the text $input default value or $dropdown option values separated by comma."
            , MessageType.Info);
            List<int> inputMarkIndexes = inspector.AllIndexesOf(inspector.blockHeaderMarkup, "$text");
            inputMarkIndexes.AddRange(inspector.AllIndexesOf(inspector.blockHeaderMarkup, "$dropdown"));
            BE2_ArrayUtils.Resize(ref inspector.inputValues, inputMarkIndexes.Count);
            for (int i = 0; i < inputMarkIndexes.Count; i++)
            {
                inspector.inputValues[i] = EditorGUILayout.TextField("$input " + i, inspector.inputValues[i]);
            }
        }

        if (GUILayout.Button("Build Block"))
        {
            // ### instantiate block ###
            newBlockTransform = inspector.BuildAndInstantiateBlock(inspector.instructionName);

            // ### create instruction ###
            instructionCompleteName = inspector.CreateInstructionScript(inspector.instructionName);
            tryAddInstruction = true;
        }
        if (tryAddInstruction)
        {
            if (!EditorApplication.isCompiling)
            {
                Debug.Log("+ End creating instruction");
                tryAddInstruction = false;
                // ### add instruction to block ###
                inspector.TryAddInstructionToBlock(instructionCompleteName, newBlockTransform.gameObject);

                // ### Add block to selection menu ###
                inspector.AddBlockToSelectionMenu(newBlockTransform);
            }
        }

        DrawSeparator();

        EditorGUILayout.LabelField("Template Block Parts", EditorStyles.boldLabel);
        inspector.BlockTemplate = (GameObject)EditorGUILayout.ObjectField("Block Template", inspector.BlockTemplate, typeof(GameObject), false);
        inspector.SimpleTemplate = (GameObject)EditorGUILayout.ObjectField("Simple Template", inspector.SimpleTemplate, typeof(GameObject), false);
        inspector.TriggerTemplate = (GameObject)EditorGUILayout.ObjectField("Trigger Template", inspector.TriggerTemplate, typeof(GameObject), false);
        inspector.OperationTemplate = (GameObject)EditorGUILayout.ObjectField("Operation Template", inspector.OperationTemplate, typeof(GameObject), false);
        inspector.SectionTemplate = (GameObject)EditorGUILayout.ObjectField("Section Template", inspector.SectionTemplate, typeof(GameObject), false);
        inspector.HeaderTemplate = (GameObject)EditorGUILayout.ObjectField("Block Header", inspector.HeaderTemplate, typeof(GameObject), false);
        inspector.HeaderMiddleTemplate = (GameObject)EditorGUILayout.ObjectField("Block Middle Header", inspector.HeaderMiddleTemplate, typeof(GameObject), false);
        inspector.BodyMiddleTemplate = (GameObject)EditorGUILayout.ObjectField("Block Middle Body", inspector.BodyMiddleTemplate, typeof(GameObject), false);
        inspector.BodyEndTemplate = (GameObject)EditorGUILayout.ObjectField("Block End Body", inspector.BodyEndTemplate, typeof(GameObject), false);
        inspector.OuterAreaTemplate = (GameObject)EditorGUILayout.ObjectField("Block Outer Area", inspector.OuterAreaTemplate, typeof(GameObject), false);
        inspector.DropdownTemplate = (GameObject)EditorGUILayout.ObjectField("Dropdown Template", inspector.DropdownTemplate, typeof(GameObject), false);
        inspector.InputFieldTemplate = (GameObject)EditorGUILayout.ObjectField("InputField Template", inspector.InputFieldTemplate, typeof(GameObject), false);
        inspector.LabelTextTemplate = (GameObject)EditorGUILayout.ObjectField("LabelText Template", inspector.LabelTextTemplate, typeof(GameObject), false);

        DrawSeparator();
    }

    void DrawSeparator()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.width + 15, rect.y));
        EditorGUILayout.EndHorizontal();
    }

    void DrawThinSeparator()
    {
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.width + 15, rect.y));
        EditorGUILayout.EndHorizontal();
    }

    void DrawVerticalSeparator()
    {
        var rect = EditorGUILayout.BeginVertical();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x, rect.yMin));
        EditorGUILayout.EndVertical();
    }
}