using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace LucasBaran.GameTime.Generation
{
    [Generator(LanguageNames.CSharp)]
    public sealed class GameTimeGenerator : IIncrementalGenerator
    {
        private const string NAMESPACE = "LucasBaran.GameTime";
        private const string ATTRIBUTE = "GeneratedTimeAttribute";
        private const string FULLY_QUALIFIED_ATTRIBUTE = NAMESPACE + "." + ATTRIBUTE;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<Model> pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: FULLY_QUALIFIED_ATTRIBUTE,
                predicate: static (syntax_node, cancellation_token) => syntax_node is ClassDeclarationSyntax class_declaration_syntax,
                transform: static (context, cancellation_token) =>
                {
                    INamedTypeSymbol type_symbol = context.TargetSymbol.ContainingType;

                    return new Model(
                        type_symbol.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)),
                        type_symbol.Name
                    );
                }
            );

            context.RegisterSourceOutput(pipeline, static (context, model) =>
            {
                SourceText source_text = SourceText.From($$"""
                    using System.Runtime.CompilerServices;

                    namespace {{model.Namespace}}
                    {
                        public static partial class {{model.ClassName}}
                        {
                            public static TimeInfo TimeInfo { get; } = new();

                            public static float TimeScale
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => TimeInfo.TimeScale;
                            }

                            public static float Time
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => TimeInfo.Time;
                            }

                            public static float DeltaTime
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => TimeInfo.DeltaTime;
                            }

                            public static float SmoothDeltaTime
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => TimeInfo.SmoothDeltaTime;
                            }
                        }
                    }
                    """, Encoding.UTF8);
            });
        }

        private readonly struct Model
        {
            public readonly string Namespace;
            public readonly string ClassName;

            public Model(string namespace_name, string class_name)
            {
                Namespace = namespace_name;
                ClassName = class_name;
            }
        }
    }
}
