﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    // TODO: Add  



    [SerializeField]
    private List<GameObject> m_allParts = new List<GameObject>();
    [SerializeField]
    private GameObject m_playerPrefab;
    
    [SerializeField]
    private List<float> m_heights;
    [SerializeField]
    private int m_height = 0;
    [SerializeField]
    private bool m_parseLevel = true;

    // TODO: Draw a gizmo box which changes size in the editor. 
    [SerializeField]
    private float m_lowestYPosition = 0;
    [SerializeField]
    private float m_lowestXPosition = 0;

    // TODO: Draw a gizmo box which changes size in the editor. 
    [SerializeField]
    private float m_YSize = 69f; // NOTE: This is hardcoded for this sprite tilemap.
    [SerializeField]
    private float m_XSize = 69f; // NOTE: This is hardcoded for this sprite tilemap.

    private ParseManager m_parseManager;
    private GenerationManager m_generationManager;
    private GameObject m_player;
    private uint m_levelSize = 0;
    private int m_backTracking = 1;


    private void Awake()
    {
        // TODO: Error check these.
        m_parseManager = GetComponent<ParseManager>();
        m_generationManager = GetComponent<GenerationManager>();

        if(m_allParts.Count <= 0)
        {
            Debug.LogWarning("ERROR: Missing level objects in allParts list. Must contain all parts of a level.");
        }

        // This will load the transition matrix from the file. 
        m_parseManager.ParseHeightLevel(0, false);


        // TODO: Check if height is < 0
        // Calculate the number of tiles up. ie height of level. 
        m_heights = new List<float>();

        for(int i = 0; i < m_height; i++)
        {
            m_heights.Add(m_lowestYPosition);
            m_lowestYPosition += m_YSize;
        }

        ResetPlayer();
    }


    private void OnEnable()
    {
        UIManager.generate += GenerateLevel;
        UIManager.parse += ParseLevel;
        UIManager.resetLevel += ResetLevel;
        UIManager.backtrackingChanged += OnBacktrackChange;
    }


    private void OnDisable()
    {
        UIManager.generate -= GenerateLevel;
        UIManager.parse -= ParseLevel;
        UIManager.resetLevel -= ResetLevel;
        UIManager.backtrackingChanged -= OnBacktrackChange;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //if(Parse())
            //{
            //    Generate();
            //}
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
        }
    }


    private void ParseLevel()
    {
        if(!m_parseManager.InitSceneObjects(out m_levelSize))
        {
            Debug.LogWarning("ERROR: Initialising ParseManager scene objects failed.");
        }

        // Gather all tiles of the scene, only once. 
        m_parseManager.ParseLevel(m_heights);

        m_parseManager.ParseHeightLevel(m_backTracking, m_parseLevel);
        Debug.LogWarning("Level Parsed. Saving the Transition Matrix is set to " + m_parseLevel);
    }


    private void GenerateLevel()
    {
        var textHeightLevel = m_generationManager.GenerateNumberLevel(m_parseManager.TransitionMatrix, m_backTracking);
        if(textHeightLevel != "")
        {
            m_generationManager.MapTiles(m_heights, textHeightLevel, m_lowestXPosition, m_XSize);
            // TODO: Add player manager.

            ResetPlayer();

            Debug.LogWarning("Level Generated");
        }
        else
        {
            Debug.LogError("Error: Generating level.");
        }
    }



    private void ResetLevel()
    {
       ResetPlayer();
    }


    private void ResetPlayer()
    {
        if(m_player) Destroy(m_player);
        m_player = Instantiate(m_playerPrefab);
    }


    private void OnBacktrackChange(int value)
    {
        m_backTracking = value;
    }


    //private void Generate()
    //{
    //    if(!m_generationManager.Generate(m_runtimeMatrix, m_allParts, m_levelSize))
    //        Debug.LogWarning("ERROR: Generating level.");
    //}
}
