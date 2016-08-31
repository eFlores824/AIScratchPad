using UnityEngine;
using System.Collections.Generic;

public class AStar {

    private static Node[] allNodes;   

    public static void SetAllNodes()
    {
        if (allNodes == null)
        {
            allNodes = GameObject.FindObjectsOfType<Node>();
        }
    }

    public static Node[] getPath(Node destination, Node start)
    {
        if (start == null)
        {
            return null;
        }
        IList<Node> open = new List<Node>();
        IList<Node> closed = new List<Node>();

        closed.Add(start);
        foreach (Node n in start.connections)
        {
            n.distance = (start.transform.position - n.transform.position).magnitude;
            n.origin = start;
            open.Add(n);
        }
        while (!closed.Contains(destination))
        {
            float smallestDistance = float.MaxValue;
            Node optimal = null;
            foreach (Node n in open)
            {
                float distanceTo = n.distance + (n.transform.position - destination.transform.position).magnitude;
                if (distanceTo < smallestDistance)
                {
                    smallestDistance = distanceTo;
                    optimal = n;
                }
            }
            if (closed.Contains(optimal))
            {
                Debug.Log("The repeated item was " + optimal.gameObject.name);
                return null;
            }
            closed.Add(optimal);
            open.Remove(optimal);
            foreach (Node n in optimal.connections)
            {
                if (!closed.Contains(n))
                {
                    float possibleDistance = optimal.distance + (optimal.transform.position - n.transform.position).magnitude;
                    if (open.Contains(n))
                    {
                        if (possibleDistance < n.distance)
                        {
                            n.distance = possibleDistance;
                            n.origin = optimal;
                        }
                    }
                    else
                    {
                        n.distance = possibleDistance;
                        n.origin = optimal;
                        open.Add(n);
                    }
                }
            }
        }
        Stack<Node> path = new Stack<Node>();
        Node current = destination;
        while (current != null)
        {
            path.Push(current);
            current = current.origin;
        }
        resetOrigins();
        return path.ToArray();
    }

    public static Node[] avoidPositionPath(Node destination, Node start, Vector3 avoiding)
    {
        if (start == null)
        {
            return null;
        }
        IList<Node> open = new List<Node>();
        IList<Node> closed = new List<Node>();

        closed.Add(start);
        avoiding.y = start.theTransform.position.y;
        foreach (Node n in start.connections)
        {
            Vector3 toN = n.theTransform.position - start.theTransform.position;
            Vector3 toAvoiding = avoiding - start.theTransform.position;
            if (toAvoiding.magnitude <= 20.0f) {
                n.distance = Mathf.Infinity;
            }
            else {
                n.distance = toN.magnitude;
            }
            n.origin = start;
            open.Add(n);
        }
        while (!closed.Contains(destination))
        {
            float smallestDistance = Mathf.Infinity;
            Node optimal = null;
            foreach (Node n in open)
            {
                float distanceTo = n.distance + (n.theTransform.position - destination.theTransform.position).magnitude;
                if (distanceTo <= smallestDistance)
                {
                    smallestDistance = distanceTo;
                    optimal = n;
                }
            }
            closed.Add(optimal);
            open.Remove(optimal);
            foreach (Node n in optimal.connections)
            {
                if (!closed.Contains(n))
                {
                    float possibleDistance;
                    Vector3 toN = n.theTransform.position - optimal.theTransform.position;
                    Vector3 toAvoiding = avoiding - optimal.theTransform.position;
                    if (toAvoiding.magnitude <= 20.0f) {
                        possibleDistance = Mathf.Infinity;
                    }
                    else
                    {
                        float parentDistance = optimal.distance == Mathf.Infinity ? 0 : optimal.distance;
                        possibleDistance = parentDistance + toN.magnitude;
                    }
                    if (open.Contains(n))
                    {
                        if (possibleDistance < n.distance)
                        {
                            n.distance = possibleDistance;
                            n.origin = optimal;
                        }
                    }
                    else
                    {
                        n.distance = possibleDistance;
                        n.origin = optimal;
                        open.Add(n);
                    }
                }
            }
        }
        Stack<Node> path = new Stack<Node>();
        Node current = destination;
        path.Push(destination);
        while (current != null)
        {
            path.Push(current);
            current = current.origin;
        }
        resetOrigins();
        return path.ToArray();
    }

    private static void resetOrigins()
    {
        foreach (Node n in allNodes)
        {
            n.distance = 0.0f;
            n.origin = null;
        }
    }

    public static Node findNearestNode(Vector3 position)
    {
        Node[] near = nearbyNodes(position);
        Node closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (Node n in near)
        {
            Vector3 toNode = position - n.transform.position;
            float distance = toNode.magnitude;
            if (distance < closestDistance)
            {
                closestNode = n;
                closestDistance = distance;
            }
        }
        return closestNode;
    }

    public static Node findFarthestNode(Vector3 position)
    {
        Node[] near = nearbyNodes(position);
        Node furthestNode = null;
        float furthestDistance = float.MinValue;
        foreach (Node n in near)
        {
            Vector3 toNode = position - n.transform.position;
            float distance = toNode.magnitude;
            if (distance > furthestDistance)
            {
                furthestNode = n;
                furthestDistance = distance;
            }
        }
        return furthestNode;
    }

    public static Node findNearestTargetNode(Vector3 position, Vector3 target)
    {
        Node[] near = nearbyNodes(position);
        Node closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (Node n in near)
        {
            Vector3 toNode = target - n.transform.position;
            float distance = toNode.magnitude;
            if (distance < closestDistance)
            {
                closestNode = n;
                closestDistance = distance;
            }
        }
        return closestNode;
    }

    public static Node findFurthestTargetNearNode(Vector3 position, Vector3 avoidingPosition)
    {
        Node[] near = nearbyNodes(position);
        Node furthestNode = null;
        float furthestDistance = float.MinValue;
        foreach (Node n in near)
        {
            Vector3 toNode = avoidingPosition - n.transform.position;
            float distance = toNode.magnitude;
            if (distance > furthestDistance)
            {
                furthestNode = n;
                furthestDistance = distance;
            }
        }
        return furthestNode;
    }

    public static Node[] nearbyNodes(Vector3 position)
    {
        List<Node> near = new List<Node>();
        foreach (Node n in allNodes)
        {
            Vector3 toEnemy = n.transform.position - position;
            bool hitSomething = Physics.Raycast(position, toEnemy, toEnemy.magnitude);
            if (!hitSomething)
            {
                near.Add(n);
            }
        }
        return near.ToArray();
    }

    public static bool pathClear(Vector3 start, Vector3 end)
    {
        Vector3 toEnd = end - start;
        bool hitSomething = Physics.Raycast(start, toEnd, toEnd.magnitude);
        return !hitSomething;
    }
}