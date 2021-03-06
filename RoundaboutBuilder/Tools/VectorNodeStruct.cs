﻿using ColossalFramework;
using ColossalFramework.Math;
using System;
using UnityEngine;

/* By Strad, 01/2019 */

/* Version BETA 1.2.0 */

/* In the end I started raging that you can actually reference the nodes (and segments too) in two distinct ways, by their actual reference and
 * by their ushort ID. You allways have the other reference which you don't need. Because of that I created this class. I was too lazy to rewrite
 * the whole code to use this. */

namespace RoundaboutBuilder.Tools
{
    /* In the end not a struct. Whatever. */
    public class VectorNodeStruct : IComparable<VectorNodeStruct>
    {
        public Vector3 vector { get; private set; } // not memory efficient, but doesn't make much difference. Sometimes I need to create the vector first before the node.
        public ushort nodeId { get; private set; } = 0;

        public double angle = 0;

        public NetNode node { get { return GetNode(nodeId); } }

        public bool exists { get { return nodeId != 0; } }

        public ushort Create(NetInfo netInfo)
        {
            if(exists)
            {
                return nodeId;
            }

            ushort newNodeId = NetAccess.CreateNode(netInfo, vector);

            nodeId = newNodeId;
            vector = node.m_position;
            return newNodeId;
        }

        /* Constructors */

        public VectorNodeStruct(Vector3 vector)
        {
            this.vector = vector;
        }

        public VectorNodeStruct(ushort nodeId)
        {
            this.nodeId = nodeId;
            this.vector = GetNode(nodeId).m_position;
        }

        /* Utility */

        public static NetManager Manager
        {
            get { return Singleton<NetManager>.instance; }
        }
        public static NetNode GetNode(ushort id)
        {
            return Manager.m_nodes.m_buffer[id];
        }

        public int CompareTo(VectorNodeStruct other)
        {
            if (angle - other.angle < 0.0001f)
                return 0;
            else if (angle > other.angle)
                return -1;

            return 1;
        }

    }
}
