using Google.Protobuf;
using RiraPersonManagement;

namespace PersonManagementGrpcClient
{
    public static class GrpcHelper
    {
        //برای نام متد از رفلکشن به دلیل تاثیر زیاد روی کاهش عملکرد استفاده نکردم . 
       public static async Task<TResponse> CallServiceAsync<TResponse>(Func<Task<PersonServiceResponse>> serviceCall, string methodName) where TResponse : class, IMessage, new()
        {
            var response = await serviceCall();
            if (response.Error != null)
            {
                Console.WriteLine($"Error in {methodName}:  {response.Error.ErrorMessage} (Code: {response.Error.ErrorCode})");
                return null;
            }
            var data = response.Data.Unpack<TResponse>();
            return data;
        }
    }
}
