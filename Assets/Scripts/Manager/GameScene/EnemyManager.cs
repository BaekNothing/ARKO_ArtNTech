using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingleTone<EnemyManager>
{
    GameObject enemyPrefab;
    GameObject sceneObjects;
    GameObject getSceneObejcts()
    {
        if(!sceneObjects) sceneObjects = GameObject.Find("SceneObjects");
        return sceneObjects;
    }

    protected override void Init()
    {
        base.Init();
        enemyPrefab =
            DataManager.instance.PrefabLoad<GameObject>
            (DataManager.ResourceCategories.GameObject, "Enemy");
        SpwnAllEnemies(20);
    }

    public void SpwnAllEnemies(int count)
    {
        for (int i = 0; i < count; i++)
            SpawnEnemy(new Vector3(UnityEngine.Random.Range(-30, 30), 0f, 0f));
    }

    public void SpawnEnemy(Vector3 pos)
    {
        if(!sceneObjects)
            sceneObjects = GameObject.Find("SceneObejcts");
        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemy.transform.parent = getSceneObejcts().transform;
    }
}
