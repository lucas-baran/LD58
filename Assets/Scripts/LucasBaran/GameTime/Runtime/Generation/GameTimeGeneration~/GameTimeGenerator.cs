using Microsoft.CodeAnalysis;
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
                predicate: static (syntax_node, cancellation_token) => true,
                transform: static (context, cancellation_token) =>
                {
                    INamedTypeSymbol type_symbol = context.TargetSymbol.ContainingType;

                    return new Model(
                        type_symbol.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)),
                        type_symbol.Name,
                        context.TargetSymbol.Name
                    );
                }
            );

            context.RegisterSourceOutput(pipeline, static (context, model) =>
            {
                string property_path = $"{model.ClassName}.{model.PropertyName}";

                SourceText source_text = SourceText.From($$"""
                    using System.Runtime.CompilerServices;

                    namespace {{model.Namespace}}
                    {
                        public static class {{model.PropertyName}}
                        {
                            public static float TimeScale
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => {{property_path}}.TimeScale;
                            }

                            public static float Time
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => {{property_path}}.Time;
                            }

                            public static float DeltaTime
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => {{property_path}}.DeltaTime;
                            }

                            public static float SmoothDeltaTime
                            {
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                get => {{property_path}}.SmoothDeltaTime;
                            }
                        }
                    }
                    """, Encoding.UTF8);

                context.AddSource($"{model.PropertyName}.g.cs", source_text);
            });
        }

        private readonly struct Model
        {
            public readonly string Namespace;
            public readonly string ClassName;
            public readonly string PropertyName;

            public Model(string namespace_name, string class_name, string property_name)
            {
                Namespace = namespace_name;
                ClassName = class_name;
                PropertyName = property_name;
            }
        }
    }
}
