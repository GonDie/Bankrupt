using Bankrupt.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateScriptableObjects
{
    [MenuItem("Assets/Create/Behaviours/BehaviourImpulsivo")]
    public static void CreateImpulsivo()
    {
        ScriptableObjectUtility.CreateAsset<BehaviourImpulsivo>();
    }

    [MenuItem("Assets/Create/Behaviours/BehaviourExigente")]
    public static void CreateExigente()
    {
        ScriptableObjectUtility.CreateAsset<BehaviourExigente>();
    }

    [MenuItem("Assets/Create/Behaviours/BehaviourCauteloso")]
    public static void CreateCauteloso()
    {
        ScriptableObjectUtility.CreateAsset<BehaviourCauteloso>();
    }

    [MenuItem("Assets/Create/Behaviours/BehaviourAleatorio")]
    public static void CreateAleatorio()
    {
        ScriptableObjectUtility.CreateAsset<BehaviourAleatorio>();
    }
}
