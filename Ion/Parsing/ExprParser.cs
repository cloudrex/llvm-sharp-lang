using System;
using Ion.CodeGeneration;
using Ion.SyntaxAnalysis;

namespace Ion.Parsing
{
    public class ExprParser : IParser<Expr>
    {
        public Expr Parse(TokenStream stream)
        {
            // Parse the left side of the expression.
            Expr leftSide = new PrimaryExprParser().Parse(stream);

            // Ensure left side was successfully parsed, otherwise throw an error.
            if (leftSide == null)
            {
                throw new Exception("Unexpected expression left-side to be null");
            }

            // Invoke the binary expression parser, for potential following expression(s).
            Expr expr = new BinaryOpRightSideParser(leftSide, 0).Parse(stream);

            // Return the parsed expression.
            return expr;
        }
    }
}
