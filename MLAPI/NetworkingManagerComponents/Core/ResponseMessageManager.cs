using System.Collections.Generic;
using UnityEngine;

namespace MLAPI.Internal
{
    internal static class ResponseMessageManager
    {
        private static readonly Dictionary<ulong, RpcResponseBase> pendingResponses = new Dictionary<ulong, RpcResponseBase>();
        private static readonly SortedList<ulong, float> responseAdded = new SortedList<ulong, float>();
        
        private static ulong messageIdCounter;
        
        internal static ulong GenerateMessageId()
        {
            return messageIdCounter++;
        }
        
        internal static void CheckTimeouts()
        {
            while (responseAdded.Count > 0 && Time.time - responseAdded[0] > pendingResponses[responseAdded.Keys[0]].Timeout)
            {
                ulong key = responseAdded.Keys[0];

                RpcResponseBase response = pendingResponses[key];
                response.IsDone = true;
                response.IsSuccessful = false;
                
                responseAdded.Remove(key);
                pendingResponses.Remove(key);
            }
        }

        internal static void Clear()
        {
            pendingResponses.Clear();
            responseAdded.Clear();
        }

        internal static void Add(ulong key, RpcResponseBase value)
        {
            pendingResponses.Add(key, value);
            responseAdded.Add(key, Time.time);
        }

        internal static void Remove(ulong key)
        {
            pendingResponses.Remove(key);
            responseAdded.Remove(key);
        }

        internal static bool ContainsKey(ulong key)
        {
            return pendingResponses.ContainsKey(key);
        }

        internal static RpcResponseBase GetByKey(ulong key)
        {
            return pendingResponses[key];
        }
    }
}