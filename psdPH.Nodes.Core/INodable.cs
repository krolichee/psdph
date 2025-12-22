namespace psdPH.Nodes
{
    public interface INodable
    {
        Let[] Chain { get; }
        Let[] Inlets { get; }
        Let[] Outlets { get; }

        void Execute();
    }
}