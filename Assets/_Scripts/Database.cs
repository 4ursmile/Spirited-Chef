using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;
using Data;
using Cysharp.Threading.Tasks;
public class Database : SingleTon<Database>
{
    [SerializeField] private DatabseResourcePathSO _resourcePath;
    [SerializeField] private EmbeddDatabaseSO _embeddDatabase;
    public DatabseResourcePathSO ResourcePath => _resourcePath;
    /// <summary>
    /// Get Table in database
    /// Remember to use this method to get table
    /// Table Contains key and value of resource path
    /// All keys are named follow snake case (ex: "table_name")
    /// </summary>
    /// <param name="key">name of table</param>
    /// <returns>Table</returns>
    public static TableResourcePathSO Table(string key) => Instance.ResourcePath.GetTable(key);
    public void GetTable(string key) => Instance.ResourcePath.GetTable(key);

    public void AddTable(string key, TableResourcePathSO value) => Instance.ResourcePath.AddTable(key, value);
    public void RemoveTable(string key) => Instance.ResourcePath.RemoveTable(key);
    public void UpdateTable(string key, TableResourcePathSO value) => Instance.ResourcePath.UpdateTable(key, value);

    public static EmbeddDatabaseSO Embedded => Instance._embeddDatabase;  
    public static float Score(string key, string key2) => Instance._embeddDatabase.Score(key, key2);
    public static List<float> QueryEmbedd(string key) => Instance._embeddDatabase.QueryEmbedd(key);
    protected override void Init()
    {
        _embeddDatabase.InitAsync();
    }

}
