using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ET
{
    public class CodeLoader : IDisposable
    {
        public static CodeLoader Instance = new CodeLoader();

        public Action Update;
        public Action LateUpdate;
        public Action OnApplicationQuit;

        private Assembly assembly;

        public CodeMode CodeMode { get; set; }

        // 所有mono的类型
        private readonly Dictionary<string, Type> monoTypes = new Dictionary<string, Type>();

        // 热更层的类型
        private readonly Dictionary<string, Type> hotfixTypes = new Dictionary<string, Type>();

        private CodeLoader()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly ass in assemblies)
            {
                var name = ass.GetName();
                if (name.Name == "GameCommon")
                {
                    foreach (Type type in ass.GetTypes())
                    {
                        this.hotfixTypes[type.FullName] = type;
                        this.hotfixTypes[type.AssemblyQualifiedName] = type;
                    }    
                    break;
                }
            }
        }

        public Type GetMonoType(string fullName)
        {
            this.monoTypes.TryGetValue(fullName, out Type type);
            return type;
        }

        public Type GetHotfixType(string fullName)
        {
            this.hotfixTypes.TryGetValue(fullName, out Type type);
            return type;
        }

        public void Dispose()
        {
            //this.appDomain?.Dispose();
        }

        public void Start()
        {
            Debug.Log("Code Loader Start");
            // this.Update += Game.Update;
            // this.LateUpdate += Game.LateUpdate;
            // this.OnApplicationQuit += Game.Close;
            // Game.EventSystem.Add(GetHotfixTypes());
            // Game.EventSystem.Publish(new EventType.AppStart());
        }

        // 热重载调用下面三个方法
        // CodeLoader.Instance.LoadLogic();
        // Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
        // Game.EventSystem.Load();
        public void LoadLogic()
        {
            if (this.CodeMode != CodeMode.Reload)
            {
                throw new Exception("CodeMode != Reload!");
            }

            // 傻屌Unity在这里搞了个傻逼优化，认为同一个路径的dll，返回的程序集就一样。所以这里每次编译都要随机名字
            string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Logic_*.dll");
            if (logicFiles.Length != 1)
            {
                throw new Exception("Logic dll count != 1");
            }

            string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
            byte[] assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.dll"));
            byte[] pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.pdb"));

            Assembly hotfixAssembly = Assembly.Load(assBytes, pdbBytes);

            foreach (Type type in this.assembly.GetTypes())
            {
                this.monoTypes[type.FullName] = type;
                this.hotfixTypes[type.FullName] = type;
            }

            foreach (Type type in hotfixAssembly.GetTypes())
            {
                this.monoTypes[type.FullName] = type;
                this.hotfixTypes[type.FullName] = type;
            }
        }

        public Dictionary<string, Type> GetHotfixTypes()
        {
            return this.hotfixTypes;
        }
    }
}