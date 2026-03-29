using System;

namespace LucasBaran.Bootstrap
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ScenarioModuleAttribute : Attribute
    {
    }
}
