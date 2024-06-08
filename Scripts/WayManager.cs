using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class WayManager : MonoBehaviour {
    public LayerMask nodeLayer;  // Маска слоя Node на дороге (можно оставить пустым).
    
    public float errorMarginInSeconds = 3f;  // Сколько секунд ждать перед выдачей предупреждения о встречке.
    public bool isRightHandedTraffic = true;  // Правостороннее ли движение?

    float margin = 0;
    Collider[] overlaps;
    Coroutine checking;
    
    void Awake() {
        // Если не было задано значение, то задать автоматически.
        if (nodeLayer == 0) 
            nodeLayer = LayerMask.GetMask("Node");
        
        overlaps = new Collider[5];
    }

    void FixedUpdate() {
        // Если игрок едет на машине, то запустить проверку на выезд на встречку, если нет - то остановить.
        switch (GameControl.driving) {
            case true when checking is null:
                checking = StartCoroutine(CheckLoop());
                break;
            case false when checking is not null:
                StopCoroutine(checking);
                break;
        }
    }

    IEnumerator CheckLoop() {
        while (true) {
            bool violation = CheckMergingToOncomingLane();

            // Отсчет длительности нахождения на встречке, если долго - отправить предупреждение.
            if (violation) {
                margin += Time.deltaTime;
                
                if (margin >= errorMarginInSeconds) {
                    NotificationSystem.Manager.ShowNotification("You can't drive on the opposite side of the road!");
                    margin = 0;
                }

                yield return null;
            }
            else {
                margin = 0;
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    bool CheckMergingToOncomingLane() {
        // Нахождение всех ближайших к игроку дорожных Node.
        int amount = Physics.OverlapSphereNonAlloc(transform.position, 15, overlaps, nodeLayer);
        if (amount == 0) return false;

        float minDst = float.MaxValue;
        Collider nearest = overlaps[0];

        // Нахождение самого ближайшего к машине нода.
        for (int i = 0; i < amount; i++) {
            Collider overlap = overlaps[i];
            float curDst = Vector3.Distance(transform.position, overlap.transform.position);
            if (curDst < minDst) {
                minDst = curDst;
                nearest = overlap;
            }
        }

        Node nearestNode = nearest.GetComponent<Node>();
        Transform nodeTransform = nearestNode.transform;

        Vector3 nearestPos = nodeTransform.position;
        Vector3 nextPosition = nearestNode.nextNode.position;
        
        // Проверка с какой стороны от разметки находится машина
        float roadSide = Vector3.Dot(nodeTransform.right, transform.position - nearestPos);
        
        // Проверка в какую сторону от середины города движется машина
        float roadDirection = Vector3.Dot(transform.forward,
            (nearestPos - nextPosition).normalized) * (Convert.ToInt32(isRightHandedTraffic) * 2 - 1);

        return roadDirection * roadSide <= 0;  // Если одно из условий отрицательное, то нарушение.
    }
}