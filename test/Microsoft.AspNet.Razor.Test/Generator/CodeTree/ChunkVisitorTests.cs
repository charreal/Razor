﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !DNXCORE50
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;
using Microsoft.AspNet.Razor.Parser;
using Moq;
using Moq.Protected;
using Xunit;

namespace Microsoft.AspNet.Razor
{
    public class ChunkVisitorTests
    {
        [Fact]
        public void Accept_InvokesAppropriateOverload()
        {
            // Arrange
            var chunks = new Chunk[] { new LiteralChunk(), new StatementChunk() };
            var visitor = CreateVisitor();

            // Act
            visitor.Object.Accept(chunks);

            // Assert
            visitor.Protected().Verify("Visit", Times.AtMostOnce(), chunks[0]);
            visitor.Protected().Verify("Visit", Times.AtMostOnce(), chunks[1]);
        }

        private static Mock<ChunkVisitor<CodeWriter>> CreateVisitor()
        {
            var codeBuilderContext = new CodeBuilderContext(
                new RazorEngineHost(new CSharpRazorCodeLanguage()),
                "myclass",
                "myns",
                string.Empty,
                shouldGenerateLinePragmas: false,
                errorSink: new ParserErrorSink());
            var writer = Mock.Of<CodeWriter>();
            return new Mock<ChunkVisitor<CodeWriter>>(writer, codeBuilderContext);
        }

        private class MyTestChunk : Chunk
        {
        }
    }
}
#endif
