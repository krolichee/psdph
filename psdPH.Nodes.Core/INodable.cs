namespace psdPH.Nodes
{
    public interface INodable
    {
        Let[] Inlets { get; }
        Let[] Outlets { get; }

        void Execute();
    }
}