using System;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace Conventional.Conventions.Cecil
{
    public class LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasksConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "Libraries must call Task.ConfigureAwait(false) to prevent deadlocks";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var failures = type.ToTypeDefinition()
                .Methods
                .Where(HasAttribute<AsyncStateMachineAttribute>)
                .Where(AwaitingTasksWithoutConfigureAwait);

            if (failures.Any())
            {
                var details = failures.Aggregate(string.Empty, (s, info) => s + "\t- " + type.Name + "." + info.Name + Environment.NewLine);
                var failureMessage = BuildFailureMessage(details);

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }

        
        /// <summary>
        /// First we get the type of the compiler generated state machine. This will contain
        /// a MoveNext method containing the implementation.
        /// We then compare the number of calls to async methods (GetAwaiter) to the number
        /// of calls to ConfigureAwait.
        /// see: http://www.codeproject.com/Articles/535635/Async-Await-and-the-Generated-StateMachine
        /// </summary>
        private static bool AwaitingTasksWithoutConfigureAwait(MethodDefinition subject)
        {
            bool IsGetAwaiterCall(Instruction instruction) => IsAsyncMethodCall(instruction, "GetAwaiter");
            bool IsConfigureAwaitCall(Instruction instruction) => IsAsyncMethodCall(instruction, "ConfigureAwait");

            var numberOfGetAwaiterCallsWithoutAConfigureAwaitCall = GetAsyncStateMachineType(subject)
                                        .Methods
                                        .Single(m => m.Name == "MoveNext")
                                        .Body
                                        .Instructions
                                        .Aggregate(0, (sum, next) => IsGetAwaiterCall(next) 
                                                            ? sum + 1 
                                                            : IsConfigureAwaitCall(next) ? sum - 1 : sum);

            return numberOfGetAwaiterCallsWithoutAConfigureAwaitCall > 0;
        }

        private static bool IsAsyncMethodCall(Instruction instruction, string methodName)
        {
            return (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt) &&
                    instruction.Operand is MethodReference &&
                    (
                        ((MethodReference)instruction.Operand).DeclaringType.Resolve().FullName == "System.Threading.Tasks.Task`1" ||
                        ((MethodReference)instruction.Operand).DeclaringType.Resolve().FullName == "System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1"
                    ) &&
                    ((MethodReference)instruction.Operand).Name == methodName;
        }

        private static bool HasAttribute<TAttribute>(MethodDefinition subject) where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(subject) != null;
        }

        private static CustomAttribute GetAttribute<TAttribute>(MethodDefinition subject) where TAttribute : Attribute
        {
            return subject.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType.Name == typeof(TAttribute).Name);
        }

        /// <summary>
        /// An async method will have an AsyncStateMachine attribute pointing to the generated async state machine type 
        /// for example [AsyncStateMachine(typeof(AsyncMethods.<DownloadHtmlAsyncTask>d__0))]
        /// see: http://www.codeproject.com/Articles/535635/Async-Await-and-the-Generated-StateMachine
        /// </summary>
        private static TypeDefinition GetAsyncStateMachineType(MethodDefinition provider)
        {
            var asyncStateMachineAttribute = GetAttribute<AsyncStateMachineAttribute>(provider);
            return (TypeDefinition)asyncStateMachineAttribute.ConstructorArguments[0].Value;
        }
    }
}