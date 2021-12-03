﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class BE2_BlocksSerializer
{
    public static void SaveBlock(I_BE2_Block block)
    {
        string blockString = SerializableToXML(BlockToSerializable(block));

        string instructionsPath = Application.dataPath + "/BlocksEngine2/Saves/";
        string fullPath = instructionsPath + "code.txt";

        StreamWriter sw = new StreamWriter(fullPath, false);
        sw.WriteLine(blockString);
        sw.Close();
    }

    public static BE2_SerializableBlock BlockToSerializable(I_BE2_Block block)
    {
        BE2_SerializableBlock serializableBlock = new BE2_SerializableBlock();

        serializableBlock.blockName = block.Transform.name;//.Instruction.GetType().ToString();//.Transform.name.Split(' ')[2];
        serializableBlock.position = block.Transform.position;

        if (block.Instruction is BE2_Op_Variable)
        {
            serializableBlock.isVariable = true;
            // v2.1 - using BE2_Text to enable usage of Text or TMP components
            BE2_Text varName = BE2_Text.GetBE2Text(block.Transform.GetChild(0).GetChild(0).GetChild(0));
            serializableBlock.varName = varName.text;
        }
        else
        {
            serializableBlock.isVariable = false;
        }

        foreach (I_BE2_BlockSection section in block.Layout.SectionsArray)
        {
            BE2_SerializableSection serializableSection = new BE2_SerializableSection();
            serializableBlock.sections.Add(serializableSection);

            foreach (I_BE2_BlockSectionHeaderInput input in section.Header.InputsArray)
            {
                BE2_SerializableInput serializableInput = new BE2_SerializableInput();
                serializableSection.inputs.Add(serializableInput);

                I_BE2_Block inputBlock = input.Transform.GetComponent<I_BE2_Block>();
                if (inputBlock != null)
                {
                    serializableInput.isOperation = true;
                    serializableInput.operation = BlockToSerializable(inputBlock);
                }
                else
                {
                    serializableInput.isOperation = false;
                    serializableInput.value = input.InputValues.stringValue;
                }
            }

            if (section.Body != null)
            {
                foreach (I_BE2_Block childBlock in section.Body.ChildBlocksArray)
                {
                    serializableSection.childBlocks.Add(BlockToSerializable(childBlock));
                }
            }
        }

        return serializableBlock;
    }

    public static string SerializableToXML(BE2_SerializableBlock serializableBlock)
    {
        // JsonUtility has a depth limitation but you can use another Json alternative
        return BE2_BlockXML.SBlockToXElement(serializableBlock).ToString();
    }

    public static void Load()
    {
        string instructionsPath = Application.dataPath + "/BlocksEngine2/Saves/";

        string fullPath = instructionsPath + "code.txt";

        var sr = new StreamReader(fullPath);
        var xmlString = sr.ReadToEnd();
        sr.Close();

        BE2_SerializableBlock serializableBlock = XMLToSerializable(xmlString);

        SerializableToBlock(serializableBlock, MonoBehaviour.FindObjectOfType<BE2_ProgrammingEnv>());
    }

    public static BE2_SerializableBlock XMLToSerializable(string blockString)
    {
        // v2.2 - bugfix: fixed empty blockString from XML file causing error on load
        blockString = blockString.Trim();
        if (blockString.Length > 1)
        {
            // JsonUtility has a depth limitation but you can use another Json alternative
            XElement xBlock = XElement.Parse(blockString);
            return BE2_BlockXML.XElementToSBlock(xBlock);
        }
        else
        {
            return null;
        }
    }

    static IEnumerator C_AddInputs(I_BE2_Block block, BE2_SerializableBlock serializableBlock, I_BE2_ProgrammingEnv programmingEnv)
    {
        yield return new WaitForEndOfFrame();

        I_BE2_BlockSection[] sections = block.Layout.SectionsArray;
        for (int s = 0; s < sections.Length; s++)
        {
            I_BE2_BlockSectionHeaderInput[] inputs = sections[s].Header.InputsArray;
            for (int i = 0; i < inputs.Length; i++)
            {
                BE2_SerializableInput serializableInput = serializableBlock.sections[s].inputs[i];
                if (serializableInput.isOperation)
                {
                    I_BE2_Block operation = SerializableToBlock(serializableInput.operation, programmingEnv);
                    BE2_DragDropManager.instance.CurrentSpot = inputs[i].Transform.GetComponent<I_BE2_Spot>();
                    operation.Transform.GetComponent<I_BE2_Drag>().OnPointerDown();
                    operation.Transform.GetComponent<I_BE2_Drag>().OnPointerUp();
                }
                else
                {
                    InputField inputText = inputs[i].Transform.GetComponent<InputField>();
                    Dropdown inputDropdown = inputs[i].Transform.GetComponent<Dropdown>();
                    if (inputText)
                    {
                        inputText.text = serializableInput.value;
                    }
                    else if (inputDropdown)
                    {
                        inputDropdown.value = inputDropdown.options.FindIndex(option => option.text == serializableInput.value);
                    }
                }
                inputs[i].UpdateValues();
            }

            I_BE2_BlockSectionBody body = sections[s].Body;
            if (body != null)
            {
                // add children
                foreach (BE2_SerializableBlock serializableChildBlock in serializableBlock.sections[s].childBlocks)
                {
                    I_BE2_Block childBlock = SerializableToBlock(serializableChildBlock, programmingEnv);
                    childBlock.Transform.SetParent(body.RectTransform);
                }
            }

            sections[s].Header.UpdateItemsArray();
            sections[s].Header.UpdateInputsArray();
        }
    }

    public static I_BE2_Block SerializableToBlock(BE2_SerializableBlock serializableBlock, I_BE2_ProgrammingEnv programmingEnv)
    {
        I_BE2_Block block = null;

        if (serializableBlock != null)
        {
            // ---> standardize prefabs name to be same as instructino class name
            string prefabName = serializableBlock.blockName;//"Block " + serializableBlock.blockName.Split('_')[1] + " " + serializableBlock.blockName.Split('_')[2];
            GameObject loadedPrefab = Resources.Load<GameObject>("Blocks/" + prefabName);
            if (!loadedPrefab)
                loadedPrefab = Resources.Load<GameObject>("Blocks/Custom/" + prefabName);

            if (loadedPrefab)
            {
                GameObject blockGo = MonoBehaviour.Instantiate(
                    loadedPrefab,
                    serializableBlock.position,
                    Quaternion.identity,
                    programmingEnv.Transform) as GameObject;

                blockGo.name = prefabName;

                block = blockGo.GetComponent<I_BE2_Block>();

                if (serializableBlock.isVariable)
                {
                    // v2.1 - using BE2_Text to enable usage of Text or TMP components
                    //                         | block     | section   | header    | text      |
                    //Text newVarName = block.Transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
                    BE2_Text newVarName = BE2_Text.GetBE2Text(block.Transform.GetChild(0).GetChild(0).GetChild(0));
                    newVarName.text = serializableBlock.varName;

                    if (!BE2_VariablesManager.instance.ContainsVariable(serializableBlock.varName))
                    {
                        BE2_VariablesManager.instance.CreateAndAddVarToPanel(serializableBlock.varName);
                    }
                }

                (block as BE2_Block).AddSpotsToManager();

                // add inputs
                BE2_ExecutionManager.instance.StartCoroutine(C_AddInputs(block, serializableBlock, programmingEnv));

                if (block.Type == BlockTypeEnum.trigger)
                {
                    BE2_ExecutionManager.instance.AddToBlocksStackArray(block.Instruction.InstructionBase.BlocksStack, programmingEnv.TargetObject);
                }
            }
        }

        return block;
    }

}
