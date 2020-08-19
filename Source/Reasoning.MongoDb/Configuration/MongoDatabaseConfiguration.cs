using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Conventions;

using Reasoning.Core.Models;
using Reasoning.Core.Contracts;

namespace Reasoning.MongoDb.Configuration
{
    public static class MongoDatabaseConfiguration
    {
        public static void ConfigureBsonSerializers()
        {
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IReasoningProcess, ReasoningProcess>(BsonSerializer.LookupSerializer<ReasoningProcess>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IKnowledgeBase, KnowledgeBase>(BsonSerializer.LookupSerializer<KnowledgeBase>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IRule, Rule>(BsonSerializer.LookupSerializer<Rule>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IPredicate, Predicate>(BsonSerializer.LookupSerializer<Predicate>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IVariable, Variable>(BsonSerializer.LookupSerializer<Variable>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IVariableSource, VariableSource>(BsonSerializer.LookupSerializer<VariableSource>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IReasoningAction, ReasoningAction>(BsonSerializer.LookupSerializer<ReasoningAction>()));
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<IReasoningRequest, ReasoningRequest>(BsonSerializer.LookupSerializer<ReasoningRequest>()));
        }

        public static void ConfigureConventionRegistry()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }
    }
}
