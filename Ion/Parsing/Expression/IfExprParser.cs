using Ion.Generation;
using Ion.Syntax;

namespace Ion.Parsing
{
    public class IfParser : IParser<If>
    {
        public If Parse(ParserContext context)
        {
            // Ensure current token type is keyword if.
            context.Stream.EnsureCurrent(TokenType.KeywordIf);

            // Skip if keyword token.
            context.Stream.Skip();

            // Invoke parentheses expression parser to parse condition.
            Construct condition = new ParenthesesExprParser().Parse(context);

            // Invoke block parser to parse if block, which is its action.
            Block action = new BlockParser().Parse(context);

            // Create the alternative action buffer.
            Block otherwise = null;

            // Capture the current token.
            Token token = context.Stream.Current;

            // If expression contains an alternative action.
            if (token.Type == TokenType.KeywordElse)
            {
                // Skip else keyword token.
                context.Stream.Skip();

                // Invoke a new block parser to parse the alternative action.
                otherwise = new BlockParser().Parse(context);
            }

            // Create the if expression entity.
            If ifExpression = new If(condition, action, otherwise);

            // Return the resulting if expression entity.
            return ifExpression;
        }
    }
}
