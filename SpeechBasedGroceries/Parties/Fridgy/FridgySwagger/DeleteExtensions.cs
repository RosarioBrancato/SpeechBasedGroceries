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
    /// Extension methods for Delete.
    /// </summary>
    public static partial class DeleteExtensions
    {
            /// <summary>
            /// Delete an existing product
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='barcode'>
            /// Barcode used as unique identifier to find product
            /// </param>
            public static void Products(this IDelete operations, string barcode)
            {
                operations.ProductsAsync(barcode).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete an existing product
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='barcode'>
            /// Barcode used as unique identifier to find product
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ProductsAsync(this IDelete operations, string barcode, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ProductsWithHttpMessagesAsync(barcode, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Delete existing fridge
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            public static void FridgeMethod(this IDelete operations, string uuid)
            {
                operations.FridgeMethodAsync(uuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete existing fridge
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task FridgeMethodAsync(this IDelete operations, string uuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.FridgeMethodWithHttpMessagesAsync(uuid, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Remove user from the owners of a fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='userUuid'>
            /// UUID of user
            /// </param>
            public static void Owners(this IDelete operations, string uuid, string userUuid)
            {
                operations.OwnersAsync(uuid, userUuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Remove user from the owners of a fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='userUuid'>
            /// UUID of user
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task OwnersAsync(this IDelete operations, string uuid, string userUuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.OwnersWithHttpMessagesAsync(uuid, userUuid, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Remove an item from the fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='itemUuid'>
            /// UUID of item
            /// </param>
            public static void FridgeMethod1(this IDelete operations, string uuid, string itemUuid)
            {
                operations.FridgeMethod1Async(uuid, itemUuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Remove an item from the fridge.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// UUID to identify a fridge
            /// </param>
            /// <param name='itemUuid'>
            /// UUID of item
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task FridgeMethod1Async(this IDelete operations, string uuid, string itemUuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.FridgeMethod1WithHttpMessagesAsync(uuid, itemUuid, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Remove user.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// uuid identifying the user
            /// </param>
            public static void Users(this IDelete operations, string uuid)
            {
                operations.UsersAsync(uuid).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Remove user.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='uuid'>
            /// uuid identifying the user
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UsersAsync(this IDelete operations, string uuid, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UsersWithHttpMessagesAsync(uuid, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}
