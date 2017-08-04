支持ILRuntime 的 protobuf-net
把 src 里面的 protobuf-net 编译或直接放在 unity Assets 中 删掉.csproj

Unity中使用 需要注册一下 

                static bool InitedILRuntime = false;
		static IMethod s_HFInitialize;
		static IMethod s_HFUpdate;
		static ILRuntime.Runtime.Enviorment.AppDomain HFDomain;

	        static void InitializeILRuntimeCLR(){
			ProtoBuf.PType.RegisterFunctionCreateInstance(PType_CreateInstance);
			ProtoBuf.PType.RegisterFunctionGetRealType(PType_GetRealType);
	        }
		static void Initialize(){
			var hfMain = HFDomain.GetType ("HotFix.Main");
			s_HFInitialize = hfMain.GetMethod ("Initialize", 0);
			s_HFUpdate = hfMain.GetMethod ("Update", 0);
			HFDomain.Invoke (s_HFInitialize, null, null);
		}
		public static void Update(){
			if (InitedILRuntime) {
				HFDomain.Invoke (s_HFUpdate, null, null);
			}
		}

		static object PType_CreateInstance(string typeName){
			return HFDomain.Instantiate (typeName);
		}
		static Type PType_GetRealType(object o){
			var type = o.GetType ();
			if (type.FullName == "ILRuntime.Runtime.Intepreter.ILTypeInstance") {
				var ilo = o as ILRuntime.Runtime.Intepreter.ILTypeInstance;
				type = ProtoBuf.PType.FindType (ilo.Type.FullName);
			}
			return type;
		}
		    

Dll 中使用 参考 hotfix目录下main.cs

        public static void Initialize()
        {
            ILRuntime_mmopb.Initlize();
            Debug.Log("Initialize");
        }
        
	public static void Update()
	{
            if (!s_Initialized) return;
            var c = new mmopb.m_login_c();
            c.account = new mmopb.p_account_c();
            c.account.account = "abc";
            c.account.snapshots.Add(new mmopb.p_avatar_snapshot());
            c.account.snapshots.Add(new mmopb.p_avatar_snapshot());
			var s = new mmopb.p_avatar_snapshot();
			s.avatar = new mmopb.p_entity_basis();
			s.avatar.account = "defxxx";
            c.account.snapshots.Add(s);
			c.account.snapshots.Add(s);
            var stream = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize(stream,c);
            Debug.Log(stream.Length);
            var bytes = stream.ToArray();
            var t = ProtoBuf.Serializer.Deserialize(typeof(mmopb.m_login_c), new System.IO.MemoryStream(bytes)) as mmopb.m_login_c;
            Debug.Log(t.account.snapshots.Count);
            Debug.Log("Update"+t.account.snapshots[3].avatar.account);
	}



