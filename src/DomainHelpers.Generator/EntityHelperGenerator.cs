﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainHelpers.Generator;

[Generator(LanguageNames.CSharp)]
public partial class EntityHelperGenerator : IIncrementalGenerator {
    const string _prefixedUlidAttributeClass = """
        namespace DomainHelpers;
        
        using System;
        
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class EntityAttribute : Attribute {
            public EntityAttribute() { 
            }
        }
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.RegisterPostInitializationOutput(static context => {
            context.AddSource("EntityAttribute.cs", _prefixedUlidAttributeClass);
        });

        var source = context.SyntaxProvider.ForAttributeWithMetadataName(
            "DomainHelpers.EntityAttribute",
            static (node, token) => true,
            static (context, token) => context
        );

        // 出力コード部分はちょっとごちゃつくので別メソッドに隔離
        context.RegisterSourceOutput(source, Emit);
    }

    static void Emit(SourceProductionContext context, GeneratorAttributeSyntaxContext source) {
        var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
        var typeNode = (TypeDeclarationSyntax)source.TargetNode;

        // グローバルネームスペース対応
        var ns = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? ""
            : $"namespace {typeSymbol.ContainingNamespace};";

        var constructorCount = typeSymbol.Constructors.First().Parameters.Length;

        var argsString = string.Join(",", Enumerable.Range(0, constructorCount).Select(x => "default!"));

        var code = $$"""
            // <auto-generated/>
            #nullable enable
            #pragma warning disable CS8600
            #pragma warning disable CS8601
            #pragma warning disable CS8602
            #pragma warning disable CS8603
            #pragma warning disable CS8604

            {{ns}}

            partial class {{typeSymbol.Name}} {
                private {{typeSymbol.Name}}(): this({{argsString}}) { }
            }
            """;


        // 出力ファイル名として使ためのエスケープ
        var fullType = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", "")
            .Replace("<", "_")
            .Replace(">", "_");

        context.AddSource($"{fullType}.g.cs", code);
    }
}
