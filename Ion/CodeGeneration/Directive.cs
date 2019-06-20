using Ion.Parsing;

namespace Ion.CodeGeneration
{
    public class Directive : Construct
    {
        public string Key { get; }

        public string Value { get; }

        public override ConstructType ConstructType => ConstructType.Directive;

        public Directive(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public Directive(PathResult key, string value) : this(key.ToString(), value)
        {
            //
        }

        public override Construct Accept(CodeGenVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
