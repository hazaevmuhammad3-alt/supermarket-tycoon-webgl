using UnityEngine;
using System.Collections.Generic;

public class CheckoutController : MonoBehaviour
{
    [Header("Checkout Settings")]
    public float checkoutTime = 2f;
    public int maxQueueSize = 5;

    [Header("Queue")]
    public Vector3 queueStartOffset = new Vector3(0, 0, -2);
    public float queueSpacing = 1.5f;

    private Queue<CustomerAI> customerQueue = new Queue<CustomerAI>();
    private CustomerAI currentCustomer;
    private float checkoutTimer = 0f;
    private bool isProcessing = false;

    void Update()
    {
        if (isProcessing && currentCustomer != null)
        {
            checkoutTimer += Time.deltaTime;

            if (checkoutTimer >= checkoutTime)
            {
                CompleteCheckout();
            }
        }
        else if (customerQueue.Count > 0 && !isProcessing)
        {
            StartNextCheckout();
        }
    }

    public void AddToQueue(CustomerAI customer)
    {
        if (customerQueue.Count < maxQueueSize)
        {
            customerQueue.Enqueue(customer);
            Debug.Log($"Customer added to checkout queue. Queue size: {customerQueue.Count}");
        }
        else
        {
            Debug.Log("Checkout queue is full!");
        }
    }

    void StartNextCheckout()
    {
        if (customerQueue.Count > 0)
        {
            currentCustomer = customerQueue.Dequeue();
            currentCustomer.StartCheckout();
            isProcessing = true;
            checkoutTimer = 0f;

            Vector3 checkoutPosition = transform.position + Vector3.forward * 1.5f;
            currentCustomer.transform.position = checkoutPosition;
        }
    }

    void CompleteCheckout()
    {
        isProcessing = false;
        currentCustomer = null;
        checkoutTimer = 0f;
    }

    public Vector3 GetQueuePosition(CustomerAI customer)
    {
        int queueIndex = 0;
        foreach (CustomerAI c in customerQueue)
        {
            if (c == customer)
                break;
            queueIndex++;
        }

        Vector3 queuePosition = transform.position + queueStartOffset;
        queuePosition += Vector3.back * (queueIndex * queueSpacing);
        return queuePosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 queueStart = transform.position + queueStartOffset;
        
        for (int i = 0; i < maxQueueSize; i++)
        {
            Vector3 position = queueStart + Vector3.back * (i * queueSpacing);
            Gizmos.DrawWireSphere(position, 0.3f);
        }
    }
}
