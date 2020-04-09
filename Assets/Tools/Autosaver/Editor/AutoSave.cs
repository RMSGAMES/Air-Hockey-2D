﻿using UnityEngine;
using UnityEditor;
using System.Timers;
using System;
using System.Diagnostics;
using LOG = UnityEngine.Debug;

[InitializeOnLoad]
public class AutoSave : EditorWindow
{
    public static AutoSave instance = null;
    protected static Timer timer = null;
    protected static int hierarchyChangeCount = 0;
    protected static string logoPath = "Assets/Sixpolys/SIXP Autosaver/Editor/SixpolysLogo.png";
    protected static bool _saveNow = false;
    protected static bool savedBeforePlay = false;
    protected static bool saveAfterPlay = false;
    protected static Stopwatch stw1 = null;

    [MenuItem("Window/Autosave Settings")]
    public static void ShowWindow()
    {
        var window = GetWindow<AutoSave>();
        window.maxSize = new Vector2(window.maxSize.x, 50);
        window.minSize = new Vector2(0, 50);
    }

    public static void LoadPreferences()
    {
        if (AutoSavePreferences.autosaveEnabled)
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = AutoSavePreferences.saveInterval;
                timer.Elapsed += new ElapsedEventHandler(timerFired);
                timer.Start();
            }
            else
            {
                if (timer.Interval != AutoSavePreferences.saveInterval)
                {
                    timer.Interval = AutoSavePreferences.saveInterval;
                }
            }
        }
        else
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
        EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
        EditorApplication.playmodeStateChanged -= playModeChanged;
        EditorApplication.hierarchyWindowChanged += HierarchyChanged;
        EditorApplication.playmodeStateChanged += playModeChanged;

        if (instance != null)
        {
            instance.Repaint();
        }
    }


    public static void playModeChanged()
    {
        if (AutoSavePreferences.saveBeforeRun && EditorApplication.isPlayingOrWillChangePlaymode && !savedBeforePlay)
        {
            savedBeforePlay = true;
            executeSave();
        }
        else if (!EditorApplication.isPaused && !EditorApplication.isPlaying)
        {
            if (saveAfterPlay)
            {
                executeSave();
            }
        }
    }

    public static void HierarchyChanged()
    {
        if (AutoSavePreferences.saveOnHierarchyChanges && !EditorApplication.isPlaying)
        {
            hierarchyChangeCount++;
            if (hierarchyChangeCount >= AutoSavePreferences.hierarchyChangeCountTrigger)
            {
                hierarchyChangeCount = 0;
                executeSave();
            }
        }
    }

    public static void timerFired(object sender, ElapsedEventArgs args)
    {
        if (!_saveNow)
        {
            _saveNow = true;
        }
    }

    public static void executeSave()
    {
        stw1.Stop();
        stw1.Reset();

        if (EditorApplication.isCompiling || BuildPipeline.isBuildingPlayer)
        {
            return;
        }

        // don't save during running game
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            saveAfterPlay = true;
            stw1.Start();
            return;
        }
        saveAfterPlay = false;

        // save untitled scene?
        string sceneName = EditorApplication.currentScene;

        if ((sceneName == "" || sceneName.StartsWith("Untitled")) && !AutoSavePreferences.saveUnnamedNewScene)
        {
            stw1.Start();
            return;
        }


        if (AutoSavePreferences.logSaveEvent)
        {
            LOG.Log("Autosave");
        }

        EditorApplication.SaveScene();

        if (AutoSavePreferences.saveAssets)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
        }
        if (instance != null)
        {
            instance.Repaint();
        }
        stw1.Start();
    }

    [InitializeOnLoadMethod]
    public static void InitAutosave()
    {
        stw1 = new Stopwatch();
        stw1.Start();
        EditorApplication.update += EditorUpdate;
        AutoSavePreferences.LoadPreferences();
        LoadPreferences();
    }

    public static void EditorUpdate()
    {
        if (_saveNow)
        {
            _saveNow = false;
            executeSave();
        }
        if (instance != null)
        {
            instance.Repaint();
        }
    }

    public void OnEnable()
    {
        instance = this;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        bool autosaveEnabled = GUILayout.Toggle(AutoSavePreferences.autosaveEnabled, "Autosave", GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("Last saved: " + Math.Floor(stw1.Elapsed.TotalMinutes) + " minutes ago");
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            AutoSavePreferences.autosaveEnabled = autosaveEnabled;
            AutoSavePreferences.SavePreferences();
        }
    }
}