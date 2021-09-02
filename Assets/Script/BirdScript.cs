using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    // 鳥のプレハブを格納する配列
    public GameObject[] BirdPrefabs;

    [SerializeField]
    private float removeBirdMinCount = 3;

    [SerializeField]
    private float birdDistance = 1.4f;

    [SerializeField]
    ScoreManager scoreManager;

    private GameObject firstBird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();

    public static int score = 0;

    private float counttime = 0.0f;
    private float timeLimit = 30.0f;

    public static int getscore()
    {
        return score;
    }

    // Start is called before the first frame update
    void Start()
    {
        TouchManager.Began += (info) =>
        {
            // クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                // ヒットしたオブジェクトのtagを判別し初期化
                if (hitObj.tag == "bird")
                {
                    firstBird = hitObj;
                    lastBird = hitObj;
                    currentName = hitObj.name;
                    removableBirdList = new List<GameObject>();
                    PushToBirdList(hitObj);
                }
            }
        };
        TouchManager.Moved += (info) =>
        {
            if (!firstBird)
            {
                return;
            }
            // クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                // ヒットしたオブジェクトのtagが鳥、尚且名前が一緒、
                // 尚且最後にhitしたオブジェクトと違う、尚且リストに格納されていない
                if (hitObj.tag == "bird" && hitObj.name == currentName
                && hitObj != lastBird && 0 > removableBirdList.IndexOf(hitObj))
                {
                    float distance = Vector2.Distance(hitObj.transform.position,
                        lastBird.transform.position);
                    if (distance > birdDistance)
                    {
                        return;
                    }

                    lastBird = hitObj;
                    PushToBirdList(hitObj);
                }
            }
        };
        TouchManager.Ended += (info) =>
        {
            int removeCount = removableBirdList.Count;
            if (removeCount >= removeBirdMinCount)
            {
                foreach (GameObject obj in removableBirdList)
                {
                    Destroy(obj);
                }
                int count = removeCount;
                if (count >= 3)
                {
                    scoreManager.AddScore((int)Mathf.Pow(2, count));
                }

                StartCoroutine(DropBirds(removeCount));

            }

            foreach (GameObject obj in removableBirdList)
            {
                ChangeColor(obj, 1.0f);
            }
            removableBirdList = new List<GameObject>();
            firstBird = null;
            lastBird = null;

        };

        StartCoroutine(DropBirds(50));

    }
    private void PushToBirdList(GameObject obj)
    {
        removableBirdList.Add(obj);
        ChangeColor(obj, 0.5f);
    }
    private void ChangeColor(GameObject obj, float transparency)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r,
            renderer.color.g,
            renderer.color.b,
            transparency);
    }

    IEnumerator DropBirds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // ランダムで出現位置を作成
            Vector2 pos = new Vector2(Random.Range(-4.69f, 4.69f), 8.61f);
            // ランダムで鳥を出現させてIDを格納
            int id = Random.Range(0, BirdPrefabs.Length);
            // 鳥を発生させる
            GameObject bird = (GameObject)Instantiate(BirdPrefabs[id],
                pos,
                Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));
            // 作成した鳥の名前を変更
            bird.name = "bird" + id;
            // 0.05秒待って次の処理へ
            yield return new WaitForSeconds(0.05f);
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        counttime += Time.deltaTime;
        if (counttime > timeLimit)
        {
            SceneManager.LoadScene("END");
        }
    }
}
