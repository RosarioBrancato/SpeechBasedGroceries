// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace SpeechBasedGroceries.Parties.Fridgy.Client
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Add.
    /// </summary>
    public static partial class AddExtensions
    {
            /// <summary>
            /// Add users as owners to a fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// UUID of user that should be added as owner
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            public static Fridge Owners(this IAdd operations, Owner body, string uuid)
            {
                return operations.OwnersAsync(body, uuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add users as owners to a fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// UUID of user that should be added as owner
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Fridge> OwnersAsync(this IAdd operations, Owner body, string uuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.OwnersWithHttpMessagesAsync(body, uuid, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds a new item into the fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// New item that should be created.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            public static Item FridgeMethod(this IAdd operations, BaseItem body, string uuid)
            {
                return operations.FridgeMethodAsync(body, uuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds a new item into the fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// New item that should be created.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Item> FridgeMethodAsync(this IAdd operations, BaseItem body, string uuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.FridgeMethodWithHttpMessagesAsync(body, uuid, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
