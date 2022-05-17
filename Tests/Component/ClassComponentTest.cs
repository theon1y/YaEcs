using YaEcs;

namespace Tests.Component
{
    public class ClassComponentTest : ComponentsTestBase<ClassComponentTest.ClassComponent>
    {
        public class ClassComponent : IComponent, IIntValue
        {
            public int Value { get; set; }
        }
    }
}