using System;
using Ion.Generation;
using Ion.CognitiveServices;
using Ion.Syntax;

namespace Ion.Parsing
{
    /// <summary>
    /// Parser right-side of a binary operation.
    /// See https://llvm.org/docs/tutorial/LangImpl02.html @ ParseBinOpRHS().
    /// </summary>
    public class BinaryOpRightSideParser : IParser<Construct>
    {
        protected readonly int minimalPrecedence;

        protected Construct leftSide;

        public BinaryOpRightSideParser(Construct leftSide, int minimalPrecedence)
        {
            this.leftSide = leftSide;
            this.minimalPrecedence = minimalPrecedence;
        }

        public Construct Parse(ParserContext context)
        {
            // If this is a binary operation, find it's precedence.
            while (true)
            {
                // Capture the current token.
                Token token = context.Stream.Current;

                // Calculate precedence for the current token.
                int firstPrecedence = Precedence.Get(token);

                /*
                If this is a binary operation that binds at least as tightly
                as the current binary operation, consume it. Otherwise, the process
                is complete.
                */
                if (firstPrecedence < this.minimalPrecedence)
                {
                    // TODO: This should throw error? Research.
                    return this.leftSide;
                }

                // At this point, it's a binary operation.
                TokenType binaryOperator = token.Type;

                // TODO: Should check if it's a BINARY operator, not just an operator.
                // Ensure the captured operator is validated.
                if (!TokenIdentifier.IsOperator(binaryOperator))
                {
                    throw context.NoticeRepository.CreateException($"Expected token to be a binary operator but got token type '{binaryOperator}'");
                }

                // Skip operator.
                context.Stream.Skip();

                // Parse the right-side.
                Construct rightSide = new PrimaryExprParser().Parse(context);

                // Ensure that the right-side was successfully parsed.
                if (rightSide == null)
                {
                    throw new Exception("Unable to parse the right-side of the binary expression");
                }

                // Determine the token precedence of the current token.
                int secondPrecedence = Precedence.Get(token);

                /*
                If binary operator binds less tightly with the right-side than
                the operator after right-side, let the pending operator take the
                right-side as its left-side.
                */
                if (firstPrecedence < secondPrecedence)
                {
                    // Invoke the right-side parser.
                    rightSide = new BinaryOpRightSideParser(rightSide, firstPrecedence + 1).Parse(context);

                    // Ensure the right-side was successfully parsed.
                    if (rightSide == null)
                    {
                        throw new Exception("Unable to parse the right-side of the binary expression");
                    }
                }

                // TODO: Identifier.
                // Create the binary expression entity.
                BinaryExpr binaryExpr = new BinaryExpr("tmp", binaryOperator, this.leftSide, rightSide, firstPrecedence);

                // Merge left-side/right-side.
                this.leftSide = binaryExpr;
            }
        }
    }
}
