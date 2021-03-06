using UnityEngine;

public class Invaders : MonoBehaviour
{

    public Invader[] Prefabs = new Invader[6];
    public float Speed = 1.0f;
    public Vector3 Direction = Vector3.right;
    public Vector3 InitialPosition;
    public System.Action<Invader> Killed;

    public int AmountKilled;
    public int AmountAlive => TotalAmount - AmountKilled;
    public int TotalAmount => Rows * Columns;
    public float PercentKilled => (float)AmountKilled / (float)TotalAmount;


    public int Rows = 5;
    public int Columns = 11;

    public Gun RocketPrefab;
    public float RocketSpawnRate = 1f;

    private void Awake()
    {
        InitialPosition = transform.position;

        for (int i = 0; i < Rows; i++)
        {
            float width = 2f * (Columns - 1);
            float height = 2f * (Rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < Columns; j++)
            {
                Invader invader = Instantiate(Prefabs[i], transform);
                invader.killed += OnInvaderKilled;

                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(RocketAttack), RocketSpawnRate, RocketSpawnRate);
    }

    private void RocketAttack()
    {
        int amountAlive = AmountAlive;

        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1f / (float)amountAlive))
            {
                Instantiate(RocketPrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        transform.position += Direction * (Speed + AmountKilled / 10) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (Direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        Direction = new Vector3(-Direction.x, 0f, 0f);

        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    private void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        AmountKilled++;
        Killed(invader);
    }

    public void ResetInvaders()
    {
        AmountKilled = 0;
        Direction = Vector3.right;
        transform.position = InitialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

}
