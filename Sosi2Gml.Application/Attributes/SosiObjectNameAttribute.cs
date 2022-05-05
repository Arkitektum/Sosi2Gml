namespace Sosi2Gml.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SosiObjectNameAttribute : Attribute
    {
        private readonly string _sosiObjectName;

        public SosiObjectNameAttribute(string sosiObjectName)
        {
            _sosiObjectName = sosiObjectName;
        }

        public virtual string SosiObjectName => _sosiObjectName;
    }
}
