namespace Mogu
{
    public readonly struct InjectorOption
    {
        public static InjectorOption Default = new InjectorOption(-1);

        public readonly int InjectDllTimeOut;

        public readonly string PreferredClientPipeName;

        public readonly string PreferredServerPipeName;

        public InjectorOption(int injectDllTimeOut, string preferredClientPipeName = null, string preferredServerPipeName = null)  
        {
            InjectDllTimeOut = injectDllTimeOut;
            PreferredClientPipeName = preferredClientPipeName;
            PreferredServerPipeName = preferredServerPipeName;
        }
    }
}
