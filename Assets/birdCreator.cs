using UnityEngine;

public class birdCreator : MonoBehaviour
{
    public float spawnDelay;
    public GameObject spawnee;
    private GameObject currentBird;
    private Vector3 spawnPos;
    private const int totalPop = 400;
    public int spawned = 0;
    public float[,] population = new float [totalPop, 23];
    private int generationNu = 0;
    private int birdTag;
    private float[,] swapPop = new float[5, 23];

    private static float Sigmoid(float value)
    {
        return 1 / (1 + Mathf.Exp(value * -1));
    }



    // Start is called before the first frame update
    private void Awake()
    {
        for (int kus = 0 ; kus < totalPop; kus++)
        {
            for (int genler = 1; genler < 22; genler++)
            {
                population[kus, genler] = Random.Range(-5.0f,5.0f);
            }
            population[kus, 0] = Random.Range(-1.0f,1.0f);
        }            
    }

    void Start()
    {
        
        while(spawned < totalPop)
        {
            SpawnBird();
            birdTag++;
        }
        InvokeRepeating("Reload",2,1.0f);
        Feed();
        //Debug.Log(generationNu);
    }

    // Update is called once per frame
    public void Reload()
    {
        if (GameObject.FindGameObjectsWithTag("bird").Length == 0)
        {
            generationNu++;
            Debug.Log(generationNu);
            //---------Pipe-Spawner-Stopped----------------
            GameObject.FindObjectOfType<PipeCreator>().CancelInvoke("SpawnPipe");

            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");

            foreach (GameObject currentPipe in allEnemies)
            {
                Destroy(currentPipe);
            }

            GameObject.FindObjectOfType<PipeCreator>().SpawnPipe();
            //---------------------------------------------
            //----------At-this-point-the-game-is-pretty-much-at-a-stop----------
            //--------so-we-can-get-the-biggest-5-scores-before-reload-----------
            int[] top5 = new int[5];
            top5 = findSurvivors();
            //----------------Define-a-new-population-----------------------------
            //------filling-swap-----------------
            for (int i = 4; i > -1; i--)
            {
                if (population[top5[i], 22] > swapPop[i, 22])
                {
                    for (int j = 0; j < 23; j++)
                    {
                        swapPop[i, j] = population[top5[i], j];
                    }
                }
            }
            print("max " + population[top5[0], 22]);

            //------filling-population-from-start
            for (int i = 0; i < 5; i++)
            {
                for (int j = i * (totalPop/5); j < (i + 1) * (totalPop/5); j++)
                {
                    for (int k = 0;k<22;k++)
                    {
                        if (Random.Range(0,8)==1)
                        {
                            population[j, k] = swapPop[i, k] + Sigmoid(Random.Range(-3.0f, 3.0f));
                        }
                    }
                    population[j, 22] = 0;
                }
            }
            for (int j = 340; j < 390; j++)
            {
                for (int k = 0; k < 22; k++)
                {
                    if (Random.Range(0, 10) == 1)
                    {
                        population[j, k] = swapPop[0, k] + Sigmoid(Random.Range(-3.0f, 3.0f));
                    }
                }
            }
            for(int j=0; j<5; j++)
            {
                for (int k = 0; k < 22; k++)
                {
                    population[393, k] = swapPop[j, k];
                }
            }


            //---------------------------------------------------------------------
            //spawn birds
            spawned = 0;
            birdTag = 0;

            while (spawned < totalPop)
            {          
                SpawnBird();
                birdTag++;
            }
            Feed();

            //--------Pipe-Spawner-Triggered----------------
            GameObject.FindObjectOfType<PipeCreator>().TriggerSpawnDelayed();
        }
    }

    public void SpawnBird()
    {
        spawned++;
        spawnPos = new Vector3(-8.87f,0f);
        currentBird = Instantiate(spawnee, spawnPos, transform.rotation);
        currentBird.GetComponent<BirdControl>().bird_tag = birdTag;
    }

    public void Feed()
    {
        foreach(GameObject currentBird_Member in GameObject.FindGameObjectsWithTag("bird"))
        {
            for (int sayac=0 ; sayac<22 ; sayac++)
            {
                currentBird_Member.GetComponent<BirdControl>().kromo[sayac] = population[currentBird_Member.GetComponent<BirdControl>().bird_tag, sayac];
            }
        }
    }

    private int[] findSurvivors()
    {
        int[] top5_Member = { 0, 0, 0, 0, 0 };
        float[] top5_scores = { 0, 0, 0, 0, 0 };
        

        for (int sayac=0 ; sayac< totalPop; sayac++)
        {
            if(population[sayac, 22] > top5_scores[0])
            {
                top5_scores[4] = top5_scores[3];
                top5_Member[4] = top5_Member[3];
                top5_scores[3] = top5_scores[2];
                top5_Member[3] = top5_Member[2];
                top5_scores[2] = top5_scores[1];
                top5_Member[2] = top5_Member[1];
                top5_scores[1] = top5_scores[0];
                top5_Member[1] = top5_Member[0];
                top5_scores[0] = population[sayac,22];
                top5_Member[0] = sayac;
            }
            else if (population[sayac, 22] > top5_scores[1])
            {
                top5_scores[4] = top5_scores[3];
                top5_Member[4] = top5_Member[3];
                top5_scores[3] = top5_scores[2];
                top5_Member[3] = top5_Member[2];
                top5_scores[2] = top5_scores[1];
                top5_Member[2] = top5_Member[1];
                top5_scores[1] = population[sayac, 22];
                top5_Member[1] = sayac;
            }
            else if (population[sayac, 22] > top5_scores[2])
            {
                top5_scores[4] = top5_scores[3];
                top5_Member[4] = top5_Member[3];
                top5_scores[3] = top5_scores[2];
                top5_Member[3] = top5_Member[2];
                top5_scores[2] = population[sayac, 22];
                top5_Member[2] = sayac;
            }
            else if (population[sayac, 22] > top5_scores[3])
            {
                top5_scores[4] = top5_scores[3];
                top5_Member[4] = top5_Member[3];
                top5_scores[3] = population[sayac, 22];
                top5_Member[3] = sayac;
            }
            else if (population[sayac, 22] > top5_scores[4])
            {
                top5_scores[4] = population[sayac, 22];
                top5_Member[4] = sayac;
            }
        }
        return top5_Member;
    }

    private void pushArray_int(int[] arr, int arrSize, int indexStart)
    {
        for(int sayac = arrSize-1 ; sayac>indexStart ; sayac--)
        {
            arr[sayac] = arr[sayac-1];
        }
        arr[indexStart] = 0;
    }
    private void pushArray_float(float[] arr, int arrSize, int indexStart)
    {
        for (int sayac = arrSize - 1; sayac > indexStart; sayac--)
        {
            arr[sayac] = arr[sayac - 1];
        }
        arr[indexStart] = 0;
    }
}
