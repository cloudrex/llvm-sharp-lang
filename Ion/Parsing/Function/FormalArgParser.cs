using System;
using Ion.Generation;
using Ion.CognitiveServices;
using Ion.Syntax;
using Type = Ion.Generation.Type;

namespace Ion.Parsing
{
    public class FormalArgParser : IParser<FormalArg>
    {
        public FormalArg Parse(ParserContext context)
        {
            // Parse the type.
            Type type = new TypeParser().Parse(context);

            // Create the formal argument entity.
            FormalArg arg = new FormalArg(type);

            // Capture the argument's name.
            string name = context.Stream.Get(TokenType.Identifier).Value;

            // Assign the argument's name.
            arg.SetName(name);

            // Skip the identifier token.
            context.Stream.Skip();

            // Return the argument.
            return arg;
        }
    }
}
