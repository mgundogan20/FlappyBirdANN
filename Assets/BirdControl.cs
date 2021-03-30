using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    public float score = 0;
    public float[] kromo= new float[22];
    private float jumpPower = 5;
    private bool isDead = false;
    public int bird_tag;

    private float a, b, c, d; //inputlar
    private float h1, h2, h3, h4; //hidden layer değerleri
    private float s, bias; //son toplam ve eşik
    private float a1, a2, a4, at; //a'nın ağırlıkları
    private float b1, b3, b4, bt; //b'nın ağırlıkları
    private float c3, c2, c4, ct; //c'nın ağırlıkları
    private float d1, d2, d3, d4, dt; //dnin ağırlıkları
    private float t1, t2, t3, t4; //gizli katman ağırlıkları

    private static float Sigmoid(float value)
    {
        return 1.0f / (1.0f + Mathf.Exp(value * -1));
    }
    

    void Start()
    {
        bias = kromo[0];
        a1 = kromo[1];
        a2 = kromo[2];
        a4 = kromo[3];
        at = kromo[4];

        b1 = kromo[5];
        b3 = kromo[6];
        b4 = kromo[7];
        bt = kromo[8];

        c2 = kromo[9];
        c3 = kromo[10];
        c4 = kromo[11];
        ct = kromo[12];

        d1 = kromo[13];
        d2 = kromo[14];
        d3 = kromo[15];
        d4 = kromo[16];
        dt = kromo[17];

        t1 = kromo[18];
        t2 = kromo[19];
        t3 = kromo[20];
        t4 = kromo[21];
        
    }
    

    void Update()
    {
        score += 0.1f;

        GameObject.FindObjectOfType<birdCreator>().population[bird_tag, 22] = score;
        float enYakinBoru = 100;
        enemy closestEnemy = null;
        enemy[] allEnemies = GameObject.FindObjectsOfType<enemy>();
        foreach (enemy currentEnemy in allEnemies)
        {
            if ((currentEnemy.transform.position.x - gameObject.transform.position.x < enYakinBoru) && (currentEnemy.transform.position.x > -10))
            {
                enYakinBoru = currentEnemy.transform.position.x - gameObject.transform.position.x;
                closestEnemy = currentEnemy;
            }
        }

        //------------ANN-Decision-System---------------------

        //inputları besle (3 değişken gelecek)
        a = transform.position.y;
        b = Mathf.Abs(enYakinBoru);
        c = closestEnemy.transform.position.y;
        d = gameObject.GetComponent<Rigidbody2D>().velocity.y;
        //ara nöron değerlerini oluştur (4 tane değişken olacak)
        h1 = Sigmoid(a1 * a + b1 * b + d1 * d);
        h2 = Sigmoid(a2 * a + c2 * c + d2 * d);
        h3 = Sigmoid(b3 * b + c3 * c + d3 * d);
        h4 = Sigmoid(a4 * a + b4 * b + c4 * c + d4 * d);
        //son toplam değeri
        s = (at * a) + (bt * b) + (ct * c) + (dt * d) + (t1 * h1) + (t2 * h2) + (t3 * h3) + (t4 * h4);
        s = Sigmoid(s);


        if (s > bias)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, jumpPower);
        }
        //------------ANN-Decision-System---------------------

        if (isDead)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -4.84 || transform.position.y > 4.74)
        {
            isDead = true;
        }
    }

}
