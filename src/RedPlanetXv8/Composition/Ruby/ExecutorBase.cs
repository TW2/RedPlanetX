using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.Ruby
{
    public abstract class ExecutorBase
    {
        private bool isInitialized = false;

        protected ScriptRuntime scriptRuntime;
        protected ScriptEngine scriptEngine;
        protected List<DynamicLanguageScript> scriptList;

        protected abstract void CreateRuntime();
        protected abstract void CreateEngine();

        public void Init()
        {
            ExecuteAll();
        }

        public ExecutorBase()
        {
            InitiateSequence();
        }

        private void InitiateSequence()
        {
            scriptList = new List<DynamicLanguageScript>();
            CreateRuntime();
            CreateEngine();
        }

        public void AddScript(string scriptContents)
        {
            scriptList.Add(new DynamicLanguageScript { CodeText = scriptContents, Executed = false });

            if (isInitialized)
                ExecuteRecent();
        }

        public void AddFile(string scriptFile)
        {
            scriptList.Add(new DynamicLanguageScript { CodeText = ReadFile(scriptFile), Executed = false });

            if (isInitialized)
                ExecuteRecent();
        }

        protected void ExecuteAll()
        {
            scriptList.ForEach(s => { scriptEngine.Execute(s.CodeText); s.Executed = true; });
            isInitialized = true;
        }

        protected void ExecuteRecent()
        {
            scriptList.Where(s => !s.Executed).ToList().ForEach(s => { scriptEngine.Execute(s.CodeText); s.Executed = true; });
        }

        public object GetInstance(string instanceName)
        {
            if (!isInitialized)
                throw new NotSupportedException("Executor not initialized. Call \"Init()\" to initialize");

            dynamic instanceVariable;
            var instanceVariableResult = scriptEngine.Runtime.Globals.TryGetVariable(instanceName, out instanceVariable);

            if (!instanceVariableResult && instanceVariable == null)
                throw new InvalidOperationException(string.Format("Unable to find {0}", instanceName));

            dynamic instance = scriptEngine.Operations.CreateInstance(instanceVariable);
            return instance;
        }

        public IEnumerable<string> GetInstanceNames()
        {
            return scriptEngine.Runtime.Globals.GetVariableNames();
        }

        public bool CanInstantiate(string className)
        {
            return scriptEngine.Runtime.Globals.ContainsVariable(className);
        }

        private string ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            throw new FileNotFoundException(string.Format("{0} was not found", fileName));
        }
    }
}
