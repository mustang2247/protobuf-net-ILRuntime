﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
    public class Main
    {
        public static void Initialize()
        {
            ILRuntime_mmopb.Initlize();
            Debug.Log("Initialize");
        }
		public static void Update()
		{
            var c = new mmopb.m_login_c();
            c.account = new mmopb.p_account_c();
            c.account.account = "abc";
            c.account.snapshots.Add(new mmopb.p_avatar_snapshot());
            c.account.snapshots[0].avatar = new mmopb.p_entity_basis();
            c.account.snapshots[0].avatar.account = "def";
            var stream = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize(stream,c);
            Debug.Log(stream.Length);
            var bytes = stream.ToArray();
            var t = ProtoBuf.Serializer.Deserialize(typeof(mmopb.m_login_c), new System.IO.MemoryStream(bytes)) as mmopb.m_login_c;
            Debug.Log("Update" + t.account.snapshots[0].avatar.account);
		}
    }
}
