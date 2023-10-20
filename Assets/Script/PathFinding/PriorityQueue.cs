using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PriorityQueue<T> where T : Node
{
    List<T> nodeList = new List<T>();

    public int count => nodeList.Count; // Lambda operator used

    /// <summary>
    /// Add item to list
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(T item)
    {
        nodeList.Add(item);
        BubbleUp();
    }

    /// <summary>
    /// Remove top item on list
    /// Reorgainze list
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        var item = nodeList[0];
        MoveLastItemToTheTop();
        ChildPosition();

        return item;
    }

    /// <summary>
    /// Move the smallest value to top
    /// </summary>
    private void BubbleUp()
    {
        var childIndex = nodeList.Count - 1;
        while (childIndex > 0)
        {
            var parentIndex = (childIndex - 1) / 2;

            // If child is greater than parent, break
            if (nodeList[childIndex].F >= (nodeList[parentIndex]).F)
            {
                break;
            }
            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveLastItemToTheTop()
    {
        var lastIndex = nodeList.Count - 1;
        nodeList[0] = nodeList[lastIndex];
        nodeList.RemoveAt(lastIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ChildPosition()
    {
        var lastIndex = nodeList.Count - 1;
        var parentIndex = 0;

        while (true)
        {
            var firstChildIndex = (parentIndex * 2) + 1;
            if (firstChildIndex > lastIndex)
            {
                break;
            }

            var secondChildIndex = firstChildIndex + 1;

            if (secondChildIndex <= lastIndex && nodeList[secondChildIndex].F < (nodeList[firstChildIndex]).F)
            {
                firstChildIndex = secondChildIndex;
            }

            if (nodeList[parentIndex].F < (nodeList[firstChildIndex]).F)
            {
                break;
            }

            Swap(parentIndex, firstChildIndex);
            parentIndex = firstChildIndex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Swap(int index1, int index2)
    {
        var tmp = nodeList[index1];
        nodeList[index1] = nodeList[index2];
        nodeList[index2] = tmp;
    }

    /// <summary>
    /// Clear Node list
    /// </summary>
    public void ClearQueue()
    {
        nodeList.Clear();
    }

    /// <summary>
    /// Return node at index 0
    /// </summary>
    /// <returns></returns>
    public Node Peek()
    {
        return nodeList[0];
    }

    /// <summary>
    /// Checks if list contians item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        return nodeList.Contains(item);
    }
}